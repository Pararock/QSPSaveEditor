namespace QSPSaveEditor.Design
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using QSPNETWrapper.Model;

    public class DesignQSPVariablesListDataService : IQSPVariablesListDataService
    {
        public async Task<IList<QSPVariable>> GetQSPVariableList( IQSPGameDataService gameDataService )
        {
            var list = gameDataService.Game.VariablesList;
            return list;
        }
        public void ResetVariablesBaseline()
        {
            throw new NotSupportedException();
        }
    }
}
