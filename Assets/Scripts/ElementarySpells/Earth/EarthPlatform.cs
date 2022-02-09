using UnityEngine;

public class EarthPlatform : MonoBehaviour
{
    public static EarthPlatform instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one instance of EarthPlatform in the scene, the old one is destroyed");
            Destroy(instance.gameObject);
        }

        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
