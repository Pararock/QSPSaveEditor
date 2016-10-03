namespace QSPNETWrapper.Model
{
    public class QSPAction
    {
        public int Index { get; }
        public string ImagePath { get; }
        public string Description { get; }

        public QSPAction(int index, string imgPath, string desc)
        {
            Index = index;
            ImagePath = imgPath;
            Description = desc;
        }
    }
}
