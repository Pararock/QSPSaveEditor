namespace QSPNETWrapper
{
    using System.ComponentModel;

    public abstract class QSPBaseVariable : INotifyPropertyChanged
    {
        public abstract event PropertyChangedEventHandler PropertyChanged;
        public virtual string ExecString { get; }
        public virtual bool IsDirty { get; protected set; }
        public virtual string Name { get; protected set; }
    }
}
