namespace QSPSaveEditor.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QSPNETWrapper.Model;

    public class QSPVariablesListDataService : IQSPVariablesListDataService
    {
        public Task<IList<QSPVariable>> GetQSPVariableList( IQSPGameDataService gameDataService )
        {
            return GetQSPVariableListInternalAsync(gameDataService);
        }

        private async Task<IList<QSPVariable>> GetQSPVariableListInternalAsync( IQSPGameDataService gameDataService )
        {
            var result = await Task.Run(() => gameDataService.Game.VariablesList);
            return result;
        }
    }
}
