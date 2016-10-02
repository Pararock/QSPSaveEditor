namespace QSPNETWrapper.Model
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Linq;

    public class QSPVariable : INotifyPropertyChanged
    {
        private readonly string variableName;
        private readonly List<QSPValues> arraySubVariables;
        private Dictionary<int, string> indicesName;
        private bool isNew;

        private int integerCount;
        private int stringCount;
        private int bothCount;

        public QSPVariable( string name, string strValue, int intValue )
        {
            variableName = name;
            arraySubVariables = new List<QSPValues>();
            AddNewVariable(0, strValue, intValue);
        }

        public QSPVariable( string name, int valuesCount , int indicesCount)
        {
            variableName = name;
            arraySubVariables = new List<QSPValues>(valuesCount);
            indicesName = new Dictionary<int, string>(indicesCount);
        }

        public void AddValues(int index, string strValue, int intValue)
        {
            AddNewVariable(index, strValue, intValue);
        }

        public bool IsArray => arraySubVariables.Count > 1;

        private void AddNewVariable(int index, string strValue, int intValue)
        {
            var subVariable = new QSPValues(strValue, intValue);

            switch ( subVariable.VariableType )
            {
                case VariableType.BothValues:
                {
                    integerCount++;
                    break;
                }
                case VariableType.IntValue:
                {
                    integerCount++;
                    break;
                }
                case VariableType.StringValue:
                {
                    stringCount++;
                    break;
                }
            }

            arraySubVariables.Insert(index, subVariable);
        }

        public void SetIndexName(int index, string indexName)
        {
            indicesName.Add(index, indexName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string ExecString
        {
            get
            {
                var returnValue = string.Empty;
                switch ( Values.VariableType )
                {
                    case VariableType.StringValue:
                        returnValue = $"${FullVariableName} = '{Values.StringValueEscaped}'";
                        break;
                    case VariableType.IntValue:
                        returnValue = $"{FullVariableName} = {Values.IntegerValue}";
                        break;
                    case VariableType.BothValues:
                        returnValue = $"${FullVariableName} = '{Values.StringValueEscaped}' & {FullVariableName} = {Values.IntegerValue}";
                        break;
                }
                return returnValue;
            }
        }

        public bool IsDirty
        {
            get
            {
                return arraySubVariables.Any(x => x.IsDirty);
            }
        }

        public bool IsModified
        {
            get
            {
                return arraySubVariables.Any(x => x.IsModified);
            }
        }

        public int ValuesCount
        {
            get
            {
                return arraySubVariables.Count;
            }
        }

        public int IntegerCount
        {
            get {
                return integerCount;
            }
        }

        public int StringCount
        {
            get
            {
                return stringCount;
            }
        }

        public int BothValuesCount
        {
            get
            {
                return bothCount;
            }
        }

        public string FullVariableName
        {
            get
            {
                return $"{Name}";
            }
        }

        public bool IsNew
        {
            get
            {
                return isNew;
            }
            set
            {
                SetField(ref isNew, value);
            }
        }

        public string Name => variableName;

        public QSPValues Values
        {
            get
            {
                return arraySubVariables.FirstOrDefault();
            }
        }

        public static QSPVariable CreateVariable( string name, int intValue, string strValue )
        {
            return new QSPVariable(name, strValue, intValue); ;
        }

        public void NewValues( QSPValues newValues )
        {
            Values.NewValues(newValues);
        }

        public override string ToString()
        {
            var returnValue = string.Empty;
            switch ( Values.VariableType )
            {
                case VariableType.StringValue:
                    returnValue = $"${FullVariableName} = '{Values.StringValue}'";
                    break;
                case VariableType.IntValue:
                    returnValue = $"{FullVariableName} = {Values.IntegerValue}";
                    break;
                case VariableType.BothValues:
                    returnValue = $"${FullVariableName} = '{Values.StringValue}' & {FullVariableName} = {Values.IntegerValue}";
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
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
