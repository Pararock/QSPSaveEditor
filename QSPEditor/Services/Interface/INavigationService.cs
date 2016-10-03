using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace QSPEditor.Services
{
    public interface INavigationService
    {
        bool CanGoBack { get; }
        bool CanGoForward { get; }
        Frame Frame { get; set; }

        event NavigatedEventHandler Navigated;
        event NavigationFailedEventHandler NavigationFailed;

        bool GoBack();
        void GoForward();
        bool Navigate(string pageKey, object parameter = null, NavigationTransitionInfo infoOverride = null);
        bool Navigate<T>(object parameter = null, NavigationTransitionInfo infoOverride = null) where T : Page;
        void Configure(string key, Type pageType);
        void Configure<VM, Type>() where Type : Page;
        string GetNameOfRegisteredPage(Type page);
    }
}
