namespace QSPNETWrapper.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public enum VariableType
    {
        IntValue,
        StringValue,
        BothValues
    }

    public class QSPVariable: INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly string _name;
        private VariableType variableType;
        private string _strValue;
        private int _intValue;
        private bool _isDirty;

        private bool _isModified;

        private bool _isNew;

        private string _strNewValue;
        private int _intNewValue;

        public QSPVariable( string name, string strValue, int intValue )
        {
            _name = name;

            if(string.IsNullOrEmpty(strValue))
            {
                variableType = VariableType.IntValue;
            }
            else if(intValue == 0)
            {
                variableType = VariableType.StringValue;
            }
            else
            {
                variableType = VariableType.BothValues;
            }

            _intValue = intValue;
            _strValue = strValue;
        }

        public int CharacterCount
        {
            get
            {
                return string.IsNullOrEmpty(_strValue) ? 0 : _strValue.Length;
            }
        }

        public void NewValues( QSPVariable newVariable)
        {
            if( _strValue != newVariable._strValue || _intValue != newVariable._intValue )
            {
                _strNewValue = newVariable._strValue;
                _intNewValue = newVariable._intValue;

                // If both value are set reset the type to both values.
                // This can happens when both string and int are in use, but the int was equal to 0 the first time around
                if(!string.IsNullOrEmpty(_strNewValue) && _intNewValue !=0)
                {
                    VariableType = VariableType.BothValues;
                }
                IsModified = true;
            }
        }

        public virtual string FullVariableName
        {
            get
            {
                return $"{Name}";
            }
        }

        public VariableType VariableType
        {
            get
            {
                return variableType;
            }
            set
            {
                SetField(ref variableType, value);
            }
        }

        private string StringValueEscaped
        {
            get
            {
                if(StringValue.Contains("'"))
                {
                    return StringValue.Replace("'", "''");
                }
                else
                {
                    return StringValue;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual string ExecString
        {
            get
            {
                var returnValue = string.Empty;
                switch ( variableType )
                {
                    case VariableType.StringValue:
                        returnValue =  $"${FullVariableName} = '{StringValueEscaped}'";
                        break;
                    case VariableType.IntValue:
                        returnValue = $"{FullVariableName} = {IntValue}";
                        break;
                    case VariableType.BothValues:
                        returnValue = $"${FullVariableName} = '{StringValueEscaped}' & {FullVariableName} = {IntValue}";
                        break;
                }
                return returnValue;
            }
        }

        public bool IsNew
        {
            get
            {
                return _isNew;
            }
            set
            {
                SetField(ref _isNew, value);
            }
        }

        public bool IsDirty
        {
            get
            {
                return _isDirty;
            }
            set
            {
                SetField(ref _isDirty, value);
            }
        }

        public bool IsModified
        {
            get
            {
                return _isModified;
            }
            set
            {
                SetField(ref _isModified, value);
            }
        }

        public string Name => _name;

        public string StringValue
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

        public string NewStringValue
        {
            get
            {
                return _strNewValue;
            }
            set
            {
                SetField(ref _strNewValue, value);
            }
        }

        public int IntValue
        {
            get
            {
                return _intValue;
            }
            set
            {
                SetField(ref _intValue, value);
            }
        }

        public int NewIntValue
        {
            get
            {
                return _intNewValue;
            }
            set
            {
                SetField(ref _intNewValue, value);
            }
        }

        public string Error
        {
            get
            {
                return string.Empty;
            }
        }

        public void ResetModified()
        {
            _intValue = _intNewValue;
            _strValue = _strNewValue;
            IsModified = false;
            IsDirty = false;
            OnPropertyChanged(nameof(IntValue));
            OnPropertyChanged(nameof(StringValue));
        }


        public string this[string columnName]
        {
            get
            {
                var validationMessage = string.Empty;
                /*switch ( columnName )
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
                }*/

                return validationMessage;
            }
        }

        public override string ToString()
        {
            var returnValue = string.Empty;
            switch ( variableType )
            {
                case VariableType.StringValue:
                    returnValue = $"${FullVariableName} = '{StringValue}'";
                    break;
                case VariableType.IntValue:
                    returnValue = $"{FullVariableName} = {IntValue}";
                    break;
                case VariableType.BothValues:
                    returnValue = $"${FullVariableName} = '{StringValue}' & {FullVariableName} = {IntValue}";
                    break;
            }
            return returnValue;
        }

        protected void OnPropertyChanged( [CallerMemberName] string propertyName = null )
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<U>( ref U field, U value, [CallerMemberName] string propertyName = null )
        {
            if ( EqualityComparer<U>.Default.Equals(field, value) ) return false;
            field = value;
            if ( propertyName != nameof(IsDirty) && propertyName != nameof(IsModified) && propertyName != nameof(IsNew))
            {
                IsDirty = true;
            }
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
