using UnityEngine.UI;

namespace Lucecita.HappinessBlossom.UI
{
    public interface INavigationGroup
    {
        public void ActiveNavigation(Navigation activeNaviation);
        public void InActiveNavigation(Navigation inActiveNavigation);
    }
   
}