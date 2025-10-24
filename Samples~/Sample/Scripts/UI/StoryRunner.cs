using Aftertime.HappinessBlossom.Presenter;
using Aftertime.HappinessBlossom.UI.Layout;
using Aftertime.StorylineEngine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lucecita
{
    public class StoryRunner : MonoBehaviour
    {
        [SerializeField] private EpisodeData[] _episodeDatas;
        [SerializeField] private ConversationPresenter _conversationPresenter;
        private StoryFlowController _storyFlowController;

        private void Start()
        {
            _storyFlowController = new StoryFlowController(_episodeDatas);

            RegisterPresenterEvent();
            RegisterFunctionEvent();
        }

        public void Run()
        {
            _conversationPresenter.Show();
            _storyFlowController.StartEpisode("1");
            LayoutManager.Instance.Hide<TitleLayout>();
            LayoutManager.Instance.Show<StoryLayout>();
        }
        
        protected virtual void RegisterPresenterEvent()
        {
            _conversationPresenter.onClickConversationButton += () => OnClickConversationButton();
        }

        private void RegisterFunctionEvent()
        {
            Choice.moveNextTextElement += _storyFlowController.StartElement;
        }

        private void OnClickConversationButton(InputAction.CallbackContext callback = default)
        {
            bool isTextShowing = _conversationPresenter.IsTextShowing();

            // 텍스트 애니메이션 중 1회 클릭 시, 모든 텍스트 Show Event
            if (isTextShowing)
            {
                _conversationPresenter.ShowTextCompleted();
            }
            // 다음 TextElement Execute
            else
            {
                _storyFlowController.NextElement();
            }
        }
    }
   
}