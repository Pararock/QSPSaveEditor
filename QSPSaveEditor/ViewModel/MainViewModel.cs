﻿namespace QSPSaveEditor.ViewModel
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using MahApps.Metro.Controls.Dialogs;
    using Message;
    using Microsoft.Win32;
    using Model;
    using QSPNETWrapper;
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;


    /// <summary>
    /// Main view model for a QSP Game
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public QSPGame _QSPGame;

        private readonly IQSPGameDataService gameDataService;
        private readonly IDialogCoordinator dialogCoordinator;

        private bool isGameOpen;
        private bool isSaveLoaded;

        private RelayCommand openGameCommand;
        private RelayCommand openSaveCommand;
        private RelayCommand reloadSaveCommand;

        private string qspGamePath;
        private string qspSavegamePath;

        private RelayCommand writeSavegameCommand;

        private RelayCommand showMainDesc;
        private RelayCommand showVarsDesc;

        private RelayCommand execStringCommand;

        private RelayCommand restartGameCommand;

        public string MainDescription => _QSPGame.MainDescription;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel( IQSPGameDataService gameDataService, IDialogCoordinator dialogCoordinator )
        {
            this.gameDataService = gameDataService;
            this.dialogCoordinator = dialogCoordinator;
            _QSPGame = this.gameDataService.Game;
            _QSPGame.PropertyChanged += _QSPGame_PropertyChanged;

            if ( IsInDesignMode )
            {
                this.gameDataService.LoadSave("");
            }

        }

        private void _QSPGame_PropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            RaisePropertyChanged(e.PropertyName);
        }

        public DateTime CompiledTime => _QSPGame.CompiledDate;
        public int FullRefreshCount => _QSPGame.FullRefreshCount;
        public string CurrentLocation => _QSPGame.CurrentLocation;

        public RelayCommand RestartGameCommand => restartGameCommand ?? new RelayCommand(() => _QSPGame.RestartWorld(true),() => false);

        public RelayCommand ShowMainDesc
        {
            get
            {
                return showMainDesc ?? (showMainDesc = new RelayCommand(() =>
                {
                    dialogCoordinator.ShowMessageAsync(this, "MainDesc", _QSPGame.MainDescription);
                },
                () =>
                {
                    return IsGameOpen;
                }));
            }
        }

        public RelayCommand ExecStringCommand
        {
            get
            {
                return execStringCommand ?? (execStringCommand = new RelayCommand(async () =>
                {
                    var execString = await dialogCoordinator.ShowInputAsync(this, "Exec string", "Please enter the command to execute");

                    if ( !string.IsNullOrEmpty(execString) )
                    {
                        if ( _QSPGame.ExecCommand(execString) )
                        {

                        }
                        else
                        {
                            await dialogCoordinator.ShowMessageAsync(this, "Error", "Error2");
                        }
                    }
                },
                () =>
                {
                    return IsGameOpen;
                }));
            }
        }

        public RelayCommand ShowVarsDesc
        {
            get
            {
                return showVarsDesc ?? (showVarsDesc = new RelayCommand(() =>
                {
                    dialogCoordinator.ShowMessageAsync(this, "VarsDesc", _QSPGame.VarsDescription);
                },
                () =>
                {
                    return IsGameOpen;
                }));
            }
        }


        public bool IsGameOpen
        {
            get
            {
                return isGameOpen;
            }
            set
            {
                Set(nameof(IsGameOpen), ref isGameOpen, value);
            }
        }

        public bool IsSaveLoaded
        {
            get
            {
                return isSaveLoaded;
            }
            set
            {
                Set(nameof(IsSaveLoaded), ref isSaveLoaded, value);
            }
        }

        public int MaxVariablesCount => _QSPGame.MaxVariablesCount;

        public RelayCommand OpenGameCommand => openGameCommand ?? new RelayCommand(() => OpenGameAsync());

        public RelayCommand OpenSaveCommand
        {
            get
            {
                return openSaveCommand ?? (openSaveCommand = new RelayCommand(() =>
                {
                    OpenSaveAsync();
                },
                () =>
                {
                    return IsGameOpen;
                }));
            }
        }

        public RelayCommand ReloadSaveCommand
        {
            get
            {
                return reloadSaveCommand ?? (reloadSaveCommand = new RelayCommand(() =>
                {
                    OpenSaveAsync(true);
                },
                () =>
                {
                    return isSaveLoaded;
                }));
            }
        }


        public string QSPPath => _QSPGame.QSPFilePath;

        public Version Version => _QSPGame.Version;

        public RelayCommand WriteSaveCommand
        {
            get
            {
                return writeSavegameCommand ?? (writeSavegameCommand = new RelayCommand(() =>
                {
                    WriteSavegameAsync();
                },
                () =>
                {
                    return isSaveLoaded;
                }));
            }

        }


        private async void OpenGameAsync()
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

                var controller = await dialogCoordinator.ShowProgressAsync(this, "Loading game", "Please wait");
                controller.SetIndeterminate();

                var openGameResult = await Task.Run(() => gameDataService.OpenGame(filename));

                if ( openGameResult != null )
                {
                    await dialogCoordinator.ShowMessageAsync(this, "Error", openGameResult.Message);
                }
                else
                {
                    if ( IsSaveLoaded )
                    {
                        IsSaveLoaded = false;
                        MessengerInstance.Send(new SaveMessage(SaveMessageType.SaveClosed));
                    }
                    IsGameOpen = true;
                    qspGamePath = filename;
                }

                await controller.CloseAsync();
            }
        }

        private async void OpenSaveAsync( bool reloadSave = false )
        {
            //TODO Check if dirty and ask for confirmation
            var saveToLoad = string.Empty;
            bool? result = false;
            if ( !reloadSave )
            {
                var fileDialog = new OpenFileDialog
                {
                    CheckFileExists = true,
                    DefaultExt = "*.save",
                    Title = "Open a QSP Game save",
                    Filter = "QSP save|*.sav"
                };

                // Show open file dialog box
                result = fileDialog.ShowDialog();
                saveToLoad = fileDialog.FileName;
            }
            else
            {
                saveToLoad = qspSavegamePath;
                result = true;
            }

            // Process open file dialog box results
            if ( result == true )
            {
                // Open document

                var controller = await dialogCoordinator.ShowProgressAsync(this, "Loading save game", "Please wait");
                controller.SetIndeterminate();

                var error = await Task.Run( () => gameDataService.LoadSave(saveToLoad));

                if ( error != null )
                {
                    await dialogCoordinator.ShowMessageAsync(this, "Error", error.Message);
                }
                else
                {
                    IsSaveLoaded = true;
                    qspSavegamePath = saveToLoad;
                    MessengerInstance.Send(new SaveMessage(SaveMessageType.SaveLoaded));
                }

                await controller.CloseAsync();
            }
        }

        private async void WriteSavegameAsync()
        {
            var controller = await dialogCoordinator.ShowProgressAsync(this, "Writing save game", "Please wait");
            controller.SetIndeterminate();

            var error = await Task.Run(() => gameDataService.WriteSaveGame(qspSavegamePath));

            if ( error != null )
            {
                await dialogCoordinator.ShowMessageAsync(this, "Error", error.Message);
            }

            await controller.CloseAsync();
        }

    }
}