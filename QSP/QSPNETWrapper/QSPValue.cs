namespace QSPNETWrapper.Model
{
    using System.ComponentModel;

    public abstract class QSPValue: INotifyPropertyChanged
    {
        public int ValueIndex { get; private set; }
        public bool IsDirty { get; protected set; }

        protected QSPValue(int valueIndex)
        {
            ValueIndex = valueIndex;
        }

        public abstract event PropertyChangedEventHandler PropertyChanged;

        public override string ToString() => string.Empty;
    }
}
