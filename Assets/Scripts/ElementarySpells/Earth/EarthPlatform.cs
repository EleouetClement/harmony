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
        else if(EarthPillar.instance != null)
        {
            Debug.LogWarning("There is more than one instance of EarthPillar in the scene, the old one is destroyed");
            Destroy(EarthPillar.instance.gameObject);
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
