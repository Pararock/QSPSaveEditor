namespace QSPNETWrapper.Model
{
    abstract public class QSPVariable
    {
        private readonly int _index;
        private readonly string _name;

        public bool IsDirty { get; protected set; }

        protected QSPVariable(int variableIndex, string name)
        {
            _index = variableIndex;
            _name = name;
        }

        public string Name => _name;

        public int VariableIndex => _index;
    }
}
