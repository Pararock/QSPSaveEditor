namespace QSPNETWrapper
{
    abstract public class QSPVariable
    {
        private int _index;
        private string _name;

        public QSPVariable(int index, string name)
        {
            _index = index;
            _name = name;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public int Index
        {
            get
            {
                return _index;
            }
        }
    }
}
