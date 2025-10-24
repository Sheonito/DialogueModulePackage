using UnityEngine.UI;

namespace Aftertime.HappinessBlossom.UI
{
    public interface INavigationGroup
    {
        public void ActiveNavigation(Navigation activeNaviation);
        public void InActiveNavigation(Navigation inActiveNavigation);
    }
   
}