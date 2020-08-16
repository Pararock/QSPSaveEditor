#include "pch.h"
#include "Engine.h"
#include "Engine.g.cpp"
#include "Location.h"
#include "Variable.h"

namespace winrt::QSPLib_CppWinrt::implementation
{
    using namespace std;
    using namespace std::chrono;

    using namespace winrt::Windows::Storage;
    using namespace winrt::Windows::System;
    using namespace winrt::Windows::Storage::Streams;
    using namespace winrt::Windows::Security::Cryptography;
    using namespace Windows::Foundation;

    Engine* Engine::Instance = nullptr;

    //from https://stackoverflow.com/questions/1765014/convert-string-from-date-into-a-time-t
    time_t time_when_compiled()
    {
        string datestr = __DATE__;
        string timestr = __TIME__;

        istringstream iss_date(datestr);
        string str_month;
        int day;
        int year;
        iss_date >> str_month >> day >> year;

        int month;
        if (str_month == "Jan") month = 1;
        else if (str_month == "Feb") month = 2;
        else if (str_month == "Mar") month = 3;
        else if (str_month == "Apr") month = 4;
        else if (str_month == "May") month = 5;
        else if (str_month == "Jun") month = 6;
        else if (str_month == "Jul") month = 7;
        else if (str_month == "Aug") month = 8;
        else if (str_month == "Sep") month = 9;
        else if (str_month == "Oct") month = 10;
        else if (str_month == "Nov") month = 11;
        else if (str_month == "Dec") month = 12;
        else exit(-1);

        for (string::size_type pos = timestr.find(':'); pos != string::npos; pos = timestr.find(':', pos))
            timestr[pos] = ' ';
        istringstream iss_time(timestr);
        int hour, min, sec;
        iss_time >> hour >> min >> sec;

        tm t = { 0 };
        t.tm_mon = month - 1;
        t.tm_mday = day;
        t.tm_year = year - 1900;
        t.tm_hour = hour - 1;
        t.tm_min = min;
        t.tm_sec = sec;
        return mktime(&t);
    }

    Engine::Engine()
    {
        qspIsDebug = QSP_FALSE;
        qspRefreshCount = qspFullRefreshCount = 0;
        qspQstPath = qspQstFullPath = 0;
        qspQstPathLen = 0;
        qspQstCRC = 0;
        qspRealCurLoc = -1;
        qspRealActIndex = -1;
        qspRealLine = 0;
        qspMSCount = 0;
        qspLocs = 0;
        qspLocsNames = 0;
        qspLocsCount = 0;
        qspCurLoc = -1;
        qspTimerInterval = 0;
        qspCurIsShowObjs = qspCurIsShowActs = qspCurIsShowVars = qspCurIsShowInput = QSP_TRUE;
        setlocale(LC_ALL, QSP_LOCALE);
        qspSetSeed(0);
        qspPrepareExecution();
        qspMemClear(QSP_TRUE);
        qspInitCallBacks();
        qspInitStats();
        qspInitMath();

        m_locations = winrt::single_threaded_observable_vector<winrt::QSPLib_CppWinrt::Location>();
        m_variables = winrt::single_threaded_observable_vector<winrt::QSPLib_CppWinrt::Variable>();
        Instance = this;
    }

    Engine& Engine::getInstance()
    {
        return *Instance;
    }

    Engine::~Engine()
    {
        Engine::CloseGameWorld();
    }

    //Engine Engine::Current()
    //{
    //    if (Instance != nullptr) {
    //        Instance = new Engine();
    //    }

    //    return *Instance;
    //}

    void Engine::CloseGameWorld()
    {
        qspMemClear(QSP_FALSE);
        qspCreateWorld(0, 0);
        if (qspQstPath) free(qspQstPath);
        if (qspQstFullPath) free(qspQstFullPath);
    }

