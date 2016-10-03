namespace QSPNETWrapper
{
    public class QSPArrayValues: QSPValues
    {
        private readonly int position;
        public QSPArrayValues(int pos, string strValue, int intValue)
           :base(strValue, intValue)
        {
            position = pos;
        }

        public int Position => position;
    }
}
