namespace QSPNETWrapper
{
    abstract public class QSPVariable
    {
        private int _index;
        private string _name;

        public QSPVariable(int variableIndex, string name)
        {
            _index = variableIndex;
            _name = name;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public int VariableIndex
        {
            get
            {
                return _index;
            }
        }
    }
}
