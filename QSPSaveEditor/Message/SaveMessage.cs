namespace QSPSaveEditor.Message
{
    using GalaSoft.MvvmLight.Messaging;

    public class SaveMessage: MessageBase
    {
        public SaveMessageType MessageType { get; private set; }

        public SaveMessage(SaveMessageType type)
        {
            MessageType = type;
        }
    }

    public enum SaveMessageType
    {
        SaveLoaded,
        SaveReloaded,
        SaveClosed
    }
}
