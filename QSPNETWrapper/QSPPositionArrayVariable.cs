namespace QSPNETWrapper.Model
{
    public class QSPPositionArrayVariable : QSPVariable
    {
        public QSPPositionArrayVariable( string parentName, int position, string strValue, int intValue )
            : base(parentName, strValue, intValue)
        {
            Position = position;
        }

        public override string FullVariableName
        {
            get
            {
                return $"{Name}[{Position}]";
            }
        }

        public int Position { get; private set; }
    }
}
