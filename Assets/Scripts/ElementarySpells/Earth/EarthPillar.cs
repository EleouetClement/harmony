using UnityEngine;

public class EarthPillar : MonoBehaviour
{
    public static EarthPillar instance;
    [Min(1)] public float speedSpawn;

    private Vector3 finalSpawnPoint;
    public bool isTotallyOut { get; private set; } = false;

    private void Awake()
    {
        // Initial and final point values for moving the pillar from bottom to top
        finalSpawnPoint = transform.position;
        transform.position += new Vector3(0f, -transform.localScale.y, 0f);

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

    void FixedUpdate()
    {
        // Extension of the pillar if it is not totally out of the ground
        if (!isTotallyOut)
        {
            transform.position += new Vector3(0f, speedSpawn * Time.fixedDeltaTime, 0f);

            // If the pillar has finished expanding
            if (transform.position.y >= finalSpawnPoint.y)
            {
                isTotallyOut = true;
            }
        }
    }
}
