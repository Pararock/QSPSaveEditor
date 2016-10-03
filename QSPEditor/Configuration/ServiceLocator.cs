using Microsoft.Extensions.DependencyInjection;
using QSPEditor.Services;
using QSPEditor.ViewModels;
//using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using Windows.UI.ViewManagement;

namespace QSPEditor.Configuration
{
    public class ServiceLocator : IDisposable
    {
        static private readonly ConcurrentDictionary<int, ServiceLocator> _serviceLocators = new ConcurrentDictionary<int, ServiceLocator>();

        static private ServiceProvider _rootServiceProvider = null;

        static public void Configure(IServiceCollection serviceCollection)
        {

            //serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();
            serviceCollection.AddSingleton<IWindowManagerService, WindowManagerService>();
            serviceCollection.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            serviceCollection.AddSingleton<IRecentFilesService, RecentFilesService>();

            //serviceCollection.AddSingleton<IMessageService, MessageService>();

            serviceCollection.AddSingleton<IFilePickerService, FilePickerService>();

            serviceCollection.AddScoped<IDialogService, DialogService>();
            serviceCollection.AddScoped<IEngineService, EngineService>();

            //serviceCollection.AddScoped<IContextService, ContextService>();
            serviceCollection.AddScoped<INavigationService, NavigationServiceEx>();
            serviceCollection.AddScoped<IQspStreamUriResolver, QspStreamUriResolver>();
            //serviceCollection.AddScoped<ICommonServices, CommonServices>();

            serviceCollection.AddTransient<ShellViewModel>();

            serviceCollection.AddTransient<LocationsViewModel>();
            serviceCollection.AddTransient<MainViewModel>();
            serviceCollection.AddTransient<HomeViewModel>();
            serviceCollection.AddTransient<VariablesViewModel>();
            serviceCollection.AddTransient<SettingsViewModel>();

            _rootServiceProvider = serviceCollection.BuildServiceProvider();
        }

        static public ServiceLocator Current
        {
            get
            {
                int currentViewId = ApplicationView.GetForCurrentView().Id;
                return _serviceLocators.GetOrAdd(currentViewId, key => new ServiceLocator());
            }
        }

        static public void DisposeCurrent()
        {
            int currentViewId = ApplicationView.GetForCurrentView().Id;
            if (_serviceLocators.TryRemove(currentViewId, out ServiceLocator current))
            {
                current.Dispose();
            }
        }

        private IServiceScope _serviceScope = null;

        private ServiceLocator()
        {
            _serviceScope = _rootServiceProvider.CreateScope();
        }

        public T GetService<T>()
        {
            return GetService<T>(true);
        }

        public T GetService<T>(bool isRequired)
        {
            if (isRequired)
            {
                return _serviceScope.ServiceProvider.GetRequiredService<T>();
            }
            return _serviceScope.ServiceProvider.GetService<T>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_serviceScope != null)
                {
                    _serviceScope.Dispose();
                }
            }
        }
    }
}
