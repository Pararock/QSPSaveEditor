namespace QSPSaveEditor.Model
{
    using QSPNETWrapper;
    using System;
    using System.Threading.Tasks;

    public interface IQSPGameDataService
    {
        QSPGame Game { get; }
        Exception LoadSave( string savePath );
        Exception OpenGame( string gamePath );
        Exception WriteSaveGame( string savePath );
    }
}
