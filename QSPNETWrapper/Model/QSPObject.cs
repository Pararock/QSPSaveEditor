namespace QSPNETWrapper.Model
{
    public class QSPObject
    {
        public int Index { get; }
        public string ImagePath { get; }
        public string Description { get; }

        public QSPObject( int index, string imgPath, string desc )
        {
            Index = index;
            ImagePath = imgPath;
            Description = desc;
        }
    }
}
