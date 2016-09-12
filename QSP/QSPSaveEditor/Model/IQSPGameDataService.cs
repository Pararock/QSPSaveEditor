namespace QSPSaveEditor.Model
{
    using QSPNETWrapper;
    using QSPNETWrapper.Model;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IQSPGameDataService
    {
        QSPGame Game { get; }
        Task<Exception> LoadSaveAsync( string savePath );
        Task<Exception> OpenGameAsync( string gamePath );
        Task<Exception> WriteSaveGameAsync( string savePath );
        IList<QSPVariable> QSPVariablesList { get; }
    }
}
