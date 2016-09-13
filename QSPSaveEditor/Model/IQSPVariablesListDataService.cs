namespace QSPSaveEditor.Model
{
    using QSPNETWrapper.Model;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IQSPVariablesListDataService
    {
        Task<IList<QSPVariable>> GetQSPVariableList(IQSPGameDataService gameDataService);
        void ResetVariablesBaseline();
    }

    public enum VariableListMessageToken
    {
        LoadList,
        ReloadList
    }
}
