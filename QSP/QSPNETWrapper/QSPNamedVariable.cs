namespace QSPNETWrapper.Model
{
    public class QSPNamedArrayVariable : QSPVariable
    {
        public QSPNamedArrayVariable( string parentName, string name, int value)
            : base(name, value)
        {
            ParentName = parentName;
        }

        public QSPNamedArrayVariable( string parentName, string name, string value )
            : base(name, value)
        {
            ParentName = parentName;
        }

        public override string ExecString
        {
            get
            {
                return $"{ParentName}['{Name}'] = {Value}";
            }
        }
        public string ParentName { get; private set; }

        public int Position { get; private set; }

        public override string ToString()
        {
            return $"{ParentName}['{Name}'] = {Value}";
        }
    }
}
