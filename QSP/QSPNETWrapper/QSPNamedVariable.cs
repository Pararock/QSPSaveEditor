namespace QSPNETWrapper.Model
{
    public class QSPNamedArrayVariable : QSPVariable
    {
        public QSPNamedArrayVariable( string parentName, string name, string strValue, int intValue)
            : base(name, strValue, intValue)
        {
            ParentName = parentName;
        }

        public override string ExecString
        {
            get
            {
                var returnValue = string.Empty;
                switch ( this.VariableType )
                {
                    case VariableType.StringValue:
                        returnValue = $"${ParentName}['{Name}'] = '{StringValue}'";
                        break;
                    case VariableType.IntValue:
                        returnValue = $"{ParentName}['{Name}'] = {IntValue}";
                        break;
                    case VariableType.BothValues:
                        returnValue = $"${ParentName}['{Name}'] = '{StringValue}' & {ParentName}['{Name}'] = {IntValue}";
                        break;
                }
                return returnValue;
            }
        }

        public string ParentName { get; private set; }

        public int Position { get; private set; }

        public override string ToString()
        {
            return ExecString;
        }
    }
}
