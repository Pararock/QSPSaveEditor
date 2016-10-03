using QSPEditor.Configuration;
using QSPEditor.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace QSPEditor.Views
{
    // TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere
    public sealed partial class SettingsPage : Page
    {
        private readonly SettingsViewModel _settingsViewModel;
        private SettingsViewModel ViewModel
        {
            get { return _settingsViewModel; }
        }

        public SettingsPage()
        {
            _settingsViewModel = ServiceLocator.Current.GetService<SettingsViewModel>();
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.InitializeAsync();
        }
    }
}
