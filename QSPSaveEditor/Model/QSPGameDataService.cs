namespace QSPSaveEditor.Model
{
    using QSPNETWrapper;
    using QSPNETWrapper.Model;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows;

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

        public Task<Exception> WriteSaveGameAsync( string gamePath )
        {
            return WriteSaveInternalAsync(gamePath);
        }

        private async Task<Exception> WriteSaveInternalAsync( string savepath )
        {
            await Task.Factory.StartNew(() => _game.ModifyVariables());

            var result = await Task.Factory.StartNew(() => _game.WriteSaveGame(savepath,true));
            return !result ? QSPGameWorld.GetLastError() : null;
        }

        private async Task<Exception> LoadSaveInternalAsync( string savepath )
        {
            var result = await Task.Factory.StartNew(() => _game.OpenSavedGame(savepath, true));

            return !result ? QSPGameWorld.GetLastError() : null;
        }

        private async Task<Exception> OpenGameInternalAsync( string gamePath )
        {
            var result = await Task.Factory.StartNew(() => _game.LoadGameWorld(gamePath));
            return !result ? QSPGameWorld.GetLastError() : null;
        }


    }
}
