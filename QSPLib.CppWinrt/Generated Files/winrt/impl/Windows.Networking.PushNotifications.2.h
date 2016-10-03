// WARNING: Please don't edit this file. It was generated by C++/WinRT v2.0.191018.6

#ifndef WINRT_Windows_Networking_PushNotifications_2_H
#define WINRT_Windows_Networking_PushNotifications_2_H
#include "winrt/impl/Windows.System.1.h"
#include "winrt/impl/Windows.Networking.PushNotifications.1.h"
WINRT_EXPORT namespace winrt::Windows::Networking::PushNotifications
{
    struct __declspec(empty_bases) PushNotificationChannel : Windows::Networking::PushNotifications::IPushNotificationChannel
    {
        PushNotificationChannel(std::nullptr_t) noexcept {}
        PushNotificationChannel(void* ptr, take_ownership_from_abi_t) noexcept : Windows::Networking::PushNotifications::IPushNotificationChannel(ptr, take_ownership_from_abi) {}
    };
    struct PushNotificationChannelManager
    {
        PushNotificationChannelManager() = delete;
        static auto CreatePushNotificationChannelForApplicationAsync();
        static auto CreatePushNotificationChannelForApplicationAsync(param::hstring const& applicationId);
        static auto CreatePushNotificationChannelForSecondaryTileAsync(param::hstring const& tileId);
        static auto GetForUser(Windows::System::User const& user);
        static auto GetDefault();
    };
    struct __declspec(empty_bases) PushNotificationChannelManagerForUser : Windows::Networking::PushNotifications::IPushNotificationChannelManagerForUser,
        impl::require<PushNotificationChannelManagerForUser, Windows::Networking::PushNotifications::IPushNotificationChannelManagerForUser2>
    {
        PushNotificationChannelManagerForUser(std::nullptr_t) noexcept {}
        PushNotificationChannelManagerForUser(void* ptr, take_ownership_from_abi_t) noexcept : Windows::Networking::PushNotifications::IPushNotificationChannelManagerForUser(ptr, take_ownership_from_abi) {}
    };
    struct __declspec(empty_bases) PushNotificationReceivedEventArgs : Windows::Networking::PushNotifications::IPushNotificationReceivedEventArgs
    {
        PushNotificationReceivedEventArgs(std::nullptr_t) noexcept {}
        PushNotificationReceivedEventArgs(void* ptr, take_ownership_from_abi_t) noexcept : Windows::Networking::PushNotifications::IPushNotificationReceivedEventArgs(ptr, take_ownership_from_abi) {}
    };
    struct __declspec(empty_bases) RawNotification : Windows::Networking::PushNotifications::IRawNotification,
        impl::require<RawNotification, Windows::Networking::PushNotifications::IRawNotification2>
    {
        RawNotification(std::nullptr_t) noexcept {}
        RawNotification(void* ptr, take_ownership_from_abi_t) noexcept : Windows::Networking::PushNotifications::IRawNotification(ptr, take_ownership_from_abi) {}
    };
}
#endif