    Windows::Foundation::IAsyncOperation<QSPLib_CppWinrt::Result> Engine::LoadGameWorld(Windows::Storage::StorageFile const questGameWorld)
    {
        winrt::apartment_context ui_thread;
        co_await winrt::resume_background();
        if (qspIsExitOnError && qspErrorNum) return ReturnQSPError(qspErrorNum);
        qspResetError();
        if (qspIsDisableCodeExec) return ReturnQSPError(static_cast<int>(StatusCode::QSP_ERR_CODEEXEDISABLE));

        char* buf;

        IBuffer buffer = co_await FileIO::ReadBufferAsync(questGameWorld);
        com_array<uint8_t> byteBuffer;
        CryptographicBuffer::CopyToByteArray(buffer, byteBuffer);
        auto filesize = buffer.Length();
        buf = (char*)malloc(filesize + 3);
        memcpy(buf, byteBuffer.data(), filesize);
        buf[filesize] = buf[filesize + 1] = buf[filesize + 2] = 0;
        qspOpenQuestFromData(buf, filesize + 3, questGameWorld.Path().c_str(), QSP_FALSE);
        free(buf);

        co_await ui_thread;
        if (qspErrorNum)
        {
            // already had a file opened
            if (m_currentFile != nullptr) {
                m_currentFile = nullptr;
                m_propertyChanged(*this, Windows::UI::Xaml::Data::PropertyChangedEventArgs{ L"CurrentGame" });
            }
            return ReturnQSPError(qspErrorNum);
        }

        if (m_currentFile == nullptr || m_currentFile.Path() != questGameWorld.Path()) {

            // try to use parent of the storage file. If it's not null it mean we have full access right set in settings

            auto parents = co_await questGameWorld.GetParentAsync();

            if (parents != nullptr)
            {
                m_haveFullAccessPermissions = true;
            }
            else {
                m_haveFullAccessPermissions = false;
            }
            m_currentFile = questGameWorld;
            m_propertyChanged(*this, Windows::UI::Xaml::Data::PropertyChangedEventArgs{ L"CurrentGame" });
            InitializeLocation();
        }

        return ReturnSuccess();
    }

    QSPLib_CppWinrt::Result Engine::ReturnQSPError(int errorNum) {
        Result error;

        error.Location.Location = qspErrorLoc;
        error.Location.Line = qspErrorLine;
        error.Location.ActionIndex = qspErrorActIndex;
        // Legacy error from QSP Engine
        if (errorNum >= 100) {
            error.Message = qspGetErrorDesc(errorNum);
        }
        else {
            switch (errorNum)
            {
            case QSPLib_CppWinrt::StatusCode::QSP_ERR_CODEEXEDISABLE:
                // I have no idea when we would hit that point
                error.Message = L"Code execution is disabled";
                break;
            case QSPLib_CppWinrt::StatusCode::QSP_CANCELED:
                error.Message = L"Canceled by user";
                break;
            default:
                error.Message = L"Unknown Error";
                break;
            }
        }

        error.Code = static_cast<StatusCode>(errorNum);

        return error;
    }

    QSPLib_CppWinrt::Result Engine::ReturnSuccess()
    {
        Result result;
        result.Message = L"";
        result.Code = StatusCode::QSP_SUCCESS;
        return result;
    }

