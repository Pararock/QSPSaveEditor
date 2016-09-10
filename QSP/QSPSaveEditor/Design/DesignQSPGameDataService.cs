
namespace QSPSaveEditor.Design
{
    using Model;
    using QSPNETWrapper;
    using System;
    using System.Threading.Tasks;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public class DesignQSPGameDataService: IQSPGameDataService
    {
        private DesignQSPGame _game;

        public QSPGame Game => _game;

        public int FullRefreshCount => 333;

        public int ObjectsCount => 343;

        public DesignQSPGameDataService()
        {
            _game = new DesignQSPGame();
        }

        public Task<Exception> LoadSaveAsync( string savepath )
        {
            return LoadSaveInternalAsync(savepath);
        }

        private Task<Exception> LoadSaveInternalAsync( string savepath )
        {
            _game.PopulateVariableList();
            return null;
        }

        public Task<Exception> WriteSaveGameAsync( string gamePath )
        {
            throw new NotSupportedException();
        }

        public Task<Exception> OpenGameAsync( string savepath )
        {
            return OpenGameInternalAsync();
        }

        private async static Task<Exception> OpenGameInternalAsync()
        {
            return null;
        }
    }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}
