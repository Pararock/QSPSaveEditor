namespace QSPNETWrapper.Model
{
    public class QSPPositionArrayVariable : QSPVariable
    {
        public QSPPositionArrayVariable( string parentName, int position, int value )
            : base(parentName, value)
        {
            Position = position;
        }

        public QSPPositionArrayVariable( string parentName, int position, string value )
            : base(parentName, value)
        {
            Position = position;
        }

        public override string ExecString
        {
            get
            {
                return $"{Name}[{Position}] = {Value}";
            }
        }

        public int Position { get; private set; }

        public override string ToString()
        {
            return $"{Name}[{Position}] = {Value}";
        }
    }
}
