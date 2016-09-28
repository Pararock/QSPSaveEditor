namespace QSPNETWrapper.Model
{
    public class QSPNamedArrayVariable : QSPVariable
    {
        public QSPNamedArrayVariable( string parentName, string name, string strValue, int intValue)
            : base(name, strValue, intValue)
        {
            IndexName = parentName;
        }

        public override string FullVariableName
        {
            get
            {
                return $"{Name}['{IndexName}']";
            }
        }

        public string IndexName { get; private set; }

        public int Position { get; private set; }
    }
}
