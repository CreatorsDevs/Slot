using UnityEngine;

namespace Generics
{
    //Non Persistant Singleton Generic Class
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance { get { return _instance; } }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                _instance = this as T;
        }
        private void OnDestroy() => _instance = null;
    }
}
