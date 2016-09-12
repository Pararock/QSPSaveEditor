namespace QSPSaveEditor.ViewModel
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using MahApps.Metro.Controls.Dialogs;
    using Message;
    using Microsoft.Win32;
    using Model;
    using QSPNETWrapper;
    using QSPNETWrapper.Model;
    using System;
    using System.ComponentModel;
    using System.Runtime.Remoting.Messaging;
    using System.Windows.Data;


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

        private string qspGamePath;
        private string qspSavegamePath;

        private RelayCommand writeSavegameCommand;

        private RelayCommand showMainDesc;
        private RelayCommand showVarsDesc;

        private RelayCommand execStringCommand;


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
                this.gameDataService.LoadSaveAsync("");
            }
        }

        private void _QSPGame_PropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            RaisePropertyChanged(e.PropertyName);
        }

        public int ActionsCount => _QSPGame.ActionsCount;

        public DateTime CompiledTime => _QSPGame.CompiledDate;
        public int FullRefreshCount => _QSPGame.FullRefreshCount;
        public string CurrentLocation => _QSPGame.CurrentLocation;

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
                    return true;
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
                    if(_QSPGame.ExecCommand(execString))
                    {

                    }else
                    {
                        await dialogCoordinator.ShowMessageAsync(this, "Error", "Error2");
                    }
                },
                () =>
                {
                    return true;
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
                    return true;
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
        public int ObjectsCount => _QSPGame.ObjectsCount;

        public RelayCommand OpenGameCommand
        {
            get
            {
                return openGameCommand ?? (openGameCommand = new RelayCommand(() =>
                {
                    OpenGameAsync();
                },
                () =>
                {
                    return true;
                }));
            }
        }

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

                var error = await gameDataService.OpenGameAsync(filename);

                if ( error != null )
                {
                    await dialogCoordinator.ShowMessageAsync(this, "Error", error.Message);
                }
                else
                {
                    IsGameOpen = true;
                    qspGamePath = filename;
                }

                await controller.CloseAsync();
            }
        }

        private async void OpenSaveAsync()
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

                var controller = await dialogCoordinator.ShowProgressAsync(this, "Loading save game", "Please wait");
                controller.SetIndeterminate();

                var error = await gameDataService.LoadSaveAsync(filename);

                if ( error != null )
                {
                    await dialogCoordinator.ShowMessageAsync(this, "Error", error.Message);
                }
                else
                {
                    IsSaveLoaded = true;
                    qspSavegamePath = filename;
                    MessengerInstance.Send(new LoadingSaveSuccessMessage());
                }

                await controller.CloseAsync();
            }
        }

        private async void WriteSavegameAsync()
        {
            var controller = await dialogCoordinator.ShowProgressAsync(this, "Writing save game", "Please wait");
            controller.SetIndeterminate();

            var error = await gameDataService.WriteSaveGameAsync(qspSavegamePath);

            if ( error != null )
            {
                await dialogCoordinator.ShowMessageAsync(this, "Error", error.Message);
            }

            await controller.CloseAsync();
        }

    }
}