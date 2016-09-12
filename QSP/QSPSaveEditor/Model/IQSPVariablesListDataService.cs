namespace QSPSaveEditor.Model
{
    using QSPNETWrapper.Model;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IQSPVariablesListDataService
    {
        Task<IList<QSPVariable>> GetQSPVariableList(IQSPGameDataService gameDataService);
    }

    public enum VariableListMessageToken
    {
        LoadList,
        ReloadList
    }
}