    Windows::Foundation::IAsyncOperation<QSPLib_CppWinrt::Result> Engine::OpenSavedGame(Windows::Storage::StorageFile const saveGame) {
        winrt::apartment_context ui_thread;

        if (qspIsExitOnError && qspErrorNum) co_return ReturnQSPError(qspErrorNum);
        qspPrepareExecution();
        if (qspIsDisableCodeExec) co_return ReturnQSPError(static_cast<int>(StatusCode::QSP_ERR_CODEEXEDISABLE));

        co_await winrt::resume_background();
        IBuffer buffer = co_await FileIO::ReadBufferAsync(saveGame);
        // Throws HRESULT_FROM_WIN32(ERROR_NO_UNICODE_TRANSLATION)
        // if the buffer is not properly encoded.
        hstring fileContent = CryptographicBuffer::ConvertBinaryToString(BinaryStringEncoding::Utf16LE, buffer);

        
        qspOpenGameStatusFromString(fileContent.c_str());

        if (qspErrorNum)
        {
            co_await ui_thread;
            co_return ReturnQSPError(qspErrorNum);
        }

        // Should we only property change when it's refresh? And what is refresh?
        //if (isRefresh) qspCallRefreshInt(QSP_FALSE);
        InitializeVariables();
        co_await ui_thread;
        m_currentSave = saveGame;
        m_propertyChanged(*this, Windows::UI::Xaml::Data::PropertyChangedEventArgs{ L"CurrentLocation" });
        m_propertyChanged(*this, Windows::UI::Xaml::Data::PropertyChangedEventArgs{ L"CurrentSave" });
        m_propertyChanged(*this, Windows::UI::Xaml::Data::PropertyChangedEventArgs{ L"Variables" });
        m_isGameDirty = false;
        m_propertyChanged(*this, Windows::UI::Xaml::Data::PropertyChangedEventArgs{ L"isGameDirty" });
        co_return ReturnSuccess();
    }

    int qspCodeWriteValToStream(const DataWriter& dataWriter, QSP_CHAR* val, QSP_BOOL isCode)
    {
        QSP_CHAR* temp;
        int len = 0;
        if (val)
        {
            if (isCode)
            {
                temp = qspCodeReCode(val, QSP_TRUE);
                len = dataWriter.WriteString((wchar_t*)temp);
                free(temp);
            }
            else
                len = dataWriter.WriteString((wchar_t*)val);
        }

        dataWriter.WriteString(QSP_STRSDELIM);
        
        return len + std::char_traits<QSP_CHAR>::length(QSP_STRSDELIM);
    }

    int qspCodeWriteIntValToStream(const DataWriter& dataWriter, int val, QSP_BOOL isCode)
    {
        QSP_CHAR* temp;

        int len = 0;
        auto tempFormatedStrig = fmt::format(L"{}", val);

        if (isCode)
        {
            temp = qspCodeReCode((QSP_CHAR*)tempFormatedStrig.c_str(), QSP_TRUE);
            len = dataWriter.WriteString((wchar_t*)temp);
            free(temp);
        }
        else
            len = dataWriter.WriteString(tempFormatedStrig);

        dataWriter.WriteString(QSP_STRSDELIM);

        return len + std::char_traits<QSP_CHAR>::length(QSP_STRSDELIM);
    }

