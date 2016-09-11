namespace QSPNETWrapper.Model
{
    public class QSPVariantVariable : QSPVariable
    {
        private int intValue;

        public QSPVariantVariable( string name, int intValue, string strValue )
            : base(name, strValue)
        {
            this.intValue = intValue;
        }

        public int IntValue
        {
            get
            {
                return intValue;
            }
            set
            {
                SetField(ref intValue, value);
            }
        }

        public override string ExecString
        {
            get
            {
                return $"${Name} = '{Value}' & {Name} = {IntValue}";
            }
        }

        public override string ToString()
        {
            return $"${Name} = {Value} & {Name} = {IntValue}";
        }
    }
}
