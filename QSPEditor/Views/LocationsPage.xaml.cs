using QSPEditor.Configuration;
using QSPEditor.ViewModels;
using QSPLib_CppWinrt;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace QSPEditor.Views
{
    public sealed partial class LocationsPage : Page
    {
        private readonly LocationsViewModel _locationsViewModel;
        private LocationsViewModel ViewModel
        {
            get { return _locationsViewModel; }
        }

        public LocationsPage()
        {
            _locationsViewModel = ServiceLocator.Current.GetService<LocationsViewModel>();
            InitializeComponent();
            Loaded += LocationsPage_Loaded;
        }
        private async void OnFilterChanged(object sender, TextChangedEventArgs args)
        {
            if (sender is TextBox filterTextBox)
            {
                _locationsViewModel.IsLoading = true;
                _locationsViewModel.ModifyFilter(filterTextBox.Text, this.NewFilteredSource);
            }
        }

        private void NewFilteredSource(IEnumerable<Location> enumerable)
        {
            Remove_NonMatching(enumerable);
            AddBack_Locations(enumerable);
        }
        private void Remove_NonMatching(IEnumerable<Location> filteredData)
        {
            for (int i = _locationsViewModel.Locations.Count - 1; i >= 0; i--)
            {
                var item = _locationsViewModel.Locations[i];

                if (!filteredData.Contains(item))
                {
                    _locationsViewModel.Locations.Remove(item);
                }
            }
        }

        private void AddBack_Locations(IEnumerable<Location> filteredData)
        {
            foreach (var item in filteredData)
            {
                if (!_locationsViewModel.Locations.Contains(item))
                {
                    _locationsViewModel.Locations.Add(item);
                }
            }
        }


        private async void LocationsPage_Loaded(object sender, RoutedEventArgs e)
        {
            //await ViewModel.LoadDataAsync(MasterDetailsViewControl.ViewState);
        }
    }
}
