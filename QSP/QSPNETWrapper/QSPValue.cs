namespace QSPNETWrapper.Model
{
    public abstract class QSPValue
    {
        public int ValueIndex { get; private set; }

        protected QSPValue(int valueIndex)
        {
            ValueIndex = valueIndex;
        }

        public override string ToString() => string.Empty;
    }
}
