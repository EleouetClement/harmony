using UnityEngine;

public class EarthPillar : MonoBehaviour
{
    public static EarthPillar instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one instance of EarthPillar in the scene, the old one is destroyed");
            Destroy(instance.gameObject);
        }
        else if(EarthPlatform.instance != null)
        {
            Debug.LogWarning("There is more than one instance of EarthPlatform in the scene, the old one is destroyed");
            Destroy(EarthPlatform.instance.gameObject);
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
