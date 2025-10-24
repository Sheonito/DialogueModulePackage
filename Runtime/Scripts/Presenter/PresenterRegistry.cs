using System;
using System.Collections.Generic;

namespace Aftertime.StorylineEngine
{
    public static class PresenterRegistry
    {
        private static readonly Dictionary<Type, object> _presenters = new();

        public static void Register(IPresenter presenter)
        {
            if (presenter == null) return;
            _presenters[presenter.GetType()] = presenter;
        }

        public static void Unregister(IPresenter presenter)
        {
            if (presenter == null) return;
            _presenters.Remove(presenter.GetType());
        }

        public static T GetPresenter<T>() where T : class, IPresenter
        {
            var result = Resolve(typeof(T));
            return result as T;
        }

        public static object Resolve(Type requestedType)
        {
            if (requestedType == null) return null;
            if (_presenters.TryGetValue(requestedType, out var exact))
                return exact;

            foreach (var kv in _presenters)
            {
                if (requestedType.IsAssignableFrom(kv.Key))
                    return kv.Value;
            }
            return null;
        }
    }
}
