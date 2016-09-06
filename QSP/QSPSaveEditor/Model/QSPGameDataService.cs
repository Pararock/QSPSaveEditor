namespace QSPSaveEditor.Model
{
    using QSPNETWrapper;
    using System;

    class QSPGameDataService : IQSPGameDataService
    {
        private QSPGameWorld _game;

        public void LoadSave( Action<Exception> callback, string savePath )
        {
            if(_game.OpenSavedGame(savePath, true))
            {
                callback?.Invoke(null);
            }
        }

        public void OpenGame( Action<QSPGame, Exception> callback, string gamePath )
        {
            _game = new QSPGameWorld();

            if( _game.LoadGameWorld(gamePath))
            {
                callback?.Invoke(_game, null);
            }
        }
    }
}
