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

            arraySubVariables.Insert(0, subVariable);
        }

        public QSPVariable( string name, int valuesCount , int indicesCount)
        {
            variableName = name;
            arraySubVariables = new List<QSPValues>(valuesCount);
            indicesName = new Dictionary<int, string>(indicesCount);
        }

        public void AddValue(int index, string strValue, int intValue)
        {
            AddNewVariable(index, strValue, intValue);
        }

        public List<QSPValues> Values => arraySubVariables;

        public bool IsArray => arraySubVariables.Count > 1;

        private void AddNewVariable(int index, string strValue, int intValue)
        {
            var subVariable = new QSPArrayValues(index, strValue, intValue);

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
            var newVariable = new QSPNamedArrayValues(indexName, arraySubVariables[index] as QSPArrayValues);
            arraySubVariables[index] = newVariable;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string ExecString
        {
            get
            {
                var returnValue = string.Empty;
                switch ( Value.VariableType )
                {
                    case VariableType.StringValue:
                        returnValue = $"${FullVariableName} = '{Value.StringValueEscaped}'";
                        break;
                    case VariableType.IntValue:
                        returnValue = $"{FullVariableName} = {Value.IntegerValue}";
                        break;
                    case VariableType.BothValues:
                        returnValue = $"${FullVariableName} = '{Value.StringValueEscaped}' & {FullVariableName} = {Value.IntegerValue}";
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

        public QSPValues Value
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
            Value.NewValues(newValues);
        }

        public override string ToString()
        {
            var returnValue = string.Empty;
            switch ( Value.VariableType )
            {
                case VariableType.StringValue:
                    returnValue = $"${FullVariableName} = '{Value.StringValue}'";
                    break;
                case VariableType.IntValue:
                    returnValue = $"{FullVariableName} = {Value.IntegerValue}";
                    break;
                case VariableType.BothValues:
                    returnValue = $"${FullVariableName} = '{Value.StringValue}' & {FullVariableName} = {Value.IntegerValue}";
                    break;
            }
            return returnValue;
        }

        protected void OnPropertyChanged( [CallerMemberName] string propertyName = null )
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>( ref T field, T value, [CallerMemberName] string propertyName = null )
        {
            if ( EqualityComparer<T>.Default.Equals(field, value) ) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
