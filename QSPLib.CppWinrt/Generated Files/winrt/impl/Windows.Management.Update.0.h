// WARNING: Please don't edit this file. It was generated by C++/WinRT v2.0.191018.6

#ifndef WINRT_Windows_Management_Update_0_H
#define WINRT_Windows_Management_Update_0_H
WINRT_EXPORT namespace winrt::Windows::Foundation::Collections
{
    struct ValueSet;
}
WINRT_EXPORT namespace winrt::Windows::Management::Update
{
    struct IPreviewBuildsManager;
    struct IPreviewBuildsManagerStatics;
    struct IPreviewBuildsState;
    struct PreviewBuildsManager;
    struct PreviewBuildsState;
}
namespace winrt::impl
{
    template <> struct category<Windows::Management::Update::IPreviewBuildsManager>{ using type = interface_category; };
    template <> struct category<Windows::Management::Update::IPreviewBuildsManagerStatics>{ using type = interface_category; };
    template <> struct category<Windows::Management::Update::IPreviewBuildsState>{ using type = interface_category; };
    template <> struct category<Windows::Management::Update::PreviewBuildsManager>{ using type = class_category; };
    template <> struct category<Windows::Management::Update::PreviewBuildsState>{ using type = class_category; };
    template <> inline constexpr auto& name_v<Windows::Management::Update::PreviewBuildsManager>{ L"Windows.Management.Update.PreviewBuildsManager" };
    template <> inline constexpr auto& name_v<Windows::Management::Update::PreviewBuildsState>{ L"Windows.Management.Update.PreviewBuildsState" };
#ifndef WINRT_LEAN_AND_MEAN
    template <> inline constexpr auto& name_v<Windows::Management::Update::IPreviewBuildsManager>{ L"Windows.Management.Update.IPreviewBuildsManager" };
    template <> inline constexpr auto& name_v<Windows::Management::Update::IPreviewBuildsManagerStatics>{ L"Windows.Management.Update.IPreviewBuildsManagerStatics" };
    template <> inline constexpr auto& name_v<Windows::Management::Update::IPreviewBuildsState>{ L"Windows.Management.Update.IPreviewBuildsState" };
#endif
    template <> inline constexpr guid guid_v<Windows::Management::Update::IPreviewBuildsManager>{ 0xFA07DD61,0x7E4F,0x59F7,{ 0x7C,0x9F,0xDE,0xF9,0x05,0x1C,0x5F,0x62 } };
    template <> inline constexpr guid guid_v<Windows::Management::Update::IPreviewBuildsManagerStatics>{ 0x3E422887,0xB112,0x5A70,{ 0x7D,0xA1,0x97,0xD7,0x8D,0x32,0xAA,0x29 } };
    template <> inline constexpr guid guid_v<Windows::Management::Update::IPreviewBuildsState>{ 0xA2F2903E,0xB223,0x5F63,{ 0x75,0x46,0x3E,0x8E,0xAC,0x07,0x0A,0x2E } };
    template <> struct default_interface<Windows::Management::Update::PreviewBuildsManager>{ using type = Windows::Management::Update::IPreviewBuildsManager; };
    template <> struct default_interface<Windows::Management::Update::PreviewBuildsState>{ using type = Windows::Management::Update::IPreviewBuildsState; };
    template <> struct abi<Windows::Management::Update::IPreviewBuildsManager>
    {
        struct __declspec(novtable) type : inspectable_abi
        {
            virtual int32_t __stdcall get_ArePreviewBuildsAllowed(bool*) noexcept = 0;
            virtual int32_t __stdcall put_ArePreviewBuildsAllowed(bool) noexcept = 0;
            virtual int32_t __stdcall GetCurrentState(void**) noexcept = 0;
            virtual int32_t __stdcall SyncAsync(void**) noexcept = 0;
        };
    };
    template <> struct abi<Windows::Management::Update::IPreviewBuildsManagerStatics>
    {
        struct __declspec(novtable) type : inspectable_abi
        {
            virtual int32_t __stdcall GetDefault(void**) noexcept = 0;
            virtual int32_t __stdcall IsSupported(bool*) noexcept = 0;
        };
    };
    template <> struct abi<Windows::Management::Update::IPreviewBuildsState>
    {
        struct __declspec(novtable) type : inspectable_abi
        {
            virtual int32_t __stdcall get_Properties(void**) noexcept = 0;
        };
    };
    template <typename D>
    struct consume_Windows_Management_Update_IPreviewBuildsManager
    {
        [[nodiscard]] auto ArePreviewBuildsAllowed() const;
        auto ArePreviewBuildsAllowed(bool value) const;
        auto GetCurrentState() const;
        auto SyncAsync() const;
    };
    template <> struct consume<Windows::Management::Update::IPreviewBuildsManager>
    {
        template <typename D> using type = consume_Windows_Management_Update_IPreviewBuildsManager<D>;
    };
    template <typename D>
    struct consume_Windows_Management_Update_IPreviewBuildsManagerStatics
    {
        auto GetDefault() const;
        auto IsSupported() const;
    };
    template <> struct consume<Windows::Management::Update::IPreviewBuildsManagerStatics>
    {
        template <typename D> using type = consume_Windows_Management_Update_IPreviewBuildsManagerStatics<D>;
    };
    template <typename D>
    struct consume_Windows_Management_Update_IPreviewBuildsState
    {
        [[nodiscard]] auto Properties() const;
    };
    template <> struct consume<Windows::Management::Update::IPreviewBuildsState>
    {
        template <typename D> using type = consume_Windows_Management_Update_IPreviewBuildsState<D>;
    };
}
#endif
