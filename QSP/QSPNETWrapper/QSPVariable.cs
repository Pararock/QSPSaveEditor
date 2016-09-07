namespace QSPNETWrapper.Model
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class QSPVariable<T> : QSPBaseVariable
    {
        private readonly string _name;
        private T _value;

        public QSPVariable( string name, T value )
        {
            _name = name;
            _value = value;
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        public override string ExecString
        {
            get
            {
                return $"{Name} = {Value}";
            }
        }

        public override bool IsDirty { get; protected set; }

        public override string Name => _name;

        public T Value
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

        public override string ToString()
        {
            return $"{Name} = {Value}";
        }

        protected void OnPropertyChanged( [CallerMemberName] string propertyName = null )
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<U>( ref U field, U value, [CallerMemberName] string propertyName = null )
        {
            if ( EqualityComparer<U>.Default.Equals(field, value) ) return false;
            field = value;
            IsDirty = true;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
