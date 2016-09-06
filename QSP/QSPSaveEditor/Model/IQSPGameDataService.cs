namespace QSPSaveEditor.Model
{
    using QSPNETWrapper;
    using System;

    public interface IQSPGameDataService
    {
        void OpenGame( Action<QSPGame, Exception> callback, string gamePath );
        void LoadSave( Action<Exception> callback, string savePath );
    }
}
