namespace QSPNETWrapper.Model
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class QSPNumValue : QSPValue
    {
        private int _value;

        public QSPNumValue( int index, int value )
            : base(index)
        {
            _value = value;
        }

        public int Value
        {
            get
            {
                return _value;
            }

            set
            {
                SetField(ref _value, value);

            }
        }

        protected bool SetField<T>( ref T field, T value, [CallerMemberName] string propertyName = null )
        {
            if ( EqualityComparer<T>.Default.Equals(field, value) ) return false;
            field = value;
            IsDirty = true;
            OnPropertyChanged(propertyName);
            return true;
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged( [CallerMemberName] string propertyName = null )
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"NUM###{Value.ToString()}";
        }
    }
}
