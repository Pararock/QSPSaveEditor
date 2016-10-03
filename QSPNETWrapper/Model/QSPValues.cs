namespace QSPNETWrapper.Model
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public enum VariableType
    {
        IntValue,
        StringValue,
        BothValues
    }

    public class QSPValues : INotifyPropertyChanged
    {
        private VariableType variableType;

        private string stringValue;
        private string newStringValue;

        private int integerValue;
        private int newIntegerValue;

        private bool isDirty;
        private bool isModified;

        public event PropertyChangedEventHandler PropertyChanged;

        public QSPValues(string strValue, int intValue)
        {
            if ( string.IsNullOrEmpty(strValue) )
            {
                variableType = VariableType.IntValue;
            }
            else if ( intValue == 0 )
            {
                variableType = VariableType.StringValue;
            }
            else
            {
                variableType = VariableType.BothValues;
            }

            integerValue = intValue;
            stringValue = strValue;
        }

        public int CharacterCount
        {
            get
            {
                return string.IsNullOrEmpty(stringValue) ? 0 : stringValue.Length;
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

        public string StringValueEscaped
        {
            get
            {
                if ( StringValue.Contains("'") )
                {
                    return StringValue.Replace("'", "''");
                }
                else
                {
                    return StringValue;
                }
            }
        }

        public void NewValues( QSPValues newValues )
        {
            if ( stringValue != newValues.stringValue || integerValue != newValues.integerValue )
            {
                newStringValue = newValues.stringValue;
                newIntegerValue = newValues.integerValue;

                // If both value are set, change the type to both values.
                // This can happens when both string and int are in use, but the int was equal to 0 the first time around
                if ( !string.IsNullOrEmpty(stringValue) && integerValue != 0 )
                {
                    VariableType = VariableType.BothValues;
                }
                IsModified = true;
            }
        }

        public void ResetBaseLine()
        {
            integerValue = newIntegerValue;
            stringValue = newStringValue;
            IsModified = false;
            IsDirty = false;
            OnPropertyChanged(nameof(IntegerValue));
            OnPropertyChanged(nameof(StringValue));
        }

        public bool IsDirty
        {
            get
            {
                return isDirty;
            }
            set
            {
                SetField(ref isDirty, value);
            }
        }

        public bool IsModified
        {
            get
            {
                return isModified;
            }
            set
            {
                SetField(ref isModified, value);
            }
        }

        public string StringValue
        {
            get
            {
                return stringValue;
            }
            set
            {
                SetField(ref stringValue, value);
            }
        }

        public string NewStringValue
        {
            get
            {
                return newStringValue;
            }
            set
            {
                SetField(ref newStringValue, value);
            }
        }

        public int IntegerValue
        {
            get
            {
                return integerValue;
            }
            set
            {
                SetField(ref integerValue, value);
            }
        }

        public int NewIntegerValue
        {
            get
            {
                return newIntegerValue;
            }
            set
            {
                SetField(ref newIntegerValue, value);
            }
        }

        protected void OnPropertyChanged( [CallerMemberName] string propertyName = null )
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>( ref T field, T value, [CallerMemberName] string propertyName = null )
        {
            if ( EqualityComparer<T>.Default.Equals(field, value) ) return false;
            field = value;

            // Don't set the dirty flag for metadata change on the variable
            if ( propertyName != nameof(IsDirty) && propertyName != nameof(IsModified))
            {
                IsDirty = true;
            }

            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