    int qspSaveGameStatusToStream(const DataWriter& dataWriter)
    {
        QSP_CHAR* locName;
        QSPVar* savedVars;
        int i, j, len, varsCount, oldRefreshCount = qspRefreshCount;

        qspExecLocByVarNameWithArgs(QSP_FMT("ONGSAVE"), 0, 0);
        if (qspRefreshCount != oldRefreshCount || qspErrorNum) return 0;
        varsCount = qspPrepareLocalVars(&savedVars);
        if (qspErrorNum) return 0;

        qspRefreshPlayList();
        locName = (qspCurLoc >= 0 ? qspLocs[qspCurLoc].Name : 0);
        len = qspCodeWriteValToStream(dataWriter, QSP_SAVEDGAMEID, QSP_FALSE);
        len += qspCodeWriteValToStream(dataWriter, QSP_VER, QSP_FALSE);
        len += qspCodeWriteIntValToStream(dataWriter, qspQstCRC, QSP_TRUE);
        len += qspCodeWriteIntValToStream(dataWriter, qspGetTime(), QSP_TRUE);
        len += qspCodeWriteIntValToStream(dataWriter, qspCurSelAction, QSP_TRUE);
        len += qspCodeWriteIntValToStream(dataWriter, qspCurSelObject, QSP_TRUE);
        len += qspCodeWriteValToStream(dataWriter, qspViewPath, QSP_TRUE);
        len += qspCodeWriteValToStream(dataWriter, qspCurInput, QSP_TRUE);
        len += qspCodeWriteValToStream(dataWriter, qspCurDesc, QSP_TRUE);
        len += qspCodeWriteValToStream(dataWriter, qspCurVars, QSP_TRUE);
        len += qspCodeWriteValToStream(dataWriter, locName, QSP_TRUE);
        len += qspCodeWriteIntValToStream(dataWriter, (int)qspCurIsShowActs, QSP_TRUE);
        len += qspCodeWriteIntValToStream(dataWriter, (int)qspCurIsShowObjs, QSP_TRUE);
        len += qspCodeWriteIntValToStream(dataWriter, (int)qspCurIsShowVars, QSP_TRUE);
        len += qspCodeWriteIntValToStream(dataWriter, (int)qspCurIsShowInput, QSP_TRUE);
        len += qspCodeWriteIntValToStream(dataWriter, qspTimerInterval, QSP_TRUE);
        len += qspCodeWriteIntValToStream(dataWriter, qspPLFilesCount, QSP_TRUE);
        for (i = 0; i < qspPLFilesCount; ++i)
            len += qspCodeWriteValToStream(dataWriter, qspPLFiles[i], QSP_TRUE);
        len += qspCodeWriteIntValToStream(dataWriter, qspCurIncFilesCount, QSP_TRUE);
        for (i = 0; i < qspCurIncFilesCount; ++i)
            len += qspCodeWriteValToStream(dataWriter, qspCurIncFiles[i], QSP_TRUE);
        len += qspCodeWriteIntValToStream(dataWriter, qspCurActionsCount, QSP_TRUE);
        for (i = 0; i < qspCurActionsCount; ++i)
        {
            if (qspCurActions[i].Image)
                len += qspCodeWriteValToStream(dataWriter, qspCurActions[i].Image + qspQstPathLen, QSP_TRUE);
            else
                len += qspCodeWriteValToStream(dataWriter, 0, QSP_FALSE);
            len += qspCodeWriteValToStream(dataWriter, qspCurActions[i].Desc, QSP_TRUE);
            len += qspCodeWriteIntValToStream(dataWriter, qspCurActions[i].OnPressLinesCount, QSP_TRUE);
            for (j = 0; j < qspCurActions[i].OnPressLinesCount; ++j)
            {
                len += qspCodeWriteValToStream(dataWriter, qspCurActions[i].OnPressLines[j].Str, QSP_TRUE);
                len += qspCodeWriteIntValToStream(dataWriter, qspCurActions[i].OnPressLines[j].LineNum, QSP_TRUE);
            }
            len += qspCodeWriteIntValToStream(dataWriter, qspCurActions[i].Location, QSP_TRUE);
            len += qspCodeWriteIntValToStream(dataWriter, qspCurActions[i].ActIndex, QSP_TRUE);
            len += qspCodeWriteIntValToStream(dataWriter, qspCurActions[i].StartLine, QSP_TRUE);
            len += qspCodeWriteIntValToStream(dataWriter, (int)qspCurActions[i].IsManageLines, QSP_TRUE);
        }
        len += qspCodeWriteIntValToStream(dataWriter, qspCurObjectsCount, QSP_TRUE);
        for (i = 0; i < qspCurObjectsCount; ++i)
        {
            if (qspCurObjects[i].Image)
                len += qspCodeWriteValToStream(dataWriter, qspCurObjects[i].Image + qspQstPathLen, QSP_TRUE);
            else
                len += qspCodeWriteValToStream(dataWriter, 0, QSP_FALSE);
            len += qspCodeWriteValToStream(dataWriter, qspCurObjects[i].Desc, QSP_TRUE);
        }
        len += qspCodeWriteIntValToStream(dataWriter, qspGetVarsCount(), QSP_TRUE);
        for (i = 0; i < QSP_VARSCOUNT; ++i)
            if (qspVars[i].Name)
            {
                len += qspCodeWriteIntValToStream(dataWriter, i, QSP_TRUE);
                len += qspCodeWriteValToStream(dataWriter, qspVars[i].Name, QSP_TRUE);
                len += qspCodeWriteIntValToStream(dataWriter, qspVars[i].ValsCount, QSP_TRUE);
                for (j = 0; j < qspVars[i].ValsCount; ++j)
                {
                    len += qspCodeWriteIntValToStream(dataWriter, qspVars[i].Values[j].Num, QSP_TRUE);
                    len += qspCodeWriteValToStream(dataWriter, qspVars[i].Values[j].Str, QSP_TRUE);
                }
                len += qspCodeWriteIntValToStream(dataWriter, qspVars[i].IndsCount, QSP_TRUE);
                for (j = 0; j < qspVars[i].IndsCount; ++j)
                {
                    len += qspCodeWriteIntValToStream(dataWriter, qspVars[i].Indices[j].Index, QSP_TRUE);
                    len += qspCodeWriteValToStream(dataWriter, qspVars[i].Indices[j].Str, QSP_TRUE);
                }
            }
        qspRestoreLocalVars(savedVars, varsCount, qspSavedVarsGroups, qspSavedVarsGroupsCount);
        if (qspErrorNum)
        {
            return 0;
        }
        return len;
    }

