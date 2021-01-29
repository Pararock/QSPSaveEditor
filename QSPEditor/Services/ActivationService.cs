using QSPEditor.Activation;
using QSPEditor.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace QSPEditor.Services
{
    // For more information on understanding and extending activation flow see
    // https://github.com/Microsoft/WindowsTemplateStudio/blob/release/docs/UWP/activation.md
    internal class ActivationService
    {
        private readonly App _app;
        private readonly Type _defaultNavItem;
        private Lazy<UIElement> _shell;
        private INavigationService _navigationService;

        private object _lastActivationArgs;

        public ActivationService(App app, Type defaultNavItem, Lazy<UIElement> shell = null)
        {
            _app = app;
            _shell = shell;
            _defaultNavItem = defaultNavItem;
        }

        public async Task ActivateAsync(object activationArgs)
        {
            if (IsInteractive(activationArgs))
            {
                // Initialize services that you need before app activation
                // take into account that the splash screen is shown while this code runs.
                _navigationService = await InitializeAsync();

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (Window.Current.Content == null)
                {
                    // Create a Shell or Frame to act as the navigation context
                    Window.Current.Content = _shell?.Value ?? new Frame();

                    // Guide on keyboard accelerator
                    // https://docs.microsoft.com/en-us/windows/uwp/design/basics/navigation-history-and-backwards-navigation#code-examples
                    Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += CoreDispatcher_AcceleratorKeyActivated;
                    SystemNavigationManager.GetForCurrentView().BackRequested  += System_BackRequested;
                    Window.Current.CoreWindow.PointerPressed += CoreWindow_PointerPressed;
                }
            }

            // Depending on activationArgs one of ActivationHandlers or DefaultActivationHandler
            // will navigate to the first page
            await HandleActivationAsync(activationArgs);
            _lastActivationArgs = activationArgs;

            if (IsInteractive(activationArgs))
            {
                // Ensure the current window is active
                Window.Current.Activate();

                // Tasks after activation
                await StartupAsync();
            }
        }

        private void CoreWindow_PointerPressed(CoreWindow sender, PointerEventArgs e)
        {
            // For this event, e.Handled arrives as 'true', so invert the value.
            if (e.CurrentPoint.Properties.IsXButton1Pressed
                && e.Handled)
            {
                e.Handled = !TryGoBack();
            }
            else if (e.CurrentPoint.Properties.IsXButton2Pressed
                    && e.Handled)
            {
                e.Handled = !TryGoForward();
            }
        }

        private void System_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = TryGoBack();
            }
        }

        public bool TryGoBack()
        {
            
            if (_navigationService.CanGoBack)
            {
                _navigationService.GoBack();
                return true;
            }
            return false;
        }

        private bool TryGoForward()
        {
            if (_navigationService.CanGoForward)
            {
                _navigationService.GoForward();
                return true;
            }
            return false;
        }

        private void CoreDispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs e)
        {
            // When Alt+Left are pressed navigate back.
            // When Alt+Right are pressed navigate forward.
            if (e.EventType == CoreAcceleratorKeyEventType.SystemKeyDown
                && (e.VirtualKey == VirtualKey.Left || e.VirtualKey == VirtualKey.Right)
                && e.KeyStatus.IsMenuKeyDown == true
                && !e.Handled)
            {
                if (e.VirtualKey == VirtualKey.Left)
                {
                    e.Handled = TryGoBack();
                }
                else if (e.VirtualKey == VirtualKey.Right)
                {
                    e.Handled = TryGoForward();
                }
            }
        }

        private async Task<INavigationService> InitializeAsync()
        {
            return await Startup.ConfigureAsync();
        }

        private async Task HandleActivationAsync(object activationArgs)
        {
            var activationHandler = GetActivationHandlers()
                                                .FirstOrDefault(h => h.CanHandle(activationArgs));

            if (activationHandler != null)
            {
                await activationHandler.HandleAsync(activationArgs);
            }

            if (IsInteractive(activationArgs))
            {
                var defaultHandler = new DefaultActivationHandler(_defaultNavItem);
                if (defaultHandler.CanHandle(activationArgs))
                {
                    await defaultHandler.HandleAsync(activationArgs);
                }
            }
        }

        private async Task StartupAsync()
        {
            await Startup.StartupAsync();
        }

        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            yield break;
        }

        private bool IsInteractive(object args)
        {
            return args is IActivatedEventArgs;
        }
    }
}
