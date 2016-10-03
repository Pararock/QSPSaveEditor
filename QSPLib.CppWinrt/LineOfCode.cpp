#include "pch.h"
#include "LineOfCode.h"
#include "LineOfCode.g.cpp"

namespace winrt::QSPLib_CppWinrt::implementation
{
    LineOfCode::LineOfCode(int location, int lineofcode)
    {
        m_lineOfCode = lineofcode;
        m_LocationIndex = location;
        //lineOfCodes[i].Text = lineOfCode.Str;
        //lineOfCodes[i].LineNum = qspLocs[m_LocationIndex].OnVisitLines[i].LineNum;
        //lineOfCodes[i].IsMultiline = qspLocs[m_LocationIndex].OnVisitLines[i].IsMultiline;
        //if (qspLocs[m_LocationIndex].OnVisitLines[i].Label != nullptr)
        //    lineOfCodes[i].Label = qspLocs[m_LocationIndex].OnVisitLines[i].Label;

        //lineOfCodes[i].StatsCount = qspLocs[m_LocationIndex].OnVisitLines[i].StatsCount;
    }

    LineOfCode::LineOfCode() {
        m_lineOfCode = -1;
        m_LocationIndex = -1;
    }

    hstring LineOfCode::Text() const
    {
        return qspLocs[m_LocationIndex].OnVisitLines[m_lineOfCode].Str;
    }

    int32_t LineOfCode::LineNum() const
    {
        return qspLocs[m_LocationIndex].OnVisitLines[m_lineOfCode].LineNum;
    }

    bool LineOfCode::IsMultiline() const
    {
        return qspLocs[m_LocationIndex].OnVisitLines[m_lineOfCode].IsMultiline;
    }

    hstring LineOfCode::Label() const
    {
        if (qspLocs[m_LocationIndex].OnVisitLines[m_lineOfCode].Label != nullptr)
            return qspLocs[m_LocationIndex].OnVisitLines[m_lineOfCode].Label;
        else
            return L"";
    }


    /*typedef struct
{
    QSP_CHAR *Str;
    int LineNum;
    QSP_BOOL IsMultiline;
    QSP_CHAR *Label;
    QSPCachedStat *Stats;
    int StatsCount;
} QSPLineOfCode;

    struct CachedStat
    {
        Int32 Stat;
        Int32 EndPos;
        Int32 ParamPos;
    };

    typedef struct
{
    int Stat;
    int EndPos;
    int ParamPos;
} QSPCachedStat;

*/
    Windows::Foundation::Collections::IVector<QSPLib_CppWinrt::CachedStat> LineOfCode::CachedStats()
    {
        if (m_LocationIndex == -1) {
            return winrt::single_threaded_vector<winrt::QSPLib_CppWinrt::CachedStat>();;
        }

        std::vector<QSPLib_CppWinrt::CachedStat> cachedStat(qspLocs[m_LocationIndex].OnVisitLines[m_lineOfCode].StatsCount);

        for (int i = 0; i < qspLocs[m_LocationIndex].OnVisitLines[m_lineOfCode].StatsCount; i++) {
            cachedStat.push_back(QSPLib_CppWinrt::CachedStat());
            cachedStat[i].Stat = static_cast<QSPLib_CppWinrt::Statement>(qspLocs[m_LocationIndex].OnVisitLines[m_lineOfCode].Stats[i].Stat);
            cachedStat[i].EndPos = qspLocs[m_LocationIndex].OnVisitLines[m_lineOfCode].Stats[i].EndPos;
            cachedStat[i].ParamPos = qspLocs[m_LocationIndex].OnVisitLines[m_lineOfCode].Stats[i].ParamPos;
        }

        return winrt::single_threaded_vector<winrt::QSPLib_CppWinrt::CachedStat>(std::move(cachedStat));
    }
}
