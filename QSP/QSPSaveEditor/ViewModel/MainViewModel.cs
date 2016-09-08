namespace QSPSaveEditor.ViewModel
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using MahApps.Metro.Controls.Dialogs;
    using Microsoft.Win32;
    using Model;
    using QSPNETWrapper;
    using QSPNETWrapper.Model;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows.Data;


    /// <summary>
    /// Main view model for a QSP Game
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IQSPGameDataService _dataService;
        private QSPGame _QSPGame;
        private readonly IDialogCoordinator _dialogCoordinator;
        private ICollectionView variablesView;

        public Version Version => _QSPGame.Version;
        public DateTime CompiledTime => _QSPGame.CompiledDate;
        public string QSPPath => _QSPGame.QSPFilePath;
        //public BindingList<QSPVariable> VariableList => _QSPGame.VariablesList;
        public int MaxVariablesCount => _QSPGame.MaxVariablesCount;
        public int FullRefreshCount => _QSPGame.FullRefreshCount;
        public int ActionsCount => _QSPGame.ActionsCount;
        public int ObjectsCount => _QSPGame.ObjectsCount;

        private RelayCommand openGameCommand;
        private RelayCommand openSaveCommand;


        public ICollectionView VariablesView
        {
            get
            {
                return variablesView;
            }
            set
            {
                Set(nameof(VariablesView), ref variablesView, value);
            }
        }

        public RelayCommand OpenGameCommand
        {
            get
            {
                return openGameCommand ?? (openGameCommand = new RelayCommand(() =>
                {
                    OpenGame();
                },
                () =>
                {
                    return true;
                }));
            }
        }

        public RelayCommand OpenSaveCommand => openSaveCommand ?? (openSaveCommand = new RelayCommand(() => OpenSave()));


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel( IQSPGameDataService dataService, IDialogCoordinator idialog )
        {
            _dataService = dataService;
            _dialogCoordinator = idialog;
            _QSPGame = _dataService.Game;
           
            if(IsInDesignMode)
            {
                _dataService.LoadSaveAsync("");
                VariablesView = CollectionViewSource.GetDefaultView(_QSPGame.VariablesList);
            }
        }

        private async void OpenSave()
        {
            //TODO Check if dirty and ask for confirmation
            var fileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                DefaultExt = "*.save",
                Title = "Open a QSP Game save",
                Filter = "QSP save|*.sav"
            };

            // Show open file dialog box
            var result = fileDialog.ShowDialog();

            // Process open file dialog box results
            if ( result == true )
            {
                // Open document
                var filename = fileDialog.FileName;

                var controller = await _dialogCoordinator.ShowProgressAsync(this, "Loading save game", "Please wait");
                controller.SetIndeterminate();

                var error = await _dataService.LoadSaveAsync(filename);

                VariablesView = CollectionViewSource.GetDefaultView(_QSPGame.VariablesList);
                
                if(error != null)
                {
                    await _dialogCoordinator.ShowMessageAsync(this, "Error", error.Message);
                }

                await controller.CloseAsync();
            }
        }


        private async void OpenGame()
        {
            //TODO Check if dirty and ask for confirmation
            var fileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                DefaultExt = "*.qsp",
                Title = "Open a QSP Game",
                Filter = "QSP Games|*.qsp; *.gam"
            };

            // Show open file dialog box
            var result = fileDialog.ShowDialog();

            // Process open file dialog box results
            if ( result == true )
            {
                // Open document
                var filename = fileDialog.FileName;

                var controller = await _dialogCoordinator.ShowProgressAsync(this, "Loading game", "Please wait");
                controller.SetIndeterminate();

                var error = await _dataService.OpenGameAsync(filename);

                if ( error != null )
                {
                    await _dialogCoordinator.ShowMessageAsync(this, "Error", error.Message);
                }

                await controller.CloseAsync();
            }
        }

    }
}