// WARNING: Please don't edit this file. It was generated by C++/WinRT v2.0.191018.6

#ifndef WINRT_Windows_Devices_Background_1_H
#define WINRT_Windows_Devices_Background_1_H
#include "winrt/impl/Windows.Devices.Background.0.h"
WINRT_EXPORT namespace winrt::Windows::Devices::Background
{
    struct __declspec(empty_bases) IDeviceServicingDetails :
        Windows::Foundation::IInspectable,
        impl::consume_t<IDeviceServicingDetails>
    {
        IDeviceServicingDetails(std::nullptr_t = nullptr) noexcept {}
        IDeviceServicingDetails(void* ptr, take_ownership_from_abi_t) noexcept : Windows::Foundation::IInspectable(ptr, take_ownership_from_abi) {}
    };
    struct __declspec(empty_bases) IDeviceUseDetails :
        Windows::Foundation::IInspectable,
        impl::consume_t<IDeviceUseDetails>
    {
        IDeviceUseDetails(std::nullptr_t = nullptr) noexcept {}
        IDeviceUseDetails(void* ptr, take_ownership_from_abi_t) noexcept : Windows::Foundation::IInspectable(ptr, take_ownership_from_abi) {}
    };
}
#endif
