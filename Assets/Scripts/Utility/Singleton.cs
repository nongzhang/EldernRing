using UnityEngine;

namespace NZ.Utility
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    SetupInstance();
                }
                return instance;
            }
        }

        public virtual void Awake()
        {
            RemoveDuplicates();
        }

        private static void SetupInstance()
        {
            instance = (T)FindObjectOfType(typeof(T));
            if (instance == null)
            {
                GameObject gameObject = new GameObject();
                gameObject.name = typeof(T).Name;
                instance = gameObject.AddComponent<T>();
                DontDestroyOnLoad(gameObject);
            }
        }

        private void RemoveDuplicates()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
