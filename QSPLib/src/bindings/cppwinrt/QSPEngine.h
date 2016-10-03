#pragma once
#include "QSPEngine.g.h"

namespace winrt::QSPLib::implementation
{
    struct QSPEngine : QSPEngineT<QSPEngine>
    {
        QSPEngine() = default;

        hstring Version();
        void Version(hstring const& value);
    };
}
namespace winrt::QSPLib::factory_implementation
{
    struct QSPEngine : QSPEngineT<QSPEngine, implementation::QSPEngine>
    {
    };
}
