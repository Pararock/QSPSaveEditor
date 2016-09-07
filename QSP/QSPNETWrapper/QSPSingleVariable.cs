namespace QSPNETWrapper.Model
{
    public class QSPSingleVariable: QSPVariable
    {
        public QSPValue Value { get; set; }
        public bool IsString { get; private set; }

        public QSPSingleVariable(int index, string name, QSPValue value)
            :base(index, name)
        {
            Value = value;
            Value.PropertyChanged += Value_PropertyChanged;
        }

        private void Value_PropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
        {
            IsDirty = true;
        }

        public override string ToString()
        {
            return $"Name:{Name} Value:{Value}";
        }
    }
}
