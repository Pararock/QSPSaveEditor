namespace QSPNETWrapper.Model
{
    using System.Collections.Generic;

    public class QSPVarArray: QSPVariable
    {
        private IEnumerable<QSPSingleVariable> _values;
        public QSPVarArray(int index, string name, IEnumerable<QSPSingleVariable> values )
            :base(index, name)
        {
            _values = values;
            foreach(QSPSingleVariable var in _values)
            {
                var.Value.PropertyChanged += Value_PropertyChanged;
            }
        }

        private void Value_PropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
        {
            IsDirty = true;
        }

        public IEnumerable<QSPSingleVariable> Values
        {
            get
            {
                return _values;
            }
        }
    }
}
