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

        public Exception LoadSave( string savepath )
        {
            var result = _game.OpenSavedGame(savepath, true);
            return !result ? QSPGameWorld.GetLastError() : null;
        }

        public Exception OpenGame( string gamePath )
        {
            var result = _game.LoadGameWorld(gamePath);
            return !result ? QSPGameWorld.GetLastError() : null;
        }

        public Exception WriteSaveGame( string gamePath )
        {
            _game.ModifyVariables();

            var result = _game.WriteSaveGame(gamePath, true);
            return !result ? QSPGameWorld.GetLastError() : null;
        }
    }
}
