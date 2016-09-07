namespace QSPNETWrapper.Model
{
    public class QSPArrayVariable<T> : QSPVariable<T>
    {
        public QSPArrayVariable( string parentName, string name, T value )
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

        public override string ToString()
        {
            return $"{ParentName}['{Name}'] = {Value}";
        }
    }
}
