namespace QSPNETWrapper.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;

    public class QSPVariable: INotifyPropertyChanged, IDataErrorInfo
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
            _intValue = 0;
        }

        public QSPVariable( string name, int value )
        {
            _name = name;
            _intValue = value;
            _strValue = value.ToString();
            isString = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual string ExecString
        {
            get
            {
                if ( isString )
                {
                    return $"{Name} = '{Value}'";
                }
                else
                {
                    return $"{Name} = {Value}";
                }
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
                return _strValue;
            }
            set
            {
                SetField(ref _strValue, value);
            }
        }

        public string Error
        {
            get
            {
                return string.Empty;
            }
        }

        public string this[string columnName]
        {
            get
            {
                var validationMessage = string.Empty;
                switch ( columnName )
                {
                    case nameof(Value):
                        if ( !isString )
                        {
                            int newValue;

                            try
                            {
                                newValue = int.Parse(Value);
                            }
                            catch ( OverflowException e )
                            {
                                if ( Value.StartsWith("-") )
                                {
                                    newValue = int.MinValue;
                                    validationMessage = $"Too small. The value will be {int.MinValue} in game";
                                }
                                else
                                {
                                    newValue = int.MaxValue;
                                    validationMessage = $"Too big. The value will be {int.MaxValue} in game";
                                }
                            }
                            catch ( FormatException e )
                            {
                                newValue = 0;
                                validationMessage = $"Invalid number. The value will be 0 in game";
                            }
                            _intValue = newValue;
                        }
                        break;
                }

                return validationMessage;
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
