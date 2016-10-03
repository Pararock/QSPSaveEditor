// WARNING: Please don't edit this file. It was generated by C++/WinRT v2.0.191018.6

#ifndef WINRT_Windows_ApplicationModel_Search_Core_0_H
#define WINRT_Windows_ApplicationModel_Search_Core_0_H
WINRT_EXPORT namespace winrt::Windows::ApplicationModel::Search
{
    struct LocalContentSuggestionSettings;
    struct SearchQueryLinguisticDetails;
    struct SearchSuggestionsRequest;
}
WINRT_EXPORT namespace winrt::Windows::Foundation
{
    struct EventRegistrationToken;
    template <typename TSender, typename TResult> struct TypedEventHandler;
}
WINRT_EXPORT namespace winrt::Windows::Storage::Streams
{
    struct IRandomAccessStreamReference;
}
WINRT_EXPORT namespace winrt::Windows::ApplicationModel::Search::Core
{
    enum class SearchSuggestionKind : int32_t
    {
        Query = 0,
        Result = 1,
        Separator = 2,
    };
    struct IRequestingFocusOnKeyboardInputEventArgs;
    struct ISearchSuggestion;
    struct ISearchSuggestionManager;
    struct ISearchSuggestionsRequestedEventArgs;
    struct RequestingFocusOnKeyboardInputEventArgs;
    struct SearchSuggestion;
    struct SearchSuggestionManager;
    struct SearchSuggestionsRequestedEventArgs;
}
namespace winrt::impl
{
    template <> struct category<Windows::ApplicationModel::Search::Core::IRequestingFocusOnKeyboardInputEventArgs>{ using type = interface_category; };
    template <> struct category<Windows::ApplicationModel::Search::Core::ISearchSuggestion>{ using type = interface_category; };
    template <> struct category<Windows::ApplicationModel::Search::Core::ISearchSuggestionManager>{ using type = interface_category; };
    template <> struct category<Windows::ApplicationModel::Search::Core::ISearchSuggestionsRequestedEventArgs>{ using type = interface_category; };
    template <> struct category<Windows::ApplicationModel::Search::Core::RequestingFocusOnKeyboardInputEventArgs>{ using type = class_category; };
    template <> struct category<Windows::ApplicationModel::Search::Core::SearchSuggestion>{ using type = class_category; };
    template <> struct category<Windows::ApplicationModel::Search::Core::SearchSuggestionManager>{ using type = class_category; };
    template <> struct category<Windows::ApplicationModel::Search::Core::SearchSuggestionsRequestedEventArgs>{ using type = class_category; };
    template <> struct category<Windows::ApplicationModel::Search::Core::SearchSuggestionKind>{ using type = enum_category; };
    template <> inline constexpr auto& name_v<Windows::ApplicationModel::Search::Core::RequestingFocusOnKeyboardInputEventArgs>{ L"Windows.ApplicationModel.Search.Core.RequestingFocusOnKeyboardInputEventArgs" };
    template <> inline constexpr auto& name_v<Windows::ApplicationModel::Search::Core::SearchSuggestion>{ L"Windows.ApplicationModel.Search.Core.SearchSuggestion" };
    template <> inline constexpr auto& name_v<Windows::ApplicationModel::Search::Core::SearchSuggestionManager>{ L"Windows.ApplicationModel.Search.Core.SearchSuggestionManager" };
    template <> inline constexpr auto& name_v<Windows::ApplicationModel::Search::Core::SearchSuggestionsRequestedEventArgs>{ L"Windows.ApplicationModel.Search.Core.SearchSuggestionsRequestedEventArgs" };
    template <> inline constexpr auto& name_v<Windows::ApplicationModel::Search::Core::SearchSuggestionKind>{ L"Windows.ApplicationModel.Search.Core.SearchSuggestionKind" };
#ifndef WINRT_LEAN_AND_MEAN
    template <> inline constexpr auto& name_v<Windows::ApplicationModel::Search::Core::IRequestingFocusOnKeyboardInputEventArgs>{ L"Windows.ApplicationModel.Search.Core.IRequestingFocusOnKeyboardInputEventArgs" };
    template <> inline constexpr auto& name_v<Windows::ApplicationModel::Search::Core::ISearchSuggestion>{ L"Windows.ApplicationModel.Search.Core.ISearchSuggestion" };
    template <> inline constexpr auto& name_v<Windows::ApplicationModel::Search::Core::ISearchSuggestionManager>{ L"Windows.ApplicationModel.Search.Core.ISearchSuggestionManager" };
    template <> inline constexpr auto& name_v<Windows::ApplicationModel::Search::Core::ISearchSuggestionsRequestedEventArgs>{ L"Windows.ApplicationModel.Search.Core.ISearchSuggestionsRequestedEventArgs" };
#endif
    template <> inline constexpr guid guid_v<Windows::ApplicationModel::Search::Core::IRequestingFocusOnKeyboardInputEventArgs>{ 0xA1195F27,0xB1A7,0x41A2,{ 0x87,0x9D,0x6A,0x68,0x68,0x7E,0x59,0x85 } };
    template <> inline constexpr guid guid_v<Windows::ApplicationModel::Search::Core::ISearchSuggestion>{ 0x5B5554B0,0x1527,0x437B,{ 0x95,0xC5,0x8D,0x18,0xD2,0xB8,0xAF,0x55 } };
    template <> inline constexpr guid guid_v<Windows::ApplicationModel::Search::Core::ISearchSuggestionManager>{ 0x3F0C50A1,0xCB9D,0x497B,{ 0xB5,0x00,0x3C,0x04,0xAC,0x95,0x9A,0xD2 } };
    template <> inline constexpr guid guid_v<Windows::ApplicationModel::Search::Core::ISearchSuggestionsRequestedEventArgs>{ 0x6FD519E5,0x9E7E,0x4AB4,{ 0x8B,0xE3,0xC7,0x6B,0x1B,0xD4,0x34,0x4A } };
    template <> struct default_interface<Windows::ApplicationModel::Search::Core::RequestingFocusOnKeyboardInputEventArgs>{ using type = Windows::ApplicationModel::Search::Core::IRequestingFocusOnKeyboardInputEventArgs; };
    template <> struct default_interface<Windows::ApplicationModel::Search::Core::SearchSuggestion>{ using type = Windows::ApplicationModel::Search::Core::ISearchSuggestion; };
    template <> struct default_interface<Windows::ApplicationModel::Search::Core::SearchSuggestionManager>{ using type = Windows::ApplicationModel::Search::Core::ISearchSuggestionManager; };
    template <> struct default_interface<Windows::ApplicationModel::Search::Core::SearchSuggestionsRequestedEventArgs>{ using type = Windows::ApplicationModel::Search::Core::ISearchSuggestionsRequestedEventArgs; };
    template <> struct abi<Windows::ApplicationModel::Search::Core::IRequestingFocusOnKeyboardInputEventArgs>
    {
        struct __declspec(novtable) type : inspectable_abi
        {
        };
    };
    template <> struct abi<Windows::ApplicationModel::Search::Core::ISearchSuggestion>
    {
        struct __declspec(novtable) type : inspectable_abi
        {
            virtual int32_t __stdcall get_Kind(int32_t*) noexcept = 0;
            virtual int32_t __stdcall get_Text(void**) noexcept = 0;
            virtual int32_t __stdcall get_Tag(void**) noexcept = 0;
            virtual int32_t __stdcall get_DetailText(void**) noexcept = 0;
            virtual int32_t __stdcall get_Image(void**) noexcept = 0;
            virtual int32_t __stdcall get_ImageAlternateText(void**) noexcept = 0;
        };
    };
    template <> struct abi<Windows::ApplicationModel::Search::Core::ISearchSuggestionManager>
    {
        struct __declspec(novtable) type : inspectable_abi
        {
            virtual int32_t __stdcall get_SearchHistoryEnabled(bool*) noexcept = 0;
            virtual int32_t __stdcall put_SearchHistoryEnabled(bool) noexcept = 0;
            virtual int32_t __stdcall get_SearchHistoryContext(void**) noexcept = 0;
            virtual int32_t __stdcall put_SearchHistoryContext(void*) noexcept = 0;
            virtual int32_t __stdcall SetLocalContentSuggestionSettings(void*) noexcept = 0;
            virtual int32_t __stdcall SetQuery(void*) noexcept = 0;
            virtual int32_t __stdcall SetQueryWithLanguage(void*, void*) noexcept = 0;
            virtual int32_t __stdcall SetQueryWithSearchQueryLinguisticDetails(void*, void*, void*) noexcept = 0;
            virtual int32_t __stdcall get_Suggestions(void**) noexcept = 0;
            virtual int32_t __stdcall AddToHistory(void*) noexcept = 0;
            virtual int32_t __stdcall AddToHistoryWithLanguage(void*, void*) noexcept = 0;
            virtual int32_t __stdcall ClearHistory() noexcept = 0;
            virtual int32_t __stdcall add_SuggestionsRequested(void*, winrt::event_token*) noexcept = 0;
            virtual int32_t __stdcall remove_SuggestionsRequested(winrt::event_token) noexcept = 0;
            virtual int32_t __stdcall add_RequestingFocusOnKeyboardInput(void*, winrt::event_token*) noexcept = 0;
            virtual int32_t __stdcall remove_RequestingFocusOnKeyboardInput(winrt::event_token) noexcept = 0;
        };
    };
    template <> struct abi<Windows::ApplicationModel::Search::Core::ISearchSuggestionsRequestedEventArgs>
    {
        struct __declspec(novtable) type : inspectable_abi
        {
            virtual int32_t __stdcall get_QueryText(void**) noexcept = 0;
            virtual int32_t __stdcall get_Language(void**) noexcept = 0;
            virtual int32_t __stdcall get_LinguisticDetails(void**) noexcept = 0;
            virtual int32_t __stdcall get_Request(void**) noexcept = 0;
        };
    };
    template <typename D>
    struct consume_Windows_ApplicationModel_Search_Core_IRequestingFocusOnKeyboardInputEventArgs
    {
    };
    template <> struct consume<Windows::ApplicationModel::Search::Core::IRequestingFocusOnKeyboardInputEventArgs>
    {
        template <typename D> using type = consume_Windows_ApplicationModel_Search_Core_IRequestingFocusOnKeyboardInputEventArgs<D>;
    };
    template <typename D>
    struct consume_Windows_ApplicationModel_Search_Core_ISearchSuggestion
    {
        [[nodiscard]] auto Kind() const;
        [[nodiscard]] auto Text() const;
        [[nodiscard]] auto Tag() const;
        [[nodiscard]] auto DetailText() const;
        [[nodiscard]] auto Image() const;
        [[nodiscard]] auto ImageAlternateText() const;
    };
    template <> struct consume<Windows::ApplicationModel::Search::Core::ISearchSuggestion>
    {
        template <typename D> using type = consume_Windows_ApplicationModel_Search_Core_ISearchSuggestion<D>;
    };
    template <typename D>
    struct consume_Windows_ApplicationModel_Search_Core_ISearchSuggestionManager
    {
        [[nodiscard]] auto SearchHistoryEnabled() const;
        auto SearchHistoryEnabled(bool value) const;
        [[nodiscard]] auto SearchHistoryContext() const;
        auto SearchHistoryContext(param::hstring const& value) const;
        auto SetLocalContentSuggestionSettings(Windows::ApplicationModel::Search::LocalContentSuggestionSettings const& settings) const;
        auto SetQuery(param::hstring const& queryText) const;
        auto SetQuery(param::hstring const& queryText, param::hstring const& language) const;
        auto SetQuery(param::hstring const& queryText, param::hstring const& language, Windows::ApplicationModel::Search::SearchQueryLinguisticDetails const& linguisticDetails) const;
        [[nodiscard]] auto Suggestions() const;
        auto AddToHistory(param::hstring const& queryText) const;
        auto AddToHistory(param::hstring const& queryText, param::hstring const& language) const;
        auto ClearHistory() const;
        auto SuggestionsRequested(Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Search::Core::SearchSuggestionManager, Windows::ApplicationModel::Search::Core::SearchSuggestionsRequestedEventArgs> const& handler) const;
        using SuggestionsRequested_revoker = impl::event_revoker<Windows::ApplicationModel::Search::Core::ISearchSuggestionManager, &impl::abi_t<Windows::ApplicationModel::Search::Core::ISearchSuggestionManager>::remove_SuggestionsRequested>;
        [[nodiscard]] SuggestionsRequested_revoker SuggestionsRequested(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Search::Core::SearchSuggestionManager, Windows::ApplicationModel::Search::Core::SearchSuggestionsRequestedEventArgs> const& handler) const;
        auto SuggestionsRequested(winrt::event_token const& token) const noexcept;
        auto RequestingFocusOnKeyboardInput(Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Search::Core::SearchSuggestionManager, Windows::ApplicationModel::Search::Core::RequestingFocusOnKeyboardInputEventArgs> const& handler) const;
        using RequestingFocusOnKeyboardInput_revoker = impl::event_revoker<Windows::ApplicationModel::Search::Core::ISearchSuggestionManager, &impl::abi_t<Windows::ApplicationModel::Search::Core::ISearchSuggestionManager>::remove_RequestingFocusOnKeyboardInput>;
        [[nodiscard]] RequestingFocusOnKeyboardInput_revoker RequestingFocusOnKeyboardInput(auto_revoke_t, Windows::Foundation::TypedEventHandler<Windows::ApplicationModel::Search::Core::SearchSuggestionManager, Windows::ApplicationModel::Search::Core::RequestingFocusOnKeyboardInputEventArgs> const& handler) const;
        auto RequestingFocusOnKeyboardInput(winrt::event_token const& token) const noexcept;
    };
    template <> struct consume<Windows::ApplicationModel::Search::Core::ISearchSuggestionManager>
    {
        template <typename D> using type = consume_Windows_ApplicationModel_Search_Core_ISearchSuggestionManager<D>;
    };
    template <typename D>
    struct consume_Windows_ApplicationModel_Search_Core_ISearchSuggestionsRequestedEventArgs
    {
        [[nodiscard]] auto QueryText() const;
        [[nodiscard]] auto Language() const;
        [[nodiscard]] auto LinguisticDetails() const;
        [[nodiscard]] auto Request() const;
    };
    template <> struct consume<Windows::ApplicationModel::Search::Core::ISearchSuggestionsRequestedEventArgs>
    {
        template <typename D> using type = consume_Windows_ApplicationModel_Search_Core_ISearchSuggestionsRequestedEventArgs<D>;
    };
}
#endif
