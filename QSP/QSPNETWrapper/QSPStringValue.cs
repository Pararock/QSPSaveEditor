namespace QSPNETWrapper
{
    public class QSPStringValue: QSPValue
    {
        public string Value { get; set; }
        
        public QSPStringValue( int index, string value)
            :base(index)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
