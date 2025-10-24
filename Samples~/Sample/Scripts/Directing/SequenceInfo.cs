using DG.Tweening;

namespace Aftertime.HappinessBlossom.Directing
{
    public class SequenceInfo
    {
        public int ImageID { get; private set; }
        public ImageDirectingType DirectingType { get; private set; }
        public Sequence Sequence { get; private set; }

        public SequenceInfo(int imageID,ImageDirectingType directingType, Sequence sequence)
        {
            ImageID = imageID;
            DirectingType = directingType;
            Sequence = sequence;
        }
    }   
}