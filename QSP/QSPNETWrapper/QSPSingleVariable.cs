namespace QSPNETWrapper
{
    public class QSPSingleVariable: QSPVariable
    {
        public QSPValue Value { get; set; }
        public bool IsString { get; private set; }

        public QSPSingleVariable(int index, string name, QSPValue value)
            :base(index, name)
        {
            Value = value;
        }
    }
}
