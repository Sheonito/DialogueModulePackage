using System.Collections.Generic;
using System.Linq;
using Aftertime.StorylineEngine;
using UnityEngine;

namespace Aftertime.HappinessBlossom.UI.Layout
{
    public class LayoutManager : SingletonMonoBehaviour<LayoutManager>
    {
        [SerializeField] private List<LayoutBase> layouts;

        public override void Initialize()
        {
        }

        public void Show<T>() where T : LayoutBase
        {
            LayoutBase layout = layouts.FirstOrDefault(layout => layout.GetType() == typeof(T));
            if (layout != null)
                layout.Show();
        }

        public void Hide<T>() where T : LayoutBase
        {
            LayoutBase layout = layouts.FirstOrDefault(layout => layout.GetType() == typeof(T));
            if (layout != null)
                layout.Hide();
        }

        public LayoutBase GetLayout<T>() where T : LayoutBase
        {
            LayoutBase layout = layouts.FirstOrDefault(layout => layout.GetType() == typeof(T));
            if (layout != null)
                return layout;
            else
                return null;
        }
    }
}