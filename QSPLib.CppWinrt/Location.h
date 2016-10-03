#pragma once
#include "Location.g.h"

namespace winrt::QSPLib_CppWinrt::implementation
{
    struct Location : LocationT<Location>
    {
        Location();
        Location(int location);

        hstring Name() const;
        hstring Desc() const;
        Windows::Foundation::Collections::IVector<QSPLib_CppWinrt::LineOfCode> OnVisitLines();

    private:
        int m_LocationIndex;
    };
}
