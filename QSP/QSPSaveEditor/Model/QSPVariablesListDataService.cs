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
        private IList<QSPVariable> listeVariable;
        public Task<IList<QSPVariable>> GetQSPVariableList( IQSPGameDataService gameDataService )
        {
            return GetQSPVariableListInternalAsync(gameDataService);
        }

        public void ResetVariablesBaseline()
        {
            var modifiedVar = listeVariable.Where(x => x.IsModified).Select(x => x);
            foreach(var variable in modifiedVar)
            {
                variable.ResetModified();
            }
        }

        private async Task<IList<QSPVariable>> GetQSPVariableListInternalAsync( IQSPGameDataService gameDataService )
        {
            listeVariable = await Task.Run(() => gameDataService.Game.VariablesList);
            return listeVariable;
        }
    }
}
