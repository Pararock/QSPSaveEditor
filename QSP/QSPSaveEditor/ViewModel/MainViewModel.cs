namespace QSPSaveEditor.ViewModel
{
    using GalaSoft.MvvmLight;
    using Model;
    using QSPNETWrapper;
    using QSPNETWrapper.Model;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IQSPGameDataService _dataService;
        private QSPGame _QSPGame;

        public Version Version => _QSPGame.Version;
        public DateTime CompiledTime => _QSPGame.CompiledDate;
        public string QSPPath => _QSPGame.QSPFilePath;
        public IEnumerable<QSPVariable> VariableList => _QSPGame.VariablesList;
        public int MaxVariablesCount => _QSPGame.MaxVariablesCount;
        public int FullRefreshCount => _QSPGame.FullRefreshCount;
        public int ActionsCount => _QSPGame.ActionsCount;
        public int ObjectsCount => _QSPGame.ObjectsCount;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel( IQSPGameDataService dataService )
        {
            _dataService = dataService;
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