using Febucci.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Aftertime.HappinessBlossom.UI
{
    public class ConversationView : View
    {
        public RectTransform ConversationBox => _conversationBox;
        public Button ConversationButton => _conversationButton;
        public TypewriterByCharacter Typewriter => _typewriter;
        public TextMeshProUGUI NameText => _nameText;
        
        [SerializeField] private RectTransform _conversationBox;
        [SerializeField] private Button _conversationButton;
        [SerializeField] private TypewriterByCharacter _typewriter;
        [SerializeField] private TextMeshProUGUI _nameText;

        public override void ResetView()
        {
            base.ResetView();
            _typewriter.TextAnimator.SetText("");
            _nameText.SetText("");
        }
    }
   
}