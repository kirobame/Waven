using UnityEngine;

public class Music : MonoBehaviour
{
    private static Music instance;
    
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        if (instance != this) return;
        instance = null;
    }
}