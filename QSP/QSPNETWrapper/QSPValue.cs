namespace QSPNETWrapper
{
    public abstract class QSPValue
    {
        public int ValueIndex { get; private set; }

        public QSPValue(int valueIndex)
        {
            ValueIndex = valueIndex;
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
