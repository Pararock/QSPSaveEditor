// WARNING: Please don't edit this file. It was generated by C++/WinRT v2.0.191018.6

#ifndef WINRT_Windows_Phone_System_2_H
#define WINRT_Windows_Phone_System_2_H
#include "winrt/impl/Windows.Phone.System.1.h"
WINRT_EXPORT namespace winrt::Windows::Phone::System
{
    struct SystemProtection
    {
        SystemProtection() = delete;
        [[nodiscard]] static auto ScreenLocked();
        static auto RequestScreenUnlock();
    };
}
#endif
