﻿namespace QSPSaveEditor.ViewModel
{
    using CefSharp;
    using CefSharp.Wpf;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using HtmlAgilityPack;
    using MahApps.Metro.Controls.Dialogs;
    using Message;
    using Microsoft.Win32;
    using Model;
    using QSPNETWrapper;
    using QSPNETWrapper.Model;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.Remoting.Messaging;
    using System.Windows;
    using System.Linq;

    using System.Windows.Data;
    using System.Xml;
    using System.Xml.Linq;
    using System.Text.RegularExpressions;
    using System.Diagnostics;
    using System.Text;


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

        private string baseURL;

        private IWpfWebBrowser webBrowser;

        public bool IsAwesomium;

        private HtmlDocument htmlDoc;

        public IWpfWebBrowser WebBrowser
        {

            get { return webBrowser; }

            set
            {
                Set(nameof(WebBrowser), ref webBrowser, value);
            }

        }




        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel( IQSPGameDataService gameDataService, IDialogCoordinator dialogCoordinator )
        {
            this.gameDataService = gameDataService;
            this.dialogCoordinator = dialogCoordinator;
            _QSPGame = this.gameDataService.Game;
            _QSPGame.PropertyChanged += _QSPGame_PropertyChanged;
            htmlDoc = new HtmlDocument();

            if ( IsInDesignMode )
            {
                this.gameDataService.LoadSaveAsync("");
            }

        }

        private void _QSPGame_PropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            RaisePropertyChanged(e.PropertyName);
            if ( e.PropertyName == nameof(MainDescription) )
            {
                if ( MainDescription != null )
                {
                    var mainDescriptionHTML = new HtmlDocument();
                    using ( var stream = new MemoryStream() )
                    {
                        using ( var writer = new StreamWriter(stream) )
                        {
                            writer.Write(MainDescription);
                            writer.Flush();
                            stream.Position = 0;
                            mainDescriptionHTML.Load(stream, Encoding.UTF8);

                        }
                    }

                    // clean up href link
                    var nodesCollection = mainDescriptionHTML.DocumentNode.SelectNodes(".//a");
                    if ( nodesCollection != null )
                    {
                        foreach ( var node in nodesCollection )
                        {
                            node.Attributes["href"].Value = System.Web.HttpUtility.HtmlEncode(node.Attributes["href"].Value);
                            node.Attributes.Add("title", node.Attributes["href"].Value);
                        }
                    }

                    if ( IsAwesomium )
                    {
                        foreach ( var node in mainDescriptionHTML.DocumentNode.SelectNodes("./div") )
                        {
                            var nodetoReplace = htmlDoc.DocumentNode.SelectSingleNode($"//div[contains(@id,{node.Id})]");
                            nodetoReplace.InnerHtml = node.InnerHtml;
                        }

                        webBrowser.LoadHtml(htmlDoc.DocumentNode.InnerHtml, baseURL);
                    }
                    else
                    {
                        webBrowser.LoadHtml(mainDescriptionHTML.DocumentNode.InnerHtml, baseURL);
                    }
                }
            }
        }

        public DateTime CompiledTime => _QSPGame.CompiledDate;
        public int FullRefreshCount => _QSPGame.FullRefreshCount;
        public string CurrentLocation => _QSPGame.CurrentLocation;

        public RelayCommand RestartGameCommand => restartGameCommand ?? new RelayCommand(() => _QSPGame.RestartWorld(true));

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

                var error = await gameDataService.OpenGameAsync(filename);

                if ( error != null )
                {
                    await dialogCoordinator.ShowMessageAsync(this, "Error", error.Message);
                }
                else
                {
                    if ( IsSaveLoaded )
                    {
                        IsSaveLoaded = false;
                        MessengerInstance.Send(new SaveMessage(SaveMessageType.SaveClosed));
                    }
                    IsGameOpen = true;
                    baseURL = Path.GetDirectoryName(filename) + Path.DirectorySeparatorChar; //we always want a trailing separator for the webbrowser
                    qspGamePath = filename;
                    DetectEngineType();
                }

                await controller.CloseAsync();
            }
        }

        private void DetectEngineType()
        {
            // is gameAwesomium.html present in the same folder as the qsp?
            var awesomiumFile = Path.Combine(baseURL, "gameAwesomium.html");
            if ( File.Exists(awesomiumFile) )
            {
                IsAwesomium = true;
                htmlDoc.Load(awesomiumFile);
            }
            else
            {
                IsAwesomium = false;
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

                var error = await gameDataService.LoadSaveAsync(saveToLoad);

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

            var error = await gameDataService.WriteSaveGameAsync(qspSavegamePath);

            if ( error != null )
            {
                await dialogCoordinator.ShowMessageAsync(this, "Error", error.Message);
            }

            await controller.CloseAsync();
        }

    }
}