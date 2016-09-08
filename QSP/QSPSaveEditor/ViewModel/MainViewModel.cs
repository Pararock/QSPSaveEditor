namespace QSPSaveEditor.ViewModel
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using MahApps.Metro.Controls.Dialogs;
    using Model;
    using QSPNETWrapper;
    using QSPNETWrapper.Model;
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// Main view model for a QSP Game
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IQSPGameDataService _dataService;
        private QSPGame _QSPGame;
        private IDialogCoordinator _dialogCoordinator;

        public Version Version => _QSPGame.Version;
        public DateTime CompiledTime => _QSPGame.CompiledDate;
        public string QSPPath => _QSPGame.QSPFilePath;
        public IEnumerable<QSPVariable> VariableList => _QSPGame.VariablesList;
        public int MaxVariablesCount => _QSPGame.MaxVariablesCount;
        public int FullRefreshCount => _QSPGame.FullRefreshCount;
        public int ActionsCount => _QSPGame.ActionsCount;
        public int ObjectsCount => _QSPGame.ObjectsCount;

        private RelayCommand textBoxButtonCmd;


        public RelayCommand TestCommand
        {
            get
            {
                return textBoxButtonCmd ?? (textBoxButtonCmd = new RelayCommand(() =>
                {

                    _dialogCoordinator.ShowMessageAsync(this, "Message from VM", "MVVM based dialogs!");
                },
                () =>
                {
                    return true;
                }));
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel( IQSPGameDataService dataService, IDialogCoordinator idialog )
        {
            _dataService = dataService;
            _dialogCoordinator = idialog;

            

            _dataService.OpenGame(
                ( item, error ) =>
                {
                    if ( error != null )
                    {
                        // Report error here
                        return;
                    }

                    _QSPGame = item;
                }, @"C:\temp\world.qsp");

            dataService.LoadSave(
                (error) =>
                {
                    //_variableList = _QSPGame.
                }, @"C:\temp\save.sav"
                );
        }
    }
}