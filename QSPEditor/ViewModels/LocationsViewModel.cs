using QSPEditor.Helpers;
using QSPEditor.Services;
using QSPEditor.Views;
using QSPLib_CppWinrt;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation.Collections;

namespace QSPEditor.ViewModels
{
    public class LocationsViewModel : Observable
    {
        private Location _selected;
        private string _locationTextFilter;
        private readonly IWindowManagerService _windowsManagerService;
        private readonly Engine _engine;
        private bool _isLoading;

        private ViewLifetimeControl ViewLifetimeControl;

        public Location Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public string LocationTextFilter
        {
            get { return _locationTextFilter; }
            set { Set(ref _locationTextFilter, value); }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(ref _isLoading, value); }
        }

        private ObservableCollection<Location> _filteredCollection;

        private RelayCommand _openInNewWindow;

        public ICommand OpenInNewWindowCommand => _openInNewWindow ?? (_openInNewWindow = new RelayCommand(OpenInNewWindow, CanOpenInNewWindow));

        public ObservableCollection<Location> Locations => _filteredCollection;

        public LocationsViewModel(IEngineService engineService, IWindowManagerService windowManagerService)
        {
            _windowsManagerService = windowManagerService;
            IsLoading = true;
            _engine = engineService.Engine;
            _filteredCollection = new ObservableCollection<Location>(_engine.Locations);
            this.PropertyChanged += LocationsViewModel_PropertyChanged;
            _engine.PropertyChanged += _engine_PropertyChanged;
            IsLoading = false;
        }
        private void LocationsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LocationTextFilter")
            {
                //IsLoading = true;
                //ModifyFilter();
                //IsLoading = false;
            }
        }

        private async void _engine_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName== "Locations")
            {
                //IsLoading = true;
                _filteredCollection = new ObservableCollection<Location>(_engine.Locations);
                //IsLoading = false;
            }

        }

        private bool Filter(Location contact)
        {
            return contact.Name.Contains(LocationTextFilter, StringComparison.InvariantCultureIgnoreCase);
        }

        internal void ModifyFilter(string text, Action<IEnumerable<Location>> newFilteredSource)
        {
            LocationTextFilter = text;
            var filtered = _engine.Locations.Where(location => Filter(location));
            Thread.Sleep(2000);
            newFilteredSource(filtered);
            //Remove_NonMatching(filtered);
            //AddBack_Locations(filtered);
        }

        private async void OpenInNewWindow()
        {
            ViewLifetimeControl = await _windowsManagerService.TryShowAsViewModeAsync("stuffs", typeof(LocationEditorControl), Windows.UI.ViewManagement.ApplicationViewMode.Default);
        }

        private bool CanOpenInNewWindow()
        {

            return true;
        }


        //public async Task LoadDataAsync(MasterDetailsViewState viewState)
        //{
        //    SampleItems.Clear();

        //    var data = await SampleDataService.GetMasterDetailDataAsync();

        //    foreach (var item in data)
        //    {
        //        SampleItems.Add(item);
        //    }

        //    if (viewState == MasterDetailsViewState.Both)
        //    {
        //        Selected = SampleItems.First();
        //    }
        //}
    }
}
