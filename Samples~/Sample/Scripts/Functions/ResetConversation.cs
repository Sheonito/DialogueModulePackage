using Lucecita.HappinessBlossom.Presenter;
using Lucecita.StorylineEngine;
using Cysharp.Threading.Tasks;

namespace Lucecita.SecretSome
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
