namespace QSPNETWrapper
{
    public class QSPSingleVariable: QSPVariable
    {
        public string Value { get; set; }

        QSPSingleVariable(int index, string name, string value)
            :base(index, name)
        {
            Value = value;
        }
    }
}
