namespace QSPNETWrapper.Model
{
    public class QSPPositionArrayVariable : QSPVariable
    {
        public QSPPositionArrayVariable( string parentName, int position, string strValue, int intValue )
            : base(parentName, strValue, intValue)
        {
            Position = position;
        }

        public override string ExecString
        {
            get
            {
                var returnValue = string.Empty;
                switch ( this.VariableType )
                {
                    case VariableType.StringValue:
                        returnValue = $"${Name}[{Position}] = '{StringValue}'";
                        break;
                    case VariableType.IntValue:
                        returnValue = $"{Name}[{Position}] = {IntValue}";
                        break;
                    case VariableType.BothValues:
                        returnValue = $"${Name}[{Position}] = '{StringValue}' & {Name}[{Position}] = {IntValue}";
                        break;
                }
                return returnValue;
            }
        }

        public int Position { get; private set; }

        public override string ToString()
        {
            return ExecString;
        }
    }
}
