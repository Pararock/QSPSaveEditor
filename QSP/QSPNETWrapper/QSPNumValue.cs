namespace QSPNETWrapper
{
    public class QSPNumValue: QSPValue
    {
        public int Value { get; set; }
        public QSPNumValue(int index, int value)
            :base(index)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
