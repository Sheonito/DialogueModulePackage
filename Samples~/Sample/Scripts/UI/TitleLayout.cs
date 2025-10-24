using System;
using Lucecita.HappinessBlossom.UI.Layout;
using UnityEngine;
using UnityEngine.UI;

namespace Lucecita
{
    public class TitleLayout : LayoutBase
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private StoryRunner _storyRunner;

        protected override void Start()
        {
            base.Start();
            _startButton.onClick.AddListener(_storyRunner.Run);
        }
    }
   
}