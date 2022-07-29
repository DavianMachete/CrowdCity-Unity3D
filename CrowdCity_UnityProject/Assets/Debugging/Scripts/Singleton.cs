using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    private static object _lock = new object();

    private static bool _isQuitting = false;

    public static T instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        // Est-que vous parlez en francais
                        Debug.LogWarning("Il y a plusieurs managers du meme type sur la scène : " + typeof(T).Name);
                        return _instance;
                    }

                    if (_instance == null && !_isQuitting) return null;
                }

                return _instance;
            }
        }
    }

    public static void SetActive(bool _Active)
    {
        instance.gameObject.SetActive(_Active);
    }

    void OnDestroy()
    {
        OnDestroySpecific();
    }

    void OnApplicationQuit()
    {
        _isQuitting = true;
    }

    protected virtual void OnDestroySpecific() { }
}
