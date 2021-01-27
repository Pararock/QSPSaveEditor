using Collections.Generic;
using QSPEditor.Helpers;
using QSPEditor.Models;
using QSPEditor.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;

namespace QSPEditor.ViewModels
{
    public class HomeViewModel : Observable
    {
        private readonly IRecentFilesService _recentFilesService;
        private readonly IMessageService _messageServices;
        private readonly IWindowManagerService _windowManagerService;

        private RelayCommand<string> _deleteRecentGameCommand;

        public ICommand DeleteRecentGameCommand => _deleteRecentGameCommand ?? (_deleteRecentGameCommand = new RelayCommand<string>(DeleteRecentGame));

        private ObservableCollection<QSPGame> recentGame;

        public HomeViewModel(IRecentFilesService recentFileService, IMessageService messageService, IWindowManagerService windowManagerService)
        {
            _recentFilesService = recentFileService;
            _messageServices = messageService;
            _windowManagerService = windowManagerService;
            UpdateRecentGamesAsync();
        }

        public void Subscribe()
        {
            _messageServices.Subscribe<ShellViewModel, string>(this, OnMessage);
        }
        public void Unsubscribe()
        {
            _messageServices.Unsubscribe(this);
        }

        private void OnMessage(ShellViewModel sender, string message, string args)
        {
            if(message == "GameLoaded")
            {
                UpdateRecentGamesAsync();
            }
        }

        private async void DeleteRecentGame(string parameter)
        {
            await _recentFilesService.DeleteQSP(parameter);
        }

        private async Task UpdateRecentGamesAsync()
        {
            await _windowManagerService.MainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                RecentGame = new ObservableCollection<QSPGame>(_recentFilesService.GameFiles.Values.ToList());
            });
            
        }

        public ObservableCollection<QSPGame> RecentGame { get => recentGame; set => recentGame = value; }
    }
}
