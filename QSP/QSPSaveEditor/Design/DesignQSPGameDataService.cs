
namespace QSPSaveEditor.Design
{
    using Model;
    using QSPNETWrapper;
    using System;

    public class DesignQSPGameDataService: IQSPGameDataService
    {
        private DesignQSPGame _game;
        public void OpenGame( Action<QSPGame, Exception> callback, string gamePath )
        {
            _game = new DesignQSPGame();
            callback?.Invoke(_game, null);

        }

        public void LoadSave( Action<Exception> callback, string savePath )
        {
            _game.PopulateVariableList();
            callback?.Invoke(null);
        }
    }
}
