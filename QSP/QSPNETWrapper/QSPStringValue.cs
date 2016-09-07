namespace QSPNETWrapper.Model
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class QSPStringValue : QSPValue
    {

        public QSPStringValue( int index, string value )
            : base(index)
        {
            Value = value;
        }

        public override event PropertyChangedEventHandler PropertyChanged;
        public string Value { get; set; }

        public override string ToString()
        {
            return $"STRRR {Value}";
        }

        protected void OnPropertyChanged( [CallerMemberName] string propertyName = null )
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>( ref T field, T value, [CallerMemberName] string propertyName = null )
        {
            if ( EqualityComparer<T>.Default.Equals(field, value) ) return false;
            field = value;
            IsDirty = true;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
