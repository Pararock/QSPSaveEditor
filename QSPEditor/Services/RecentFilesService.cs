using Newtonsoft.Json;
using QSPEditor.Helpers;
using QSPEditor.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace QSPEditor.Services
{
    public interface IRecentFilesService
    {
        Task AddQSP(FilePickerResult file, int gameCrc);
        Task DeleteQSP(string key);
        Task AddSave(FilePickerResult file);
        Task InitializeAsync();
        Dictionary<string, QSPGame> GameFiles { get; }
    }

    public class RecentFilesService : IRecentFilesService
    {
        private const string RecentGamesKey = "RecentGames:v2";
        private const string RecentSaveKey = "RecentSaves";

        private const string legacyRecentGamesKey = "RecentGames";

        public Dictionary<string, QSPGame> GameFiles { get; set; }
        //public static Dictionary<string,string> Files { get; set; }

        private static StorageItemMostRecentlyUsedList _mru;

        public async Task InitializeAsync()
        {
            _mru = StorageApplicationPermissions.MostRecentlyUsedList;
            ApplicationData.Current.LocalSettings.Values.Remove(legacyRecentGamesKey);
            GameFiles = await LoadRecentGamesFromSettingsAsync();
        }

        public async Task AddQSP(FilePickerResult file, int gameCrc)
        {
            var alreadyExist = false;

            if(GameFiles.TryGetValue(file.File.FolderRelativeId, out var qspGame))
            {
                _mru.AddOrReplace(qspGame.MruToken, qspGame.StorageItem, "QSPGame", RecentStorageItemVisibility.AppOnly);
                alreadyExist = true;
                qspGame.LastOpened = DateTimeOffset.UtcNow;

                if (qspGame.GameCrc != gameCrc)
                {
                    qspGame.GameCrc = gameCrc;
                }

            }

            if (!alreadyExist)
            {
                if (_mru.Entries.Count < _mru.MaximumItemsAllowed)
                {
                    var token =  _mru.Add(file.File, "QSPGame", RecentStorageItemVisibility.AppOnly);
                    var newGame = new QSPGame(file, gameCrc);
                    newGame.MruToken = token;
                    GameFiles.Add(newGame.FolderRelativeId, newGame);
                }
                else
                {
                    // todo: remove save before qsp
                }
            }

            await SaveRecentGamesFromSettingsAsync(GameFiles.Values);
        }

        public Task DeleteQSP(string key)
        {
            if (GameFiles.TryGetValue(key, out var qspGame))
            {
                _mru.Remove(qspGame.MruToken);
                GameFiles.Remove(key);
            }
            return Task.CompletedTask;
        }

        public async Task AddSave(FilePickerResult file)
        {

        }

        private static async Task<Dictionary<string, QSPGame>> LoadRecentGamesFromSettingsAsync()
        {
            var recentFiles = new Dictionary<string, QSPGame>((int)_mru.MaximumItemsAllowed);
            List<QSPGame> tokenList = null;
            try
            {
                tokenList = await ApplicationData.Current.LocalSettings.ReadAsync<List<QSPGame>>(RecentGamesKey);
            }
            catch(JsonException e)
            {
                Console.WriteLine(e);
            }
            

            if (tokenList != null)
            {
                foreach (var token in tokenList)
                {
                    try
                    {
                        var recentGame = await _mru.GetFileAsync(token.MruToken);
                        if (recentGame != null)
                        {
                            if (_mru.CheckAccess(recentGame) && recentGame.IsAvailable)
                            {
                                token.StorageItem = recentGame;
                                recentFiles.Add(token.FolderRelativeId, token);
                            }
                            else
                            {
                                _mru.Remove(token.FolderRelativeId);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }

            return recentFiles;
        }

        private static async Task SaveRecentGamesFromSettingsAsync(IEnumerable<QSPGame> games)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(RecentGamesKey, games);
        }
    }
}
