using QSPEditor.Helpers;
using QSPEditor.Services;
using QSPEditor.Views;
using QSPLib_CppWinrt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace QSPEditor.ViewModels
{
    public class ShellViewModel : Observable
    {
        private readonly KeyboardAccelerator _altLeftKeyboardAccelerator;
        private readonly KeyboardAccelerator _backKeyboardAccelerator;

        private readonly KeyboardAccelerator _ctrlOpenKeyboardAccelerator;

        private bool _isBackEnabled;
        private IList<KeyboardAccelerator> _keyboardAccelerators;
        private WinUI.NavigationView _navigationView;
        private WinUI.NavigationViewItem _selected;
        private ICommand _loadedCommand;
        private ICommand _itemInvokedCommand;

        private RelayCommand _startGameWorldCommand;
        private RelayCommand _loadGameWorldCommand;
        private RelayCommand _openSaveCommand;
        private RelayCommand _saveStateCommand;
        private int _saveProgress;
        private readonly Engine _engine;
        private readonly IFilePickerService _filePickerService;
        private readonly IRecentFilesService _recentFilesService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IWindowManagerService _windowManagerService;

        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { Set(ref _isBackEnabled, value); }
        }

        public int SaveProgress
        {
            get { return _saveProgress; }
            set { Set(ref _saveProgress, value); }
        }

        public INavigationService NavigationService => _navigationService;

        public WinUI.NavigationViewItem Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand(OnLoaded));

        public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand = new RelayCommand<WinUI.NavigationViewItemInvokedEventArgs>(OnItemInvoked));
        public ICommand LoadGameWorldCommand => _loadGameWorldCommand ?? (_loadGameWorldCommand = new RelayCommand(LoadGameWorld));
        public ICommand OpenSaveCommand => _openSaveCommand ?? (_openSaveCommand = new RelayCommand(OpenSave, CanOpenSave));
        public ICommand SaveGameStateCommand => _saveStateCommand ?? (_saveStateCommand = new RelayCommand(SaveState, CanSave));

        public ICommand StartGameWorldCommand => _startGameWorldCommand ?? (_startGameWorldCommand = new RelayCommand(StartGameWorld, CanStartGameWorld));

        public ShellViewModel(IEngineService engine, IFilePickerService filePickerService, IRecentFilesService recentFilesService, IDialogService dialogService, INavigationService navigationService, IWindowManagerService windowManagerService)
        {
            _engine = engine.Engine;
            _filePickerService = filePickerService;
            _recentFilesService = recentFilesService;
            _dialogService = dialogService;
            _navigationService = navigationService;
            _windowManagerService = windowManagerService;

            _ctrlOpenKeyboardAccelerator = BuildOpenAccelerator(VirtualKey.O, VirtualKeyModifiers.Control);
            _altLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);
            _backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);
        }

        public void Initialize(Frame frame, WinUI.NavigationView navigationView, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            _navigationView = navigationView;
            _keyboardAccelerators = keyboardAccelerators;
            NavigationService.Frame = frame;
            NavigationService.NavigationFailed += Frame_NavigationFailed;
            NavigationService.Navigated += Frame_Navigated;
            _navigationView.BackRequested += OnBackRequested;
            _engine.PropertyChanged += _engine_PropertyChanged;
        }

        private void _engine_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "isGameDirty")
            {
                _saveStateCommand.OnCanExecuteChanged();
            }
        }

        private async void LoadGameWorld()
        {
            var fileResult = await _filePickerService.OpenQSPGameAsync();
            if (fileResult != null)
            {
                var result = await _engine.LoadGameWorld(fileResult.File);
                if (result.Code == StatusCode.QSP_SUCCESS)
                {
                    await _recentFilesService.AddQSP(fileResult);
                    ValidateAppPermission();
                    _openSaveCommand.OnCanExecuteChanged();
                }
                else
                {
                    await _dialogService.ShowAsync(result);
                }

            }
        }

        private async void StartGameWorld()
        {
            var result = _engine.StartGame(true);
            if (result.Code == StatusCode.QSP_SUCCESS)
            {
            }
            else
            {
                await _dialogService.ShowAsync(result);
            }
        }

        private async void OpenSave()
        {
            var fileResult = await _filePickerService.OpenSaveAsync();
            if (fileResult != null)
            {
                var result = await _engine.OpenSavedGame(fileResult.File);
                if (result.Code == StatusCode.QSP_SUCCESS)
                {
                    //
                    _openSaveCommand.OnCanExecuteChanged();
                    _saveStateCommand.OnCanExecuteChanged();
                }
                else
                {
                    await _dialogService.ShowAsync(result);
                }
            }
        }

        private void SaveState()
        {
            var saveTask = _engine.SaveState(_engine.CurrentSave);

            saveTask.Progress = async (saveResult, progress) => await _windowManagerService.MainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                SaveProgress = (int)Math.Ceiling(progress * 100);
            });
        }

        private async void ValidateAppPermission()
        {
            if (!_engine.isFileAccessSafe)
            {
                //https://support.microsoft.com/en-us/help/4468237/windows-10-file-system-access-and-privacy-microsoft-privacy
                var launch = await _dialogService.ShowAsync(@"Need permissions for broad file access",
                    "Click Ok to open settings apps and request the settings change\n" +
                    "This is only necessary for additional content like image, game loading and save editing is still ok\n" +
                    "Note that this will crash the app\n",
                    "Open Settings", "Ok");
                if (launch)
                {
                    await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-broadfilesystemacces"));
                }
            }

        }

        private bool CanOpenSave()
        {
            return _engine.CurrentGame != null;
        }

        private bool CanSave()
        {
            return _engine.isGameDirty;
        }

        private bool CanStartGameWorld()
        {
            return true;
        }

        private async void OnLoaded()
        {
            SetupKeyboardAccelerator();
            await Task.CompletedTask;
        }

        private async void SetupKeyboardAccelerator()
        {
            // Keyboard accelerators are added here to avoid showing 'Alt + left' tooltip on the page.
            // More info on tracking issue https://github.com/Microsoft/microsoft-ui-xaml/issues/8
            _keyboardAccelerators.Add(_altLeftKeyboardAccelerator);
            _keyboardAccelerators.Add(_backKeyboardAccelerator);
            _keyboardAccelerators.Add(_ctrlOpenKeyboardAccelerator);
            await Task.CompletedTask;
        }

        private void OnItemInvoked(WinUI.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                NavigationService.Navigate(typeof(SettingsViewModel).FullName, null, args.RecommendedNavigationTransitionInfo);
                return;
            }

            if (args.InvokedItemContainer is WinUI.NavigationViewItem selectedItem)
            {
                var pageKey = selectedItem.GetValue(NavHelper.NavigateToProperty) as string;
                NavigationService.Navigate(pageKey, null, args.RecommendedNavigationTransitionInfo);
            }
        }

        private void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
            NavigationService.GoBack();
        }

        private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw e.Exception;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;
            if (e.SourcePageType == typeof(SettingsPage))
            {
                Selected = _navigationView.SettingsItem as WinUI.NavigationViewItem;
                return;
            }

            var selectedItem = GetSelectedItem(_navigationView.MenuItems, e.SourcePageType);
            if (selectedItem != null)
            {
                Selected = selectedItem;
            }
        }

        private WinUI.NavigationViewItem GetSelectedItem(IEnumerable<object> menuItems, Type pageType)
        {
            foreach (var item in menuItems.OfType<WinUI.NavigationViewItem>())
            {
                if (IsMenuItemForPageType(item, pageType))
                {
                    return item;
                }

                var selectedChild = GetSelectedItem(item.MenuItems, pageType);
                if (selectedChild != null)
                {
                    return selectedChild;
                }
            }

            return null;
        }

        private bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
        {
            var navigatedPageKey = NavigationService.GetNameOfRegisteredPage(sourcePageType);
            var pageKey = menuItem.GetValue(NavHelper.NavigateToProperty) as string;
            return pageKey == navigatedPageKey;
        }

        private KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
        {
            var keyboardAccelerator = new KeyboardAccelerator() { Key = key };
            if (modifiers.HasValue)
            {
                keyboardAccelerator.Modifiers = modifiers.Value;
            }

            keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;
            return keyboardAccelerator;
        }

        private KeyboardAccelerator BuildOpenAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
        {
            var keyboardAccelerator = new KeyboardAccelerator() { Key = key };
            if (modifiers.HasValue)
            {
                keyboardAccelerator.Modifiers = modifiers.Value;
            }

            keyboardAccelerator.Invoked += OpenGameKeyBoardAcceleratedInvoked;
            return keyboardAccelerator;
        }

        private void OpenGameKeyBoardAcceleratedInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            LoadGameWorld();
            args.Handled = true;
        }

        private void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var result = _navigationService.GoBack();
            args.Handled = result;
        }
    }
}
