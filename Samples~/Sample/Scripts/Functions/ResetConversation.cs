using Aftertime.HappinessBlossom.Presenter;
using Aftertime.StorylineEngine;
using Cysharp.Threading.Tasks;

namespace Aftertime.SecretSome
{
    public class ResetConversation : Function, IElementChangeFunc
    {
        [Presenter] private ConversationPresenter Presenter;
        public override async UniTask StartFunction()
        {
            Presenter.ResetView();
        }

        public TextElement GetNextElement()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextTextElement = EpisodeData.GetNextTextElement(curElementIndex);
            return nextTextElement;
        }
    }
}