    IAsyncOperationWithProgress<QSPLib_CppWinrt::Result, double> Engine::SaveState(StorageFile saveGame)
    {
        auto progress{ co_await winrt::get_progress_token() };
        auto cancellation = co_await winrt::get_cancellation_token();
        winrt::apartment_context ui_thread;
        auto localSave{ saveGame };



        if (qspIsExitOnError && qspErrorNum) co_return ReturnQSPError(qspErrorNum);
        qspPrepareExecution();
        if (qspIsDisableCodeExec) co_return ReturnQSPError(static_cast<int>(StatusCode::QSP_ERR_CODEEXEDISABLE));

        co_await winrt::resume_background();

        progress(0.01);
        auto transaction = co_await localSave.OpenTransactedWriteAsync();

//        cancellation.callback([transaction] {
//                transaction.Close();
//                co_return ReturnQSPError(static_cast<int>(QSPLib_CppWinrt::StatusCode::QSP_CANCELED));
//            });

        auto stream = transaction.Stream();
        DataWriter dataWriter{ stream };

        dataWriter.UnicodeEncoding(UnicodeEncoding::Utf16LE);
        dataWriter.ByteOrder(ByteOrder::LittleEndian);

        progress(0.05);

        auto len = qspSaveGameStatusToStream(dataWriter);

        progress(0.5);

        if (qspErrorNum || cancellation())
        {
            transaction.Close();

            //co_await ui_thread;
            if(qspErrorNum)
                co_return ReturnQSPError(qspErrorNum);
            else 
                co_return ReturnQSPError(static_cast<int>(QSPLib_CppWinrt::StatusCode::QSP_CANCELED));
        }


        auto result = co_await dataWriter.StoreAsync();
        progress(0.75);

        // Make sure we have written the number of characters we were expecting
        assert(static_cast<int>(result / 2) == len);
                
        stream.Size(result);

        // Last chance to cancel before we commit
        if (cancellation())
        {
            transaction.Close();
            co_return ReturnQSPError(static_cast<int>(QSPLib_CppWinrt::StatusCode::QSP_CANCELED));                    
        }

        // We can still be canceled here and crash.
        co_await transaction.CommitAsync();
        transaction.Close();
        m_isGameDirty = false;
        progress(1.0);
        co_await ui_thread;
        m_propertyChanged(*this, Windows::UI::Xaml::Data::PropertyChangedEventArgs{ L"isGameDirty" });


        if (false) qspCallRefreshInt(QSP_FALSE);
        co_return ReturnSuccess();
    }

