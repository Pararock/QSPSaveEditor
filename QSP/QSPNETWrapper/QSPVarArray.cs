namespace QSPNETWrapper.Model
{
    using System.Collections.Generic;

    public class QSPVarArray: QSPVariable
    {
        private List<QSPSingleVariable> _values;
        public QSPVarArray(int index, string name, List<QSPSingleVariable> values )
            :base(index, name)
        {
            _values = values;
        }

        public List<QSPSingleVariable> Values
        {
            get
            {
                return _values;
            }
        }
    }
}
