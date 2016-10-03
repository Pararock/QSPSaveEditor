// WARNING: Please don't edit this file. It was generated by C++/WinRT v2.0.191018.6

#ifndef WINRT_Windows_Networking_Proximity_0_H
#define WINRT_Windows_Networking_Proximity_0_H
WINRT_EXPORT namespace winrt::Windows::Foundation
{
    struct EventRegistrationToken;
    template <typename TSender, typename TResult> struct TypedEventHandler;
    struct Uri;
}
WINRT_EXPORT namespace winrt::Windows::Networking
{
    struct HostName;
}
WINRT_EXPORT namespace winrt::Windows::Networking::Sockets
{
    struct StreamSocket;
}
WINRT_EXPORT namespace winrt::Windows::Storage::Streams
{
    struct IBuffer;
}
WINRT_EXPORT namespace winrt::Windows::Networking::Proximity
{
    enum class PeerDiscoveryTypes : uint32_t
    {
        None = 0,
        Browse = 0x1,
        Triggered = 0x2,
    };
    enum class PeerRole : int32_t
    {
        Peer = 0,
        Host = 1,
        Client = 2,
    };
    enum class PeerWatcherStatus : int32_t
    {
        Created = 0,
        Started = 1,
        EnumerationCompleted = 2,
        Stopping = 3,
        Stopped = 4,
        Aborted = 5,
    };
    enum class TriggeredConnectState : int32_t
    {
        PeerFound = 0,
        Listening = 1,
        Connecting = 2,
        Completed = 3,
        Canceled = 4,
        Failed = 5,
    };
    struct IConnectionRequestedEventArgs;
    struct IPeerFinderStatics;
    struct IPeerFinderStatics2;
    struct IPeerInformation;
    struct IPeerInformation3;
    struct IPeerInformationWithHostAndService;
    struct IPeerWatcher;
    struct IProximityDevice;
    struct IProximityDeviceStatics;
    struct IProximityMessage;
    struct ITriggeredConnectionStateChangedEventArgs;
    struct ConnectionRequestedEventArgs;
    struct PeerFinder;
    struct PeerInformation;
    struct PeerWatcher;
    struct ProximityDevice;
    struct ProximityMessage;
    struct TriggeredConnectionStateChangedEventArgs;
    struct DeviceArrivedEventHandler;
    struct DeviceDepartedEventHandler;
    struct MessageReceivedHandler;
    struct MessageTransmittedHandler;
}
namespace winrt::impl
{
    template <> struct category<Windows::Networking::Proximity::IConnectionRequestedEventArgs>{ using type = interface_category; };
    template <> struct category<Windows::Networking::Proximity::IPeerFinderStatics>{ using type = interface_category; };
    template <> struct category<Windows::Networking::Proximity::IPeerFinderStatics2>{ using type = interface_category; };
    template <> struct category<Windows::Networking::Proximity::IPeerInformation>{ using type = interface_category; };
    template <> struct category<Windows::Networking::Proximity::IPeerInformation3>{ using type = interface_category; };
    template <> struct category<Windows::Networking::Proximity::IPeerInformationWithHostAndService>{ using type = interface_category; };
    template <> struct category<Windows::Networking::Proximity::IPeerWatcher>{ using type = interface_category; };
    template <> struct category<Windows::Networking::Proximity::IProximityDevice>{ using type = interface_category; };
    template <> struct category<Windows::Networking::Proximity::IProximityDeviceStatics>{ using type = interface_category; };
    template <> struct category<Windows::Networking::Proximity::IProximityMessage>{ using type = interface_category; };
    template <> struct category<Windows::Networking::Proximity::ITriggeredConnectionStateChangedEventArgs>{ using type = interface_category; };
    template <> struct category<Windows::Networking::Proximity::ConnectionRequestedEventArgs>{ using type = class_category; };
    template <> struct category<Windows::Networking::Proximity::PeerFinder>{ using type = class_category; };
    template <> struct category<Windows::Networking::Proximity::PeerInformation>{ using type = class_category; };
    template <> struct category<Windows::Networking::Proximity::PeerWatcher>{ using type = class_category; };
    template <> struct category<Windows::Networking::Proximity::ProximityDevice>{ using type = class_category; };
    template <> struct category<Windows::Networking::Proximity::ProximityMessage>{ using type = class_category; };
    template <> struct category<Windows::Networking::Proximity::TriggeredConnectionStateChangedEventArgs>{ using type = class_category; };
    template <> struct category<Windows::Networking::Proximity::PeerDiscoveryTypes>{ using type = enum_category; };
    template <> struct category<Windows::Networking::Proximity::PeerRole>{ using type = enum_category; };
    template <> struct category<Windows::Networking::Proximity::PeerWatcherStatus>{ using type = enum_category; };
    template <> struct category<Windows::Networking::Proximity::TriggeredConnectState>{ using type = enum_category; };
    template <> struct category<Windows::Networking::Proximity::DeviceArrivedEventHandler>{ using type = delegate_category; };
    template <> struct category<Windows::Networking::Proximity::DeviceDepartedEventHandler>{ using type = delegate_category; };
    template <> struct category<Windows::Networking::Proximity::MessageReceivedHandler>{ using type = delegate_category; };
    template <> struct category<Windows::Networking::Proximity::MessageTransmittedHandler>{ using type = delegate_category; };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::ConnectionRequestedEventArgs>{ L"Windows.Networking.Proximity.ConnectionRequestedEventArgs" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::PeerFinder>{ L"Windows.Networking.Proximity.PeerFinder" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::PeerInformation>{ L"Windows.Networking.Proximity.PeerInformation" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::PeerWatcher>{ L"Windows.Networking.Proximity.PeerWatcher" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::ProximityDevice>{ L"Windows.Networking.Proximity.ProximityDevice" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::ProximityMessage>{ L"Windows.Networking.Proximity.ProximityMessage" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::TriggeredConnectionStateChangedEventArgs>{ L"Windows.Networking.Proximity.TriggeredConnectionStateChangedEventArgs" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::PeerDiscoveryTypes>{ L"Windows.Networking.Proximity.PeerDiscoveryTypes" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::PeerRole>{ L"Windows.Networking.Proximity.PeerRole" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::PeerWatcherStatus>{ L"Windows.Networking.Proximity.PeerWatcherStatus" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::TriggeredConnectState>{ L"Windows.Networking.Proximity.TriggeredConnectState" };
#ifndef WINRT_LEAN_AND_MEAN
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::IConnectionRequestedEventArgs>{ L"Windows.Networking.Proximity.IConnectionRequestedEventArgs" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::IPeerFinderStatics>{ L"Windows.Networking.Proximity.IPeerFinderStatics" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::IPeerFinderStatics2>{ L"Windows.Networking.Proximity.IPeerFinderStatics2" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::IPeerInformation>{ L"Windows.Networking.Proximity.IPeerInformation" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::IPeerInformation3>{ L"Windows.Networking.Proximity.IPeerInformation3" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::IPeerInformationWithHostAndService>{ L"Windows.Networking.Proximity.IPeerInformationWithHostAndService" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::IPeerWatcher>{ L"Windows.Networking.Proximity.IPeerWatcher" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::IProximityDevice>{ L"Windows.Networking.Proximity.IProximityDevice" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::IProximityDeviceStatics>{ L"Windows.Networking.Proximity.IProximityDeviceStatics" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::IProximityMessage>{ L"Windows.Networking.Proximity.IProximityMessage" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::ITriggeredConnectionStateChangedEventArgs>{ L"Windows.Networking.Proximity.ITriggeredConnectionStateChangedEventArgs" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::DeviceArrivedEventHandler>{ L"Windows.Networking.Proximity.DeviceArrivedEventHandler" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::DeviceDepartedEventHandler>{ L"Windows.Networking.Proximity.DeviceDepartedEventHandler" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::MessageReceivedHandler>{ L"Windows.Networking.Proximity.MessageReceivedHandler" };
    template <> inline constexpr auto& name_v<Windows::Networking::Proximity::MessageTransmittedHandler>{ L"Windows.Networking.Proximity.MessageTransmittedHandler" };
#endif
    template <> inline constexpr guid guid_v<Windows::Networking::Proximity::IConnectionRequestedEventArgs>{ 0xEB6891AE,0x4F1E,0x4C66,{ 0xBD,0x0D,0x46,0x92,0x4A,0x94,0x2E,0x08 } };
    template <> inline constexpr guid guid_v<Windows::Networking::Proximity::IPeerFinderStatics>{ 0x914B3B61,0xF6E1,0x47C4,{ 0xA1,0x4C,0x14,0x8A,0x19,0x03,0xD0,0xC6 } };
    template <> inline constexpr guid guid_v<Windows::Networking::Proximity::IPeerFinderStatics2>{ 0xD6E73C65,0xFDD0,0x4B0B,{ 0x93,0x12,0x86,0x64,0x08,0x93,0x5D,0x82 } };
    template <> inline constexpr guid guid_v<Windows::Networking::Proximity::IPeerInformation>{ 0x20024F08,0x9FFF,0x45F4,{ 0xB6,0xE9,0x40,0x8B,0x2E,0xBE,0xF3,0x73 } };
    template <> inline constexpr guid guid_v<Windows::Networking::Proximity::IPeerInformation3>{ 0xB20F612A,0xDBD0,0x40F8,{ 0x95,0xBD,0x2D,0x42,0x09,0xC7,0x83,0x6F } };
    template <> inline constexpr guid guid_v<Windows::Networking::Proximity::IPeerInformationWithHostAndService>{ 0xECC7CCAD,0x1B70,0x4E8B,{ 0x92,0xDB,0xBB,0xE7,0x81,0x41,0x93,0x08 } };
    template <> inline constexpr guid guid_v<Windows::Networking::Proximity::IPeerWatcher>{ 0x3CEE21F8,0x2FA6,0x4679,{ 0x96,0x91,0x03,0xC9,0x4A,0x42,0x0F,0x34 } };
    template <> inline constexpr guid guid_v<Windows::Networking::Proximity::IProximityDevice>{ 0xEFA8A552,0xF6E1,0x4329,{ 0xA0,0xFC,0xAB,0x6B,0x0F,0xD2,0x82,0x62 } };
    template <> inline constexpr guid guid_v<Windows::Networking::Proximity::IProximityDeviceStatics>{ 0x914BA01D,0xF6E1,0x47C4,{ 0xA1,0x4C,0x14,0x8A,0x19,0x03,0xD0,0xC6 } };
    template <> inline constexpr guid guid_v<Windows::Networking::Proximity::IProximityMessage>{ 0xEFAB0782,0xF6E1,0x4675,{ 0xA0,0x45,0xD8,0xE3,0x20,0xC2,0x48,0x08 } };
    template <> inline constexpr guid guid_v<Windows::Networking::Proximity::ITriggeredConnectionStateChangedEventArgs>{ 0xC6A780AD,0xF6E1,0x4D54,{ 0x96,0xE2,0x33,0xF6,0x20,0xBC,0xA8,0x8A } };
    template <> inline constexpr guid guid_v<Windows::Networking::Proximity::DeviceArrivedEventHandler>{ 0xEFA9DA69,0xF6E1,0x49C9,{ 0xA4,0x9E,0x8E,0x0F,0xC5,0x8F,0xB9,0x11 } };
    template <> inline constexpr guid guid_v<Windows::Networking::Proximity::DeviceDepartedEventHandler>{ 0xEFA9DA69,0xF6E2,0x49C9,{ 0xA4,0x9E,0x8E,0x0F,0xC5,0x8F,0xB9,0x11 } };
    template <> inline constexpr guid guid_v<Windows::Networking::Proximity::MessageReceivedHandler>{ 0xEFAB0782,0xF6E2,0x4675,{ 0xA0,0x45,0xD8,0xE3,0x20,0xC2,0x48,0x08 } };
    template <> inline constexpr guid guid_v<Windows::Networking::Proximity::MessageTransmittedHandler>{ 0xEFAA0B4A,0xF6E2,0x4D7D,{ 0x85,0x6C,0x78,0xFC,0x8E,0xFC,0x02,0x1E } };
    template <> struct default_interface<Windows::Networking::Proximity::ConnectionRequestedEventArgs>{ using type = Windows::Networking::Proximity::IConnectionRequestedEventArgs; };
    template <> struct default_interface<Windows::Networking::Proximity::PeerInformation>{ using type = Windows::Networking::Proximity::IPeerInformation; };
    template <> struct default_interface<Windows::Networking::Proximity::PeerWatcher>{ using type = Windows::Networking::Proximity::IPeerWatcher; };
    template <> struct default_interface<Windows::Networking::Proximity::ProximityDevice>{ using type = Windows::Networking::Proximity::IProximityDevice; };
    template <> struct default_interface<Windows::Networking::Proximity::ProximityMessage>{ using type = Windows::Networking::Proximity::IProximityMessage; };
    template <> struct default_interface<Windows::Networking::Proximity::TriggeredConnectionStateChangedEventArgs>{ using type = Windows::Networking::Proximity::ITriggeredConnectionStateChangedEventArgs; };
    template <> struct abi<Windows::Networking::Proximity::IConnectionRequestedEventArgs>
    {
        struct __declspec(novtable) type : inspectable_abi
        {
            virtual int32_t __stdcall get_PeerInformation(void**) noexcept = 0;
        };
    };
    template <> struct abi<Windows::Networking::Proximity::IPeerFinderStatics>
    {
        struct __declspec(novtable) type : inspectable_abi
        {
            virtual int32_t __stdcall get_AllowBluetooth(bool*) noexcept = 0;
            virtual int32_t __stdcall put_AllowBluetooth(bool) noexcept = 0;
            virtual int32_t __stdcall get_AllowInfrastructure(bool*) noexcept = 0;
            virtual int32_t __stdcall put_AllowInfrastructure(bool) noexcept = 0;
            virtual int32_t __stdcall get_AllowWiFiDirect(bool*) noexcept = 0;
            virtual int32_t __stdcall put_AllowWiFiDirect(bool) noexcept = 0;
            virtual int32_t __stdcall get_DisplayName(void**) noexcept = 0;
            virtual int32_t __stdcall put_DisplayName(void*) noexcept = 0;
            virtual int32_t __stdcall get_SupportedDiscoveryTypes(uint32_t*) noexcept = 0;
            virtual int32_t __stdcall get_AlternateIdentities(void**) noexcept = 0;
            virtual int32_t __stdcall Start() noexcept = 0;
            virtual int32_t __stdcall StartWithMessage(void*) noexcept = 0;
            virtual int32_t __stdcall Stop() noexcept = 0;
            virtual int32_t __stdcall add_TriggeredConnectionStateChanged(void*, winrt::event_token*) noexcept = 0;
            virtual int32_t __stdcall remove_TriggeredConnectionStateChanged(winrt::event_token) noexcept = 0;
            virtual int32_t __stdcall add_ConnectionRequested(void*, winrt::event_token*) noexcept = 0;
            virtual int32_t __stdcall remove_ConnectionRequested(winrt::event_token) noexcept = 0;
            virtual int32_t __stdcall FindAllPeersAsync(void**) noexcept = 0;
            virtual int32_t __stdcall ConnectAsync(void*, void**) noexcept = 0;
        };
    };
    template <> struct abi<Windows::Networking::Proximity::IPeerFinderStatics2>
    {
        struct __declspec(novtable) type : inspectable_abi
        {
            virtual int32_t __stdcall get_Role(int32_t*) noexcept = 0;
            virtual int32_t __stdcall put_Role(int32_t) noexcept = 0;
            virtual int32_t __stdcall get_DiscoveryData(void**) noexcept = 0;
            virtual int32_t __stdcall put_DiscoveryData(void*) noexcept = 0;
            virtual int32_t __stdcall CreateWatcher(void**) noexcept = 0;
        };
    };
    template <> struct abi<Windows::Networking::Proximity::IPeerInformation>
    {
        struct __declspec(novtable) type : inspectable_abi
        {
            virtual int32_t __stdcall get_DisplayName(void**) noexcept = 0;
        };
    };
    template <> struct abi<Windows::Networking::Proximity::IPeerInformation3>
    {
        struct __declspec(novtable) type : inspectable_abi
        {
            virtual int32_t __stdcall get_Id(void**) noexcept = 0;
            virtual int32_t __stdcall get_DiscoveryData(void**) noexcept = 0;
        };
    };
    template <> struct abi<Windows::Networking::Proximity::IPeerInformationWithHostAndService>
    {
        struct __declspec(novtable) type : inspectable_abi
        {
            virtual int32_t __stdcall get_HostName(void**) noexcept = 0;
            virtual int32_t __stdcall get_ServiceName(void**) noexcept = 0;
        };
    };
    template <> struct abi<Windows::Networking::Proximity::IPeerWatcher>
    {
        struct __declspec(novtable) type : inspectable_abi
        {
            virtual int32_t __stdcall add_Added(void*, winrt::event_token*) noexcept = 0;
            virtual int32_t __stdcall remove_Added(winrt::event_token) noexcept = 0;
            virtual int32_t __stdcall add_Removed(void*, winrt::event_token*) noexcept = 0;
            virtual int32_t __stdcall remove_Removed(winrt::event_token) noexcept = 0;
            virtual int32_t __stdcall add_Updated(void*, winrt::event_token*) noexcept = 0;
            virtual int32_t __stdcall remove_Updated(winrt::event_token) noexcept = 0;
            virtual int32_t __stdcall add_EnumerationCompleted(void*, winrt::event_token*) noexcept = 0;
            virtual int32_t __stdcall remove_EnumerationCompleted(winrt::event_token) noexcept = 0;
            virtual int32_t __stdcall add_Stopped(void*, winrt::event_token*) noexcept = 0;
            virtual int32_t __stdcall remove_Stopped(winrt::event_token) noexcept = 0;
            virtual int32_t __stdcall get_Status(int32_t*) noexcept = 0;
            virtual int32_t __stdcall Start() noexcept = 0;
            virtual int32_t __stdcall Stop() noexcept = 0;
        };
    };
    template <> struct abi<Windows::Networking::Proximity::IProximityDevice>
    {
        struct __declspec(novtable) type : inspectable_abi
        {
            virtual int32_t __stdcall SubscribeForMessage(void*, void*, int64_t*) noexcept = 0;
            virtual int32_t __stdcall PublishMessage(void*, void*, int64_t*) noexcept = 0;
            virtual int32_t __stdcall PublishMessageWithCallback(void*, void*, void*, int64_t*) noexcept = 0;
            virtual int32_t __stdcall PublishBinaryMessage(void*, void*, int64_t*) noexcept = 0;
            virtual int32_t __stdcall PublishBinaryMessageWithCallback(void*, void*, void*, int64_t*) noexcept = 0;
            virtual int32_t __stdcall PublishUriMessage(void*, int64_t*) noexcept = 0;
            virtual int32_t __stdcall PublishUriMessageWithCallback(void*, void*, int64_t*) noexcept = 0;
            virtual int32_t __stdcall StopSubscribingForMessage(int64_t) noexcept = 0;
            virtual int32_t __stdcall StopPublishingMessage(int64_t) noexcept = 0;
            virtual int32_t __stdcall add_DeviceArrived(void*, winrt::event_token*) noexcept = 0;
            virtual int32_t __stdcall remove_DeviceArrived(winrt::event_token) noexcept = 0;
            virtual int32_t __stdcall add_DeviceDeparted(void*, winrt::event_token*) noexcept = 0;
            virtual int32_t __stdcall remove_DeviceDeparted(winrt::event_token) noexcept = 0;
            virtual int32_t __stdcall get_MaxMessageBytes(uint32_t*) noexcept = 0;
            virtual int32_t __stdcall get_BitsPerSecond(uint64_t*) noexcept = 0;
            virtual int32_t __stdcall get_DeviceId(void**) noexcept = 0;
        };
    };
    template <> struct abi<Windows::Networking::Proximity::IProximityDeviceStatics>
    {
        struct __declspec(novtable) type : inspectable_abi
        {
            virtual int32_t __stdcall GetDeviceSelector(void**) noexcept = 0;
            virtual int32_t __stdcall GetDefault(void**) noexcept = 0;
            virtual int32_t __stdcall FromId(void*, void**) noexcept = 0;
        };
    };
    template <> struct abi<Windows::Networking::Proximity::IProximityMessage>
    {
        struct __declspec(novtable) type : inspectable_abi
        {
            virtual int32_t __stdcall get_MessageType(void**) noexcept = 0;
            virtual int32_t __stdcall get_SubscriptionId(int64_t*) noexcept = 0;
            virtual int32_t __stdcall get_Data(void**) noexcept = 0;
            virtual int32_t __stdcall get_DataAsString(void**) noexcept = 0;
        };
    };
    template <> struct abi<Windows::Networking::Proximity::ITriggeredConnectionStateChangedEventArgs>
    {
        struct __declspec(novtable) type : inspectable_abi
        {
            virtual int32_t __stdcall get_State(int32_t*) noexcept = 0;
            virtual int32_t __stdcall get_Id(uint32_t*) noexcept = 0;
            virtual int32_t __stdcall get_Socket(void**) noexcept = 0;
        };
    };
    template <> struct abi<Windows::Networking::Proximity::DeviceArrivedEventHandler>
    {
        struct __declspec(novtable) type : unknown_abi
        {
            virtual int32_t __stdcall Invoke(void*) noexcept = 0;
        };
    };
    template <> struct abi<Windows::Networking::Proximity::DeviceDepartedEventHandler>
    {
        struct __declspec(novtable) type : unknown_abi
        {
            virtual int32_t __stdcall Invoke(void*) noexcept = 0;
        };
    };
    template <> struct abi<Windows::Networking::Proximity::MessageReceivedHandler>
    {
        struct __declspec(novtable) type : unknown_abi
        {
            virtual int32_t __stdcall Invoke(void*, void*) noexcept = 0;
        };
    };
    template <> struct abi<Windows::Networking::Proximity::MessageTransmittedHandler>
    {
        struct __declspec(novtable) type : unknown_abi
        {
            virtual int32_t __stdcall Invoke(void*, int64_t) noexcept = 0;
        };
    };
    template <typename D>
    struct consume_Windows_Networking_Proximity_IConnectionRequestedEventArgs
    {
        [[nodiscard]] auto PeerInformation() const;
    };
    template <> struct consume<Windows::Networking::Proximity::IConnectionRequestedEventArgs>
    {
        template <typename D> using type = consume_Windows_Networking_Proximity_IConnectionRequestedEventArgs<D>;
    };
    template <typename D>
    struct consume_Windows_Networking_Proximity_IPeerFinderStatics
    {
        [[nodiscard]] auto AllowBluetooth() const;
        auto AllowBluetooth(bool value) const;
        [[nodiscard]] auto AllowInfrastructure() const;
        auto AllowInfrastructure(bool value) const;
        [[nodiscard]] auto AllowWiFiDirect() const;
        auto AllowWiFiDirect(bool value) const;
        [[nodiscard]] auto DisplayName() const;
        auto DisplayName(param::hstring const& value) const;
        [[nodiscard]] auto SupportedDiscoveryTypes() const;
        [[nodiscard]] auto AlternateIdentities() const;
        auto Start() const;
        auto Start(param::hstring const& peerMessage) const;
        auto Stop() const;
        auto TriggeredConnectionStateChanged(Windows::Foundation::TypedEventHandler<Windows::Foundation::IInspectable, Windows::Networking::Proximity::TriggeredConnectionStateChangedEventArgs> const& handler) const;
        using TriggeredConnectionStateChanged_revoker = impl::event_revoker<Windows::Networking::Proximity::IPeerFinderStatics, &impl::abi_t<Windows::Networking::Proximity::IPeerFinderStatics>::remove_TriggeredConnectionStateChanged>;
        [[nodiscard]] TriggeredConnectionStateChanged_revoker TriggeredConnectionStateChanged(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::Foundation::IInspectable, Windows::Networking::Proximity::TriggeredConnectionStateChangedEventArgs> const& handler) const;
        auto TriggeredConnectionStateChanged(winrt::event_token const& cookie) const noexcept;
        auto ConnectionRequested(Windows::Foundation::TypedEventHandler<Windows::Foundation::IInspectable, Windows::Networking::Proximity::ConnectionRequestedEventArgs> const& handler) const;
        using ConnectionRequested_revoker = impl::event_revoker<Windows::Networking::Proximity::IPeerFinderStatics, &impl::abi_t<Windows::Networking::Proximity::IPeerFinderStatics>::remove_ConnectionRequested>;
        [[nodiscard]] ConnectionRequested_revoker ConnectionRequested(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::Foundation::IInspectable, Windows::Networking::Proximity::ConnectionRequestedEventArgs> const& handler) const;
        auto ConnectionRequested(winrt::event_token const& cookie) const noexcept;
        auto FindAllPeersAsync() const;
        auto ConnectAsync(Windows::Networking::Proximity::PeerInformation const& peerInformation) const;
    };
    template <> struct consume<Windows::Networking::Proximity::IPeerFinderStatics>
    {
        template <typename D> using type = consume_Windows_Networking_Proximity_IPeerFinderStatics<D>;
    };
    template <typename D>
    struct consume_Windows_Networking_Proximity_IPeerFinderStatics2
    {
        [[nodiscard]] auto Role() const;
        auto Role(Windows::Networking::Proximity::PeerRole const& value) const;
        [[nodiscard]] auto DiscoveryData() const;
        auto DiscoveryData(Windows::Storage::Streams::IBuffer const& value) const;
        auto CreateWatcher() const;
    };
    template <> struct consume<Windows::Networking::Proximity::IPeerFinderStatics2>
    {
        template <typename D> using type = consume_Windows_Networking_Proximity_IPeerFinderStatics2<D>;
    };
    template <typename D>
    struct consume_Windows_Networking_Proximity_IPeerInformation
    {
        [[nodiscard]] auto DisplayName() const;
    };
    template <> struct consume<Windows::Networking::Proximity::IPeerInformation>
    {
        template <typename D> using type = consume_Windows_Networking_Proximity_IPeerInformation<D>;
    };
    template <typename D>
    struct consume_Windows_Networking_Proximity_IPeerInformation3
    {
        [[nodiscard]] auto Id() const;
        [[nodiscard]] auto DiscoveryData() const;
    };
    template <> struct consume<Windows::Networking::Proximity::IPeerInformation3>
    {
        template <typename D> using type = consume_Windows_Networking_Proximity_IPeerInformation3<D>;
    };
    template <typename D>
    struct consume_Windows_Networking_Proximity_IPeerInformationWithHostAndService
    {
        [[nodiscard]] auto HostName() const;
        [[nodiscard]] auto ServiceName() const;
    };
    template <> struct consume<Windows::Networking::Proximity::IPeerInformationWithHostAndService>
    {
        template <typename D> using type = consume_Windows_Networking_Proximity_IPeerInformationWithHostAndService<D>;
    };
    template <typename D>
    struct consume_Windows_Networking_Proximity_IPeerWatcher
    {
        auto Added(Windows::Foundation::TypedEventHandler<Windows::Networking::Proximity::PeerWatcher, Windows::Networking::Proximity::PeerInformation> const& handler) const;
        using Added_revoker = impl::event_revoker<Windows::Networking::Proximity::IPeerWatcher, &impl::abi_t<Windows::Networking::Proximity::IPeerWatcher>::remove_Added>;
        [[nodiscard]] Added_revoker Added(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::Networking::Proximity::PeerWatcher, Windows::Networking::Proximity::PeerInformation> const& handler) const;
        auto Added(winrt::event_token const& token) const noexcept;
        auto Removed(Windows::Foundation::TypedEventHandler<Windows::Networking::Proximity::PeerWatcher, Windows::Networking::Proximity::PeerInformation> const& handler) const;
        using Removed_revoker = impl::event_revoker<Windows::Networking::Proximity::IPeerWatcher, &impl::abi_t<Windows::Networking::Proximity::IPeerWatcher>::remove_Removed>;
        [[nodiscard]] Removed_revoker Removed(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::Networking::Proximity::PeerWatcher, Windows::Networking::Proximity::PeerInformation> const& handler) const;
        auto Removed(winrt::event_token const& token) const noexcept;
        auto Updated(Windows::Foundation::TypedEventHandler<Windows::Networking::Proximity::PeerWatcher, Windows::Networking::Proximity::PeerInformation> const& handler) const;
        using Updated_revoker = impl::event_revoker<Windows::Networking::Proximity::IPeerWatcher, &impl::abi_t<Windows::Networking::Proximity::IPeerWatcher>::remove_Updated>;
        [[nodiscard]] Updated_revoker Updated(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::Networking::Proximity::PeerWatcher, Windows::Networking::Proximity::PeerInformation> const& handler) const;
        auto Updated(winrt::event_token const& token) const noexcept;
        auto EnumerationCompleted(Windows::Foundation::TypedEventHandler<Windows::Networking::Proximity::PeerWatcher, Windows::Foundation::IInspectable> const& handler) const;
        using EnumerationCompleted_revoker = impl::event_revoker<Windows::Networking::Proximity::IPeerWatcher, &impl::abi_t<Windows::Networking::Proximity::IPeerWatcher>::remove_EnumerationCompleted>;
        [[nodiscard]] EnumerationCompleted_revoker EnumerationCompleted(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::Networking::Proximity::PeerWatcher, Windows::Foundation::IInspectable> const& handler) const;
        auto EnumerationCompleted(winrt::event_token const& token) const noexcept;
        auto Stopped(Windows::Foundation::TypedEventHandler<Windows::Networking::Proximity::PeerWatcher, Windows::Foundation::IInspectable> const& handler) const;
        using Stopped_revoker = impl::event_revoker<Windows::Networking::Proximity::IPeerWatcher, &impl::abi_t<Windows::Networking::Proximity::IPeerWatcher>::remove_Stopped>;
        [[nodiscard]] Stopped_revoker Stopped(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::Networking::Proximity::PeerWatcher, Windows::Foundation::IInspectable> const& handler) const;
        auto Stopped(winrt::event_token const& token) const noexcept;
        [[nodiscard]] auto Status() const;
        auto Start() const;
        auto Stop() const;
    };
    template <> struct consume<Windows::Networking::Proximity::IPeerWatcher>
    {
        template <typename D> using type = consume_Windows_Networking_Proximity_IPeerWatcher<D>;
    };
    template <typename D>
    struct consume_Windows_Networking_Proximity_IProximityDevice
    {
        auto SubscribeForMessage(param::hstring const& messageType, Windows::Networking::Proximity::MessageReceivedHandler const& messageReceivedHandler) const;
        auto PublishMessage(param::hstring const& messageType, param::hstring const& message) const;
        auto PublishMessage(param::hstring const& messageType, param::hstring const& message, Windows::Networking::Proximity::MessageTransmittedHandler const& messageTransmittedHandler) const;
        auto PublishBinaryMessage(param::hstring const& messageType, Windows::Storage::Streams::IBuffer const& message) const;
        auto PublishBinaryMessage(param::hstring const& messageType, Windows::Storage::Streams::IBuffer const& message, Windows::Networking::Proximity::MessageTransmittedHandler const& messageTransmittedHandler) const;
        auto PublishUriMessage(Windows::Foundation::Uri const& message) const;
        auto PublishUriMessage(Windows::Foundation::Uri const& message, Windows::Networking::Proximity::MessageTransmittedHandler const& messageTransmittedHandler) const;
        auto StopSubscribingForMessage(int64_t subscriptionId) const;
        auto StopPublishingMessage(int64_t messageId) const;
        auto DeviceArrived(Windows::Networking::Proximity::DeviceArrivedEventHandler const& arrivedHandler) const;
        using DeviceArrived_revoker = impl::event_revoker<Windows::Networking::Proximity::IProximityDevice, &impl::abi_t<Windows::Networking::Proximity::IProximityDevice>::remove_DeviceArrived>;
        [[nodiscard]] DeviceArrived_revoker DeviceArrived(auto_revoke_t, Windows::Networking::Proximity::DeviceArrivedEventHandler const& arrivedHandler) const;
        auto DeviceArrived(winrt::event_token const& cookie) const noexcept;
        auto DeviceDeparted(Windows::Networking::Proximity::DeviceDepartedEventHandler const& departedHandler) const;
        using DeviceDeparted_revoker = impl::event_revoker<Windows::Networking::Proximity::IProximityDevice, &impl::abi_t<Windows::Networking::Proximity::IProximityDevice>::remove_DeviceDeparted>;
        [[nodiscard]] DeviceDeparted_revoker DeviceDeparted(auto_revoke_t, Windows::Networking::Proximity::DeviceDepartedEventHandler const& departedHandler) const;
        auto DeviceDeparted(winrt::event_token const& cookie) const noexcept;
        [[nodiscard]] auto MaxMessageBytes() const;
        [[nodiscard]] auto BitsPerSecond() const;
        [[nodiscard]] auto DeviceId() const;
    };
    template <> struct consume<Windows::Networking::Proximity::IProximityDevice>
    {
        template <typename D> using type = consume_Windows_Networking_Proximity_IProximityDevice<D>;
    };
    template <typename D>
    struct consume_Windows_Networking_Proximity_IProximityDeviceStatics
    {
        auto GetDeviceSelector() const;
        auto GetDefault() const;
        auto FromId(param::hstring const& deviceId) const;
    };
    template <> struct consume<Windows::Networking::Proximity::IProximityDeviceStatics>
    {
        template <typename D> using type = consume_Windows_Networking_Proximity_IProximityDeviceStatics<D>;
    };
    template <typename D>
    struct consume_Windows_Networking_Proximity_IProximityMessage
    {
        [[nodiscard]] auto MessageType() const;
        [[nodiscard]] auto SubscriptionId() const;
        [[nodiscard]] auto Data() const;
        [[nodiscard]] auto DataAsString() const;
    };
    template <> struct consume<Windows::Networking::Proximity::IProximityMessage>
    {
        template <typename D> using type = consume_Windows_Networking_Proximity_IProximityMessage<D>;
    };
    template <typename D>
    struct consume_Windows_Networking_Proximity_ITriggeredConnectionStateChangedEventArgs
    {
        [[nodiscard]] auto State() const;
        [[nodiscard]] auto Id() const;
        [[nodiscard]] auto Socket() const;
    };
    template <> struct consume<Windows::Networking::Proximity::ITriggeredConnectionStateChangedEventArgs>
    {
        template <typename D> using type = consume_Windows_Networking_Proximity_ITriggeredConnectionStateChangedEventArgs<D>;
    };
}
#endif
