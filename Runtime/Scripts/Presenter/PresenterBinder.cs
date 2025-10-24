using System.Linq;
using UnityEngine;

namespace Aftertime.StorylineEngine
{
    [DisallowMultipleComponent]
    public sealed class PresenterBinder : MonoBehaviour
    {
        private IPresenter[] _presenters;

        private void Awake()
        {
            CachePresenters();
        }

        private void OnEnable()
        {
            CachePresenters();
            foreach (var presenter in _presenters)
                PresenterRegistry.Register(presenter);
        }

        private void OnDisable()
        {
            if (_presenters == null) return;
            foreach (var p in _presenters)
                PresenterRegistry.Unregister(p);
        }

        private void CachePresenters()
        {
            _presenters = GetComponents<MonoBehaviour>().OfType<IPresenter>().ToArray();
        }
    }
}
