namespace QSPNETWrapper.Model
{
    public class QSPNamedArrayVariable : QSPVariable
    {
        public QSPNamedArrayVariable( string parentName, string name, string strValue, int intValue)
            : base(name, strValue, intValue)
        {
            ParentName = parentName;
        }

        public override string FullVariableName
        {
            get
            {
                return $"{ParentName}['{Name}']";
            }
        }

        public string ParentName { get; private set; }

        public int Position { get; private set; }
    }
}
