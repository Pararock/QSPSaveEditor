namespace QSPNETWrapper.Model
{
    public class QSPNamedArrayValues: QSPArrayValues
    {
        private readonly string indexName;
        public QSPNamedArrayValues(string name, int position, string strValue, int intValue)
            :base(position, strValue, intValue)
        {
            indexName = name;
        }

        public QSPNamedArrayValues(string name, QSPArrayValues values)
            :base(values.Position, values.StringValue, values.IntegerValue)
        {
            indexName = name;
        }

        public string Name => indexName;
    }
}
