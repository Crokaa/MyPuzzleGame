using UnityEngine;

public class EssentialObjects : MonoBehaviour
{
    public static EssentialObjects instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
