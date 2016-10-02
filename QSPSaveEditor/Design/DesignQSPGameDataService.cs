
namespace QSPSaveEditor.Design
{
    using Model;
    using QSPNETWrapper;
    using System;
    using System.Threading.Tasks;
    using QSPNETWrapper.Model;
    using System.Collections.Generic;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public class DesignQSPGameDataService: IQSPGameDataService
    {
        private DesignQSPGame _game;

        public QSPGame Game => _game;

        public int FullRefreshCount => 333;

        public int ObjectsCount => 343;

        public IList<QSPVariable> QSPVariablesList
        {
            get
            {
                return _game.VariablesList;
            }
        }

        public DesignQSPGameDataService()
        {
            _game = new DesignQSPGame();
            _game.PopulateVariableList();
        }

        public Exception LoadSave( string savepath )
        {
            return null;
        }

        public Exception WriteSaveGame( string gamePath )
        {
            throw new NotSupportedException();
        }

        public Exception OpenGame( string savepath )
        {
            throw new NotSupportedException();
        }
    }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}
