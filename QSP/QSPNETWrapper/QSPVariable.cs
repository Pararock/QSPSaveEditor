namespace QSPNETWrapper.Model
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class QSPVariable: INotifyPropertyChanged
    {
        private readonly string _name;
        private string _strValue;
        private int _intValue;
        private bool isString;
        private bool _isDirty;

        public QSPVariable( string name, string value )
        {
            _name = name;
            _strValue = value;
            isString = true;
        }

        public QSPVariable( string name, int value )
        {
            _name = name;
            _intValue = value;
            isString = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual string ExecString
        {
            get
            {
                return $"{Name} = {Value}";
            }
        }

        public bool IsDirty
        {
            get
            {
                return _isDirty;
            }
            protected set
            {
                SetField(ref _isDirty, value);
            }
        }

        public string Name => _name;

        public string Value
        {
            get
            {
                return isString ? _strValue : _intValue.ToString();
            }
            set
            {
                if ( isString )
                {
                    SetField(ref _strValue, value);
                }
                else
                {
                    _intValue = int.Parse(value);
                    SetField(ref _strValue, value);
                }
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
