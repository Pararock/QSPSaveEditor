using QSPEditor.Configuration;
using QSPEditor.ViewModels;

using Windows.UI.Xaml.Controls;

namespace QSPEditor.Views
{
    public sealed partial class HomePage : Page
    {
        private readonly HomeViewModel _homeViewModel;
        private HomeViewModel ViewModel
        {
            get { return _homeViewModel; }
        }

        public HomePage()
        {
            _homeViewModel = ServiceLocator.Current.GetService<HomeViewModel>();
            InitializeComponent();
        }
    }
}
