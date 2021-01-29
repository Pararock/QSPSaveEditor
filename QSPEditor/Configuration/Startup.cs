using Microsoft.Extensions.DependencyInjection;
using QSPEditor.Services;
using QSPEditor.ViewModels;
using QSPEditor.Views;
using System.Threading.Tasks;

namespace QSPEditor.Configuration
{
    static public class Startup
    {
        static private readonly ServiceCollection _serviceCollection = new ServiceCollection();
        static public async Task<INavigationService> ConfigureAsync()
        {
            ServiceLocator.Configure(_serviceCollection);

            ConfigureNavigation();

            await EnsureThemeAsync();
            await EnsureRecentFilesAsync();
            await EnsureWindowManagerServiceAsync();

            //var logService = ServiceLocator.Current.GetService<ILogService>();
            //await logService.WriteAsync(Data.LogType.Information, "Startup", "Configuration", "Application Start", $"Application started.");
            return ServiceLocator.Current.GetService<INavigationService>();
        }

        private static async Task EnsureWindowManagerServiceAsync()
        {
            await ServiceLocator.Current.GetService<IWindowManagerService>().InitializeAsync();
        }

        static public async Task StartupAsync()
        {
            await ServiceLocator.Current.GetService<IThemeSelectorService>().SetRequestedThemeAsync();
            await QspStreamUriResolver.StartupAsync();
        }

        private static void ConfigureNavigation()
        {
            var navigationService = ServiceLocator.Current.GetService<INavigationService>();

            navigationService.Configure<ShellViewModel, ShellPage>();
            navigationService.Configure<HomeViewModel, HomePage>();
            navigationService.Configure<LocationsViewModel, LocationsPage>();
            navigationService.Configure<VariablesViewModel, VariablesPage>();
            navigationService.Configure<MainViewModel, MainPage>();
            navigationService.Configure<SettingsViewModel, SettingsPage>();
        }

        static private async Task EnsureRecentFilesAsync()
        {
            await ServiceLocator.Current.GetService<IRecentFilesService>().InitializeAsync();
        }

        static private async Task EnsureThemeAsync()
        {
            await ServiceLocator.Current.GetService<IThemeSelectorService>().InitializeAsync();
        }
    }
}
