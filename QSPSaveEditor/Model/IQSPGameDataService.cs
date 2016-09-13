namespace QSPSaveEditor.Model
{
    using QSPNETWrapper;
    using System;
    using System.Threading.Tasks;

    public interface IQSPGameDataService
    {
        QSPGame Game { get; }
        Task<Exception> LoadSaveAsync( string savePath );
        Task<Exception> OpenGameAsync( string gamePath );
        Task<Exception> WriteSaveGameAsync( string savePath );
    }
}
