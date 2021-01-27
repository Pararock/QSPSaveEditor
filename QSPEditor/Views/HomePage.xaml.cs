using QSPEditor.Configuration;
using QSPEditor.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

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

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Subscribe();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.Unsubscribe();
        }

        private void MenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem selectedItem)
            {
                string sortOption = selectedItem.Tag.ToString();
                switch (sortOption)
                {
                    case "name":
                        //SortByRating();
                        break;
                    case "opened":
                        //SortByMatch();
                        break;
                }
                //Control1Output.Text = "Sort by: " + sortOption;
            }
        }
    }
}
