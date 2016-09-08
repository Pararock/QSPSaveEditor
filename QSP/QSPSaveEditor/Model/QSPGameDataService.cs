namespace QSPSaveEditor.Model
{
    using QSPNETWrapper;
    using System;
    using System.Threading.Tasks;

    class QSPGameDataService : IQSPGameDataService
    {
        private QSPGameWorld _game;

        public QSPGameDataService()
        {
            _game = new QSPGameWorld();
        }


        public QSPGame Game => _game;

        public Task<Exception> LoadSaveAsync( string savepath )
        {
            return LoadSaveInternalAsync(savepath);
        }

        public Task<Exception> OpenGameAsync( string gamePath )
        {
            return OpenGameInternalAsync(gamePath);
        }

        private async Task<Exception> LoadSaveInternalAsync( string savepath )
        {
            var result = await Task.Run(() => _game.OpenSavedGame(savepath, true));
            if ( !result )
            {
                return QSPGameWorld.GetLastError();
            }
            else
            {
                return null;
            }
        }

        private async Task<Exception> OpenGameInternalAsync( string gamePath )
        {
            var result = await Task.Run(() => _game.LoadGameWorld(gamePath));
            if ( !result )
            {
                return QSPGameWorld.GetLastError();
            }
            else
            {
                return null;
            }
        }


    }
}
