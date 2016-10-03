#pragma once
#include <pch.h>
#include "LineOfCode.g.h"


namespace winrt::QSPLib_CppWinrt::implementation
{
    struct LineOfCode : LineOfCodeT<LineOfCode>
    {
        LineOfCode();
        LineOfCode(int location, int lineofcode);

        hstring Text() const;
        int32_t LineNum() const;
        bool IsMultiline() const;
        hstring Label() const;
        Windows::Foundation::Collections::IVector<QSPLib_CppWinrt::CachedStat> CachedStats();

    private:
        //QSPLineOfCode* m_lineofCode;
        int m_LocationIndex;
        int m_lineOfCode;
    };
}
