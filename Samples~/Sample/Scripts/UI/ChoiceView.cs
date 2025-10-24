using System.Collections.Generic;
using System.Linq;
using Aftertime.HappinessBlossom;
using Aftertime.StorylineEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Lucecita
{
    public class ChoiceView : View
    {
        [SerializeField] private List<ChoiceButton> selectButtons;
        [SerializeField] private Image _dimImage;
        [SerializeField] private RectTransform _content;

        public override void SetInteractable(bool enable)
        {
            base.SetInteractable(enable);
            SetBlocksRaycasts(enable);

            foreach (ChoiceButton selectButton in selectButtons)
            {
                selectButton.interactable = enable;
            }
        }

        public override void ResetView()
        {
            base.ResetView();
            foreach (ChoiceButton selectButton in selectButtons)
            {
                selectButton.onClick.RemoveAllListeners();
                selectButton.gameObject.SetActive(false);
            }
        }

        public ChoiceButton GetInActiveButton()
        {
            ChoiceButton inActiveButton = selectButtons.FirstOrDefault(button => button.gameObject.activeSelf == false);

            return inActiveButton;
        }

        public ChoiceButton GetSelectButton(string buttonTitle)
        {
            ChoiceButton button = selectButtons.FirstOrDefault(button => button.TextMeshPro.text == buttonTitle);

            return button;
        }

        public List<ChoiceButton> GetActiveSelectButtons()
        {
            return selectButtons.Where(button => button.gameObject.activeSelf).ToList();
        }

        public void SetDim(bool enable)
        {
            float alpha = enable ? 0.4f : 0;
            _dimImage.DOFade(alpha, 0);
        }

        public async void UpdateContentLayout()
        {
            await UniTask.WaitForEndOfFrame();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
        }
    }
}