    Windows::Storage::StorageFile Engine::CurrentSave() const {
        return m_currentSave;
    }

    QSPLib_CppWinrt::Result Engine::ExecuteCommandString(hstring command, bool isRefresh)
    {
        if (qspIsExitOnError && qspErrorNum) return ReturnQSPError(qspErrorNum);
        qspPrepareExecution();
        if (qspIsDisableCodeExec) return ReturnQSPError(static_cast<int>(StatusCode::QSP_ERR_CODEEXEDISABLE));
        
        qspExecStringAsCodeWithArgs((QSP_CHAR*)command.c_str(), 0, 0, 0);
		
        if (qspErrorNum) return ReturnQSPError(qspErrorNum);
        if (isRefresh) qspCallRefreshInt(QSP_FALSE);
        return ReturnSuccess();
    }

    QSPLib_CppWinrt::Result Engine::SelectObject(int objectIndex, bool isRefresh)
    {
        if (objectIndex >= 0 && objectIndex < qspCurObjectsCount && objectIndex != qspCurSelObject)
        {
            if (qspIsExitOnError && qspErrorNum) return ReturnQSPError(qspErrorNum);
            qspPrepareExecution();
            if (qspIsDisableCodeExec) return ReturnQSPError(static_cast<int>(StatusCode::QSP_ERR_CODEEXEDISABLE));

            qspCurSelObject = objectIndex;
            qspExecLocByVarNameWithArgs(QSP_FMT("ONOBJSEL"), 0, 0);

            if (qspErrorNum) return ReturnQSPError(qspErrorNum);
            if (isRefresh) qspCallRefreshInt(QSP_FALSE);
        }
        return ReturnSuccess();
    }

    QSPLib_CppWinrt::Result Engine::SelectAction(int objectIndex, bool isRefresh)
    {
        if (objectIndex >= 0 && objectIndex < qspCurActionsCount && objectIndex != qspCurSelAction)
        {
            if (qspIsExitOnError && qspErrorNum) return ReturnQSPError(qspErrorNum);
            qspPrepareExecution();
            if (qspIsDisableCodeExec) return ReturnQSPError(static_cast<int>(StatusCode::QSP_ERR_CODEEXEDISABLE));

            qspCurSelAction = objectIndex;
            qspExecLocByVarNameWithArgs(QSP_FMT("ONACTSEL"), 0, 0);

            if (qspErrorNum) return ReturnQSPError(qspErrorNum);
            if (isRefresh) qspCallRefreshInt(QSP_FALSE);
        }

        return ReturnSuccess();
    }

    QSPLib_CppWinrt::Result Engine::ExecuteSelectedAction(bool isRefresh)
    {
        if (qspCurSelAction >= 0)
        {
            if (qspIsExitOnError && qspErrorNum) return ReturnQSPError(qspErrorNum);
            qspPrepareExecution();
            if (qspIsDisableCodeExec) return ReturnQSPError(static_cast<int>(StatusCode::QSP_ERR_CODEEXEDISABLE));

            qspExecAction(qspCurSelAction);

            if (qspErrorNum) return ReturnQSPError(qspErrorNum);
            if (isRefresh) qspCallRefreshInt(QSP_FALSE);
        }
        return ReturnSuccess();
    }

    QSPLib_CppWinrt::Result Engine::StartGame(bool isRefresh)
    {
        if (qspIsExitOnError && qspErrorNum) return ReturnQSPError(qspErrorNum);
        qspPrepareExecution();
        if (qspIsDisableCodeExec) return ReturnQSPError(static_cast<int>(StatusCode::QSP_ERR_CODEEXEDISABLE));

        qspNewGame(QSP_TRUE);

        if (qspErrorNum) return ReturnQSPError(qspErrorNum);
        if (isRefresh) qspCallRefreshInt(QSP_FALSE);
        return ReturnSuccess();
    }

