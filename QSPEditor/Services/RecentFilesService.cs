using QSPEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace QSPEditor.Services
{
    public interface IRecentFilesService
    {
        Task AddQSP(FilePickerResult file);
        Task AddSave(FilePickerResult file);
        Task InitializeAsync();
        Dictionary<string, IStorageItem> GameFiles { get; }
    }

    public class RecentFilesService : IRecentFilesService
    {
        private const string RecentGamesKey = "RecentGames";
        private const string RecentSaveKey = "RecentSaves";


        public Dictionary<string, IStorageItem> GameFiles { get; private set; }
        //public static Dictionary<string,string> Files { get; set; }

        private static StorageItemMostRecentlyUsedList _mru;

        public async Task InitializeAsync()
        {
            _mru = StorageApplicationPermissions.MostRecentlyUsedList;
            GameFiles = await LoadRecentGamesFromSettingsAsync();
        }

        public async Task AddQSP(FilePickerResult file)
        {
            var alreadyExist = false;
            foreach (var item in GameFiles)
            {
                if (file.File.IsEqual(item.Value))
                {
                    // todo put the crc here instead
                    _mru.AddOrReplace(item.Key, item.Value, "QSPGame", RecentStorageItemVisibility.AppOnly);
                    alreadyExist = true;
                }
            }

            if (!alreadyExist)
            {
                if (_mru.Entries.Count < _mru.MaximumItemsAllowed)
                {
                    var token = _mru.Add(file.File, "QSPGame", RecentStorageItemVisibility.AppOnly);
                    GameFiles.Add(token, file.File);
                }
                else
                {
                    // todo: remove save before qsp
                }
            }

            //await SetRequestedThemeAsync();
            await SaveRecentGamesFromSettingsAsync(GameFiles.Keys);
        }

        public async Task AddSave(FilePickerResult file)
        {

        }

        private static async Task<Dictionary<string, IStorageItem>> LoadRecentGamesFromSettingsAsync()
        {
            var recentFiles = new Dictionary<string, IStorageItem>((int)_mru.MaximumItemsAllowed);
            var tokenList = await ApplicationData.Current.LocalSettings.ReadAsync<List<string>>(RecentGamesKey);

            if (tokenList != null)
            {
                foreach (var token in tokenList)
                {
                    try
                    {
                        var recentGame = await _mru.GetFileAsync(token);
                        if (recentGame != null)
                        {
                            if (_mru.CheckAccess(recentGame) && recentGame.IsAvailable)
                            {
                                recentFiles.Add(token, recentGame);
                            }
                            else
                            {
                                _mru.Remove(token);
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

        private static async Task SaveRecentGamesFromSettingsAsync(IEnumerable<string> games)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(RecentGamesKey, games);
        }
    }
}
