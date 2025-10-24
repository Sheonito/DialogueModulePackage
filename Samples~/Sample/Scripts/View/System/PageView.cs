namespace Aftertime.HappinessBlossom.UI
{
    public class PageView : View
    {
        public override void SelectFirstSelectedObject()
        {
            firstSelectedObject = _originFirstSelectedObject;
            base.SelectFirstSelectedObject();
        }
    }
}