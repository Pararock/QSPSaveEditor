namespace QSPNETWrapper
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Diagnostics;
    using Serilog;
    using System.Timers;

    public class QSPGameWorld : QSPGame
    {

        private static ILogger logger;
        public QSPWrapper qspWrapper;

        private Dictionary<string, QSPVariable> _variableList;
        private bool isGameWorldActive;
        private bool isGameWorldLoaded;

        private string currentSaveFile = string.Empty;

        private static Stopwatch stopWatch;
        private static Timer timer;

        private bool processedEvents = true;

        private BindingList<QSPAction> actionList;

        private BindingList<QSPObject> objectList;

        private Queue<String> lastDebugCommands;

        private static QSP_CALL_DEBUG callDebug;
        private static QSP_CALL_ISPLAYINGFILE callIsPlayingFile;
        private static QSP_CALL_PLAYFILE callPlayFile;
        private static QSP_CALL_CLOSEFILE callCloseFile;
        private static QSP_CALL_SHOWIMAGE callShowImage;
        private static QSP_CALL_SHOWWINDOW callShowWindow;
        private static QSP_CALL_DELETEMENU callDeleteMenu;
        private static QSP_CALL_ADDMENUITEM callAddMenuItem;
        private static QSP_CALL_SHOWMENU callShowMenu;
        private static QSP_CALL_SHOWMSGSTR callShowMessage;
        private static QSP_CALL_REFRESHINT callRefreshInt;
        private static QSP_CALL_SETTIMER callSetTimer;
        private static QSP_CALL_SETINPUTSTRTEXT callSetInputText;
        private static QSP_CALL_SYSTEM callSystem;
        private static QSP_CALL_OPENGAMESTATUS callOpenGameStatus;
        private static QSP_CALL_SAVEGAMESTATUS callSaveGameStatus;
        private static QSP_CALL_SLEEP callSleep;
        private static QSP_CALL_GETMSCOUNT callGetMSCount;
        private static QSP_CALL_INPUTBOX callInputBox;

        private int oldRefreshCount;

        public QSPGameWorld()
        {

            logger = new LoggerConfiguration()
                .WriteTo.Trace()
                .CreateLogger();
            logger.Information("QSPGameWorld Constructor");
            qspWrapper = new QSPWrapper();
            QSPWrapper.EnableDebugMode(true);
            actionList = new BindingList<QSPAction>();
            objectList = new BindingList<QSPObject>();

            stopWatch = new Stopwatch();
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(ElapsedEvent);

            lastDebugCommands = new Queue<string>(10);

            #region delegates setup

            var intptr_delegate = IntPtr.Zero;

            callDebug = new QSP_CALL_DEBUG(Call_Debug);
            callIsPlayingFile = new QSP_CALL_ISPLAYINGFILE(Call_IsPlayingFile);
            callPlayFile = new QSP_CALL_PLAYFILE(Call_PlayFile);
            callCloseFile = new QSP_CALL_CLOSEFILE(Call_CloseFile);
            callShowImage = new QSP_CALL_SHOWIMAGE(Call_ShowImage);
            callShowWindow = new QSP_CALL_SHOWWINDOW(Call_ShowWindow);
            callDeleteMenu = new QSP_CALL_DELETEMENU(Call_DeleteMenu);
            callAddMenuItem = new QSP_CALL_ADDMENUITEM(Call_AddMenuItem);
            callShowMenu = new QSP_CALL_SHOWMENU(Call_ShowMenu);
            callShowMessage = new QSP_CALL_SHOWMSGSTR(Call_ShowMessage);
            callRefreshInt = new QSP_CALL_REFRESHINT(Call_RefreshInt);
            callSetTimer = new QSP_CALL_SETTIMER(Call_SetTimer);
            callSetInputText = new QSP_CALL_SETINPUTSTRTEXT(Call_SetInputText);
            callSystem = new QSP_CALL_SYSTEM(Call_System);
            callOpenGameStatus = new QSP_CALL_OPENGAMESTATUS(Call_OpenGameStatus);
            callSaveGameStatus = new QSP_CALL_SAVEGAMESTATUS(Call_SaveGameStatus);
            callSleep = new QSP_CALL_SLEEP(Call_Sleep);
            callGetMSCount = new QSP_CALL_GETMSCOUNT(Call_GetMSCount);
            callInputBox = new QSP_CALL_INPUTBOX(Call_InputBox);

            intptr_delegate = Marshal.GetFunctionPointerForDelegate(callDebug);
            QSPWrapper.SetCallBack(QSPWrapper.QSPCallback.QSP_CALL_DEBUG, intptr_delegate);

            intptr_delegate = Marshal.GetFunctionPointerForDelegate(callIsPlayingFile);
            QSPWrapper.SetCallBack(QSPWrapper.QSPCallback.QSP_CALL_ISPLAYINGFILE, intptr_delegate);

            intptr_delegate = Marshal.GetFunctionPointerForDelegate(callPlayFile);
            QSPWrapper.SetCallBack(QSPWrapper.QSPCallback.QSP_CALL_PLAYFILE, intptr_delegate);

            intptr_delegate = Marshal.GetFunctionPointerForDelegate(callCloseFile);
            QSPWrapper.SetCallBack(QSPWrapper.QSPCallback.QSP_CALL_CLOSEFILE, intptr_delegate);

            intptr_delegate = Marshal.GetFunctionPointerForDelegate(callShowImage);
            QSPWrapper.SetCallBack(QSPWrapper.QSPCallback.QSP_CALL_SHOWIMAGE, intptr_delegate);

            intptr_delegate = Marshal.GetFunctionPointerForDelegate(callShowWindow);
            QSPWrapper.SetCallBack(QSPWrapper.QSPCallback.QSP_CALL_SHOWWINDOW, intptr_delegate);

            intptr_delegate = Marshal.GetFunctionPointerForDelegate(callDeleteMenu);
            QSPWrapper.SetCallBack(QSPWrapper.QSPCallback.QSP_CALL_DELETEMENU, intptr_delegate);

            intptr_delegate = Marshal.GetFunctionPointerForDelegate(callAddMenuItem);
            QSPWrapper.SetCallBack(QSPWrapper.QSPCallback.QSP_CALL_ADDMENUITEM, intptr_delegate);

            intptr_delegate = Marshal.GetFunctionPointerForDelegate(callShowMenu);
            QSPWrapper.SetCallBack(QSPWrapper.QSPCallback.QSP_CALL_SHOWMENU, intptr_delegate);

            intptr_delegate = Marshal.GetFunctionPointerForDelegate(callShowMessage);
            QSPWrapper.SetCallBack(QSPWrapper.QSPCallback.QSP_CALL_SHOWMSGSTR, intptr_delegate);

            intptr_delegate = Marshal.GetFunctionPointerForDelegate(callRefreshInt);
            QSPWrapper.SetCallBack(QSPWrapper.QSPCallback.QSP_CALL_REFRESHINT, intptr_delegate);

            intptr_delegate = Marshal.GetFunctionPointerForDelegate(callSetTimer);
            QSPWrapper.SetCallBack(QSPWrapper.QSPCallback.QSP_CALL_SETTIMER, intptr_delegate);

            intptr_delegate = Marshal.GetFunctionPointerForDelegate(callSetInputText);
            QSPWrapper.SetCallBack(QSPWrapper.QSPCallback.QSP_CALL_SETINPUTSTRTEXT, intptr_delegate);

            intptr_delegate = Marshal.GetFunctionPointerForDelegate(callSystem);
            QSPWrapper.SetCallBack(QSPWrapper.QSPCallback.QSP_CALL_SYSTEM, intptr_delegate);

            intptr_delegate = Marshal.GetFunctionPointerForDelegate(callOpenGameStatus);
            QSPWrapper.SetCallBack(QSPWrapper.QSPCallback.QSP_CALL_OPENGAMESTATUS, intptr_delegate);

            intptr_delegate = Marshal.GetFunctionPointerForDelegate(callSaveGameStatus);
            QSPWrapper.SetCallBack(QSPWrapper.QSPCallback.QSP_CALL_SAVEGAMESTATUS, intptr_delegate);

            intptr_delegate = Marshal.GetFunctionPointerForDelegate(callSleep);
            QSPWrapper.SetCallBack(QSPWrapper.QSPCallback.QSP_CALL_SLEEP, intptr_delegate);

            intptr_delegate = Marshal.GetFunctionPointerForDelegate(callGetMSCount);
            QSPWrapper.SetCallBack(QSPWrapper.QSPCallback.QSP_CALL_GETMSCOUNT, intptr_delegate);

            intptr_delegate = Marshal.GetFunctionPointerForDelegate(callInputBox);
            QSPWrapper.SetCallBack(QSPWrapper.QSPCallback.QSP_CALL_INPUTBOX, intptr_delegate);
            #endregion delegate setup

        }

        #region delegates
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void QSP_CALL_DEBUG( IntPtr str );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool QSP_CALL_ISPLAYINGFILE( IntPtr file );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void QSP_CALL_PLAYFILE( IntPtr file, int volume );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void QSP_CALL_CLOSEFILE( IntPtr file );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void QSP_CALL_SHOWIMAGE( IntPtr file );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void QSP_CALL_SHOWWINDOW( [MarshalAsAttribute(UnmanagedType.I4)] QSPWrapper.QSPWindow windowType, bool isShown);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void QSP_CALL_DELETEMENU();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void QSP_CALL_ADDMENUITEM( IntPtr name, IntPtr imgPath );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int QSP_CALL_SHOWMENU();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void QSP_CALL_SHOWMSGSTR( IntPtr str );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void QSP_CALL_REFRESHINT( bool isRedrawn );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void QSP_CALL_SETTIMER( int msecs );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void QSP_CALL_SETINPUTSTRTEXT( IntPtr text );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void QSP_CALL_SYSTEM( IntPtr str );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void QSP_CALL_OPENGAMESTATUS( IntPtr file );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void QSP_CALL_SAVEGAMESTATUS( IntPtr file );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void QSP_CALL_SLEEP( int msecs );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int QSP_CALL_GETMSCOUNT();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int QSP_CALL_INPUTBOX(IntPtr text, IntPtr buffer, int maxLen);

        #endregion

        public override Queue<string> LastDebugCommands => lastDebugCommands;

        public override event PropertyChangedEventHandler PropertyChanged;

        public override DateTime CompiledDate => QSPWrapper.GetCompiledDate();

        public override string CurrentLocation => QSPWrapper.GetCurrentLocation();

        public override int FullRefreshCount => QSPWrapper.GetFullRefreshCount();

        public override string MainDescription => QSPWrapper.GetMainDesc();

        public override int MaxVariablesCount => QSPWrapper.QSPGetMaxVarsCount();

        public override string QSPFilePath => QSPWrapper.GetQstFullPath();

        public override IList<QSPVariable> VariablesList => _variableList.Values.ToList();

        public override string VarsDescription => QSPWrapper.GetVarsDesc();

        public override Version Version => QSPWrapper.GetVersion();

        public int LocationsCount => QSPWrapper.GetLocationsCount();

        public override BindingList<QSPAction> ActionList => actionList;

        public override BindingList<QSPObject> ObjectList => objectList;


        private void ElapsedEvent( object sender, ElapsedEventArgs e )
        {
            try
            {
                var result = QSPWrapper.ExecCounter(true);
                logger.Verbose($"Callback {nameof(ElapsedEvent)} => {result}");
            }
            catch(Exception exp)
            {
                logger.Error(exp.ToString());
                foreach(var str in lastDebugCommands)
                {
                    logger.Error(str);
                }
            }
        }

        private int Call_InputBox( IntPtr textPtr, IntPtr bufferPtr, int maxLen )
        {
            var text = Marshal.PtrToStringUni(textPtr);
            var buffer = Marshal.PtrToStringUni(bufferPtr);
            logger.Information($"Callback {nameof(Call_InputBox)}: {textPtr} & {buffer} & {maxLen}");
            return 0;
        }

        private int Call_GetMSCount()
        {
            var time = (int)stopWatch.ElapsedMilliseconds;
            logger.Information($"Callback {nameof(Call_GetMSCount)} => {time} ms");
            stopWatch.Reset();
            return time;
        }

        private void Call_Sleep( int msecs )
        {
            logger.Information($"Callback {nameof(Call_Sleep)}: {msecs}");
        }

        private void Call_SaveGameStatus( IntPtr filePtr )
        {
            var file = Marshal.PtrToStringUni(filePtr);
            logger.Information($"Callback {nameof(Call_SaveGameStatus)}: {file}");
        }

        private void Call_OpenGameStatus( IntPtr filePtr )
        {
            var file = Marshal.PtrToStringUni(filePtr);
            logger.Information($"Callback {nameof(Call_OpenGameStatus)}: {file}");
        }

        private void Call_System( IntPtr strPtr )
        {
            var str = Marshal.PtrToStringUni(strPtr);
            logger.Information($"Callback {nameof(Call_System)}: {str}");
        }

        private void Call_SetInputText( IntPtr textPtr )
        {
            var text = Marshal.PtrToStringUni(textPtr);
            logger.Information($"Callback {nameof(Call_SetInputText)}: {text}");
        }

        private void Call_SetTimer( int msecs )
        {
            logger.Information($"Callback {nameof(Call_SetTimer)}: {msecs}");
            if ( msecs > 0 )
            {
                timer.Interval = msecs;
                timer.Start();
            }
            else
            {
                timer.Stop();
            }
        }

        private void Call_RefreshInt( bool isRedrawn )
        {
            logger.Verbose($"Callback {nameof(Call_RefreshInt)}: {isRedrawn}");

            if ( QSPWrapper.QSPIsMainDescChanged() )
            {
                logger.Information($"Callback {nameof(Call_RefreshInt)}{isRedrawn}: Main Description changed");
                OnPropertyChanged(nameof(MainDescription));
            }

            if ( QSPWrapper.IsVarsDescChanged() )
            {
                logger.Information($"Callback {nameof(Call_RefreshInt)}{isRedrawn}: Vars Description changed");
                OnPropertyChanged(nameof(VarsDescription));
            }

            if( oldRefreshCount != FullRefreshCount )
            {
                logger.Information($"Callback {nameof(Call_RefreshInt)}{isRedrawn}: Refresh Count {oldRefreshCount} => {FullRefreshCount}");
                oldRefreshCount = FullRefreshCount;
                OnPropertyChanged(nameof(FullRefreshCount));
            }
        }

        private void Call_ShowMessage( IntPtr messagePtr )
        {
            var message = Marshal.PtrToStringUni(messagePtr);
            logger.Information($"Callback {nameof(Call_ShowMessage)}: {message}");
        }

        private int Call_ShowMenu()
        {
            logger.Information($"Callback {nameof(Call_ShowMenu)}");
            return 0;
        }

        private void Call_AddMenuItem( IntPtr namePtr, IntPtr imagePath )
        {
            var name = Marshal.PtrToStringUni(namePtr);
            var image = Marshal.PtrToStringUni(imagePath);
            logger.Information($"Callback {nameof(Call_AddMenuItem)}: {name} & {image}");
        }

        private void Call_DeleteMenu()
        {
            logger.Information($"Callback {nameof(Call_DeleteMenu)}");
        }

        private void Call_ShowWindow( QSPWrapper.QSPWindow windowType, bool isShown )
        {
            logger.Information($"Callback {nameof(Call_ShowWindow)}: {windowType} & {isShown}");
        }

        private void Call_ShowImage( IntPtr filePtr )
        {
            var file = Marshal.PtrToStringUni(filePtr);
            logger.Information($"Callback {nameof(Call_ShowImage)}: {file}");
        }

        private void Call_CloseFile( IntPtr filePtr )
        {
            var file = Marshal.PtrToStringUni(filePtr);
            logger.Information($"Callback {nameof(Call_CloseFile)}: {file}");
        }

        private void Call_PlayFile( IntPtr filePtr, int volume )
        {
            var file = Marshal.PtrToStringUni(filePtr);
            logger.Information($"Callback {nameof(Call_PlayFile)}: {file} & {volume}");
        }

        private bool Call_IsPlayingFile( IntPtr filePtr )
        {
            var file = Marshal.PtrToStringUni(filePtr);
            logger.Information($"Callback {nameof(Call_IsPlayingFile)}: {file}");
            return false;
        }

        private void Call_Debug( IntPtr strPtr )
        {
            var str = Marshal.PtrToStringUni(strPtr);
            if(lastDebugCommands.Count == 10)
            {
                lastDebugCommands.Dequeue();
            }

            lastDebugCommands.Enqueue(str);
            logger.Verbose($"Callback {nameof(Call_Debug)}: {str}");
        }

        public static Exception GetLastError()
        {
            QSPWrapper.QSPErrorCode error;
            int errorActIndex;
            int errorLine;
            var ptrError = IntPtr.Zero;
            QSPWrapper.QSPGetLastErrorData(out error, ref ptrError, out errorActIndex, out errorLine);
            Exception exception;
            if ( ptrError == IntPtr.Zero )
            {
                exception = new Exception(QSPWrapper.GetErrorDesc(error));
            }
            else
            {
                var errorStr = Marshal.PtrToStringUni(ptrError);
                exception = new Exception($"Error #{error} {errorStr} actIndex: {errorActIndex} line:{errorLine}");
            }



            return exception;
        }

        public string GetLocationName( int index )
        {
            string locationName = null;
            QSPWrapper.GetLocationName(index, out locationName);
            return locationName;
        }

        public void GetCurrentStateData( out string location, out int actIndex, out int line )
        {
            QSPWrapper.GetCurrentStateData(out location, out actIndex, out line);
        }

        public override bool ExecCommand( string command )
        {
            logger.Information($"Running command : {command}");
            return QSPWrapper.QSPExecString(command, true);
        }

        public bool LoadGameWorld( string QSPPath )
        {
            StopGame();
            logger.Information($"Opening game: {QSPPath}");
            isGameWorldLoaded = QSPWrapper.QSPOpenGameFile(QSPPath);
            isGameWorldActive = false;
            //RestartWorld(true);
            return isGameWorldLoaded;
        }

        public override bool RestartWorld( bool isRefreshed)
        {
            return QSPWrapper.RestartGame(isRefreshed);
        }

        public void ModifyVariables()
        {
            /*var dirtyVar = _variableList.Where(var => var.Value.IsDirty).Select(var => var.Value);
            logger.Information($"Modyfing variables Count: {dirtyVar.Count()}");
            foreach ( var variable in dirtyVar )
            {
                logger.Verbose($"{variable.FullVariableName} => {variable.ExecString}");
                if(ExecString(variable.ExecString, true))
                {
                    variable.IsDirty = false;
                }
            }*/
        }

        public bool OpenSavedGame( string savePath, bool isRefreshed )
        {
            StopGame();
            if ( isGameWorldLoaded )
            {
                logger.Information($"Loading save: {savePath}");
                if ( QSPWrapper.QSPLoadSavedGame(savePath, isRefreshed) )
                {
                    if(isGameWorldActive && savePath == currentSaveFile )
                    {
                        UpdateVariableList();
                    }
                    else
                    {
                        currentSaveFile = savePath;
                        isGameWorldActive = true;
                        PopulateVariableList();
                        RefreshActionList();
                        RefreshObjectList();
                    }

                    SendPropertyChange();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool WriteSaveGame( string savePath, bool isRefreshed )
        {
            logger.Information($"Saving game to {savePath}");
            return QSPWrapper.QSPWriteSaveGame(savePath, isRefreshed);
        }

        protected void OnPropertyChanged( string propertyName = null )
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static bool ExecString( string cmd, bool isRefreshed )
        {
            return QSPWrapper.QSPExecString(cmd, isRefreshed);
        }

        public void UpdateVariableList()
        {
            logger.Information("Updating variable List");
            /*for ( int i = 0; i < MaxVariablesCount; i++ )
            {
                var name = QSPWrapper.GetVariableNameByIndex(i);
                if ( !string.IsNullOrEmpty(name) )
                {
                    foreach(var variable in GetAllValues(name) )
                    {
                        if ( _variableList.ContainsKey(variable.FullVariableName) )
                        {
                            var newVariable = _variableList[variable.FullVariableName];
                            newVariable.NewValues(variable);
                        }
                        else
                        {
                            variable.IsNew = true;
                            _variableList.Add(variable.FullVariableName, variable);
                        }
                    }

                }
            }
            */
            OnPropertyChanged(nameof(VariablesList));
        }

        private void RefreshActionList()
        {
            logger.Information("Refresh Action List");
            ActionList.Clear();
            for(int i = 0; i < QSPWrapper.GetActionsCount(); i++ )
            {
                string imgPath = null;
                string desc = null;
                QSPWrapper.GetActionData(i, out imgPath, out desc);
                ActionList.Add(new QSPAction(i, imgPath, desc));
            }
        }

        private void RefreshObjectList()
        {
            logger.Information("Refresh Object List");
            ObjectList.Clear();
            for ( int i = 0; i < QSPWrapper.GetObjectsCount(); i++ )
            {
                string imgPath = null;
                string desc = null;
                QSPWrapper.GetObjectData(i, out imgPath, out desc);
                ObjectList.Add(new QSPObject(i, imgPath, desc));
            }
        }

        private void PopulateVariableList()
        {
            logger.Information("Populate Variable List");
            var variablesList = new Dictionary<string, QSPVariable>();

            for ( int i = 0; i < MaxVariablesCount; i++ )
            {
                var name = QSPWrapper.GetVariableNameByIndex(i);
                if ( !string.IsNullOrEmpty(name) )
                {
                    var newVariable = GetAllValues(name);
                    variablesList.Add(newVariable.FullVariableName, newVariable);
                }
            }

            _variableList = variablesList;
        }

        private static QSPVariable GetAllValues( string name )
        {
            QSPVariable newQSPVariable;
            var valueCount = QSPWrapper.GetVariableValuesCount(name);
            var indexCount = QSPWrapper.GetVariableIndexesCount(name);

            if ( valueCount == 0 )
            {

                int intValue;
                string strValue;
                QSPWrapper.GetVariableValues(name, 0, out intValue, out strValue);
                newQSPVariable = new QSPVariable(name, strValue, intValue);
            }
            else
            {
                newQSPVariable = new QSPVariable(name, valueCount, indexCount);

                for (int i = 0; i < valueCount; i++ )
                {
                    int intValue;
                    string strValue;
                    QSPWrapper.GetVariableValues(name, i, out intValue, out strValue);
                    newQSPVariable.AddValues(i, strValue, intValue);
                }

                for (int i = 0; i < indexCount; i++)
                {
                    int valueIndex;
                    string indexName;
                    QSPWrapper.GetIndexNameForVariable(name, i, out valueIndex, out indexName);
                    newQSPVariable.SetIndexName(valueIndex, indexName);
                }
            }
            return newQSPVariable;
        }

        private void StopGame()
        {
            stopWatch.Stop();
            timer.Stop();
        }

        private void SendPropertyChange()
        {
            OnPropertyChanged(nameof(FullRefreshCount));
            OnPropertyChanged(nameof(CurrentLocation));
            //OnPropertyChanged(nameof(MainDescription));
            //OnPropertyChanged(nameof(VarsDescription));
            OnPropertyChanged(nameof(QSPFilePath));
        }

    }
}