    void Engine::InitializeLocation() {
        m_locations.Clear();
        for (int i = 0; i < qspLocsCount; i++)
        {
            m_locations.Append(winrt::make<winrt::QSPLib_CppWinrt::implementation::Location>(i));
        }
        m_propertyChanged(*this, Windows::UI::Xaml::Data::PropertyChangedEventArgs{ L"Locations" });
    }

    void Engine::InitializeVariables() {
        m_variables.Clear();

        int totalVariable = 0;
        for (int i = 0; i < QSP_VARSCOUNT; i++)
        {
            if (qspVars[i].Name != nullptr)
            {
                for(int j = 0; j < qspVars[i].ValsCount; j++)
                { 
                    m_variables.Append(winrt::make<winrt::QSPLib_CppWinrt::implementation::Variable>(i , totalVariable, j));
                    totalVariable++;
                }
                
            }

        }
    }

    Windows::Foundation::Collections::IObservableVector<winrt::QSPLib_CppWinrt::Location> Engine::Locations()
    {
        return m_locations;
    }

    Windows::Foundation::Collections::IObservableVector<winrt::QSPLib_CppWinrt::Variable> Engine::Variables()
    {
        return m_variables;
    }

    winrt::event_token Engine::PropertyChanged(Windows::UI::Xaml::Data::PropertyChangedEventHandler const& handler)
    {
        return m_propertyChanged.add(handler);
    }
    void Engine::PropertyChanged(winrt::event_token const& token) noexcept
    {
        m_propertyChanged.remove(token);
    }

    void Engine::SendVariablesPropertyChanges(int Index, int virtualIndex, int position)
    {
        auto variablesPropertyName = fmt::format(L"Variables[{}]", virtualIndex);
        m_isGameDirty = true;
        m_propertyChanged(*this, Windows::UI::Xaml::Data::PropertyChangedEventArgs{ variablesPropertyName });
        m_propertyChanged(*this, Windows::UI::Xaml::Data::PropertyChangedEventArgs{ L"isGameDirty" });
    }

    /// @brief Return the Version of the engine
    /// @return 
    hstring Engine::Version()
    {
        return QSP_VER;
    }

    Windows::Storage::StorageFile Engine::CurrentGame() const
    {
        return m_currentFile;
    }

    hstring Engine::MainView() const
    {
        //auto agile{ winrt::make_agile(qspCurDesc) };
        //return qspCurDesc;
        hstring fromWideLiteralWithSize{ qspCurDesc, (size_t)qspCurDescLen };
        return fromWideLiteralWithSize;
    }

    /// @brief Is file in the same folder as the qsp are readable due to any permissions issue
    /// @return 
    bool Engine::isFileAccessSafe() const
    {
        return m_haveFullAccessPermissions;
    }

    bool Engine::isWorldDirty() const
    {
        return m_isWorldDirty;
    }

    bool Engine::isGameDirty() const
    {
        return m_isGameDirty;
    }

    IAsyncOperation<IInputStream> Engine::MainViewStream() const
    {
        int size_needed = WideCharToMultiByte(CP_UTF8, 0, qspCurDesc, -1, NULL, 0, NULL, NULL);
        std::string strTo(size_needed, 0);
        WideCharToMultiByte(CP_UTF8, 0, qspCurDesc, -1, &strTo[0], size_needed, NULL, NULL);
        InMemoryRandomAccessStream stream;
        DataWriter dataWriter{ stream };

        for (long i = 0; i < size_needed; i++)
            dataWriter.WriteByte(strTo[i]);

        co_return stream.GetInputStreamAt(0);
    }

    hstring Engine::CurrentLocation() const
    {
        return (qspCurLoc >= 0 ? qspLocs[qspCurLoc].Name : L"");
    }

    Windows::Foundation::DateTime Engine::CompiledDate()
    {
        return winrt::clock::from_time_t(time_when_compiled());
    }
}
