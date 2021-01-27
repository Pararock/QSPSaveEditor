using Newtonsoft.Json;
using QSPEditor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace QSPEditor.Models
{
    public class QSPGame
    {
        [JsonIgnore]
        public IStorageItem StorageItem;

        public int GameCrc;
        public string FolderRelativeId;

        public string MruToken;
        public DateTimeOffset LastOpened;
        public QSPGame()
        {

        }
        public QSPGame(int gameCrc, string gamePath, string mruToken)
        {
            GameCrc = gameCrc;
            FolderRelativeId = gamePath;
            MruToken = mruToken;
        }
        public QSPGame(FilePickerResult result, int gameCrc)
        {
            GameCrc = gameCrc;
            FolderRelativeId = result.File.FolderRelativeId;
            StorageItem = result.File;
            LastOpened = DateTimeOffset.UtcNow;
        }
    }
}
