using QSPEditor.Configuration;
using QSPEditor.ViewModels;

using Windows.UI.Xaml.Controls;

namespace QSPEditor.Views
{
    // TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : Page
    {
        private readonly ShellViewModel _shellViewModel;
        private ShellViewModel ViewModel
        {
            get { return _shellViewModel; }
        }

        public ShellPage()
        {
            _shellViewModel = ServiceLocator.Current.GetService<ShellViewModel>();
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, navigationView, KeyboardAccelerators);
        }
    }
}
