#include "pch.h"
#include "Location.h"
#include "Location.g.cpp"

#include "LineOfCode.h"

namespace winrt::QSPLib_CppWinrt::implementation
{
    Location::Location() {

        m_LocationIndex = -1;
    }

    Location::Location(int location) {
        m_LocationIndex = location;
    }

    hstring Location::Name() const
    {
        if (m_LocationIndex == -1) return L"Unitialized";
        return qspLocs[m_LocationIndex].Name;
    }

    hstring Location::Desc() const
    {
        if (m_LocationIndex == -1) return L"Unitialized";
        return qspLocs[m_LocationIndex].Desc;
    }
    
    Windows::Foundation::Collections::IVector<QSPLib_CppWinrt::LineOfCode> Location::OnVisitLines()
    {
        if (m_LocationIndex == -1) {
            return winrt::single_threaded_vector<winrt::QSPLib_CppWinrt::LineOfCode>();
        }

       std::vector<QSPLib_CppWinrt::implementation::LineOfCode> lineOfCodes(qspLocs[m_LocationIndex].OnVisitLinesCount);

        //m_locations.Append();
        auto lineOfCodesWinRT = winrt::single_threaded_observable_vector<winrt::QSPLib_CppWinrt::LineOfCode>();

        for (int i = 0; i < qspLocs[m_LocationIndex].OnVisitLinesCount; i++) {
            auto lineOfCode = winrt::make<winrt::QSPLib_CppWinrt::implementation::LineOfCode>(m_LocationIndex, i);

            //auto test = &qspLocs[m_LocationIndex].OnVisitLinesCount[i];
            //QSPLineOfCode* test = static_cast<QSPLineOfCode*>(&qspLocs[m_LocationIndex].OnVisitLinesCount[i]);
            //auto lineOfCode = winrt::make<winrt::QSPLib_CppWinrt::implementation::LineOfCode>((QSPLineOfCode*)&qspLocs[m_LocationIndex].OnVisitLinesCount[i]);





            //auto lineOfCode = std::make_unique<winrt::QSPLib_CppWinrt::implementation::LineOfCode>(m_LocationIndex, i);
            //lineOfCodes.push_back(winrt::make<winrt::QSPLib_CppWinrt::implementation::LineOfCode>(qspLocs[m_LocationIndex].OnVisitLines[i]));

            //lineOfCodes.push_back(winrt::make<winrt::QSPLib_CppWinrt::implementation::LineOfCode>(m_LocationIndex, i));

            //lineOfCodes.emplace_back(lineOfCode);

            lineOfCodesWinRT.Append(lineOfCode);
        }

        //return winrt::single_threaded_vector<winrt::QSPLib_CppWinrt::LineOfCode>(std::move(lineOfCodes));
        //return winrt::single_threaded_vector<winrt::QSPLib_CppWinrt::LineOfCode>();
        return lineOfCodesWinRT;
    }
}
