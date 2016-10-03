using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace QSPEditor.Services
{
    public interface IWindowManagerService
    {
        CoreDispatcher MainDispatcher { get; }
        int MainViewId { get; }
        ObservableCollection<ViewLifetimeControl> SecondaryViews { get; }

        Task InitializeAsync();
        bool IsWindowOpen(string windowTitle);
        Task<ViewLifetimeControl> TryShowAsStandaloneAsync(string windowTitle, Type pageType);
        Task<ViewLifetimeControl> TryShowAsViewModeAsync(string windowTitle, Type pageType, ApplicationViewMode viewMode = ApplicationViewMode.Default);
    }
}