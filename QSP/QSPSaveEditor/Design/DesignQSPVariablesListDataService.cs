namespace QSPSaveEditor.Design
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QSPNETWrapper.Model;

    public class DesignQSPVariablesListDataService : IQSPVariablesListDataService
    {
        public async Task<IList<QSPVariable>> GetQSPVariableList( IQSPGameDataService gameDataService )
        {
            var list = gameDataService.Game.VariablesList;
            return list;
        }
    }
}
