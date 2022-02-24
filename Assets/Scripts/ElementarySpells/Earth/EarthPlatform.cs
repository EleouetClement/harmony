using UnityEngine;

public class EarthPlatform : MonoBehaviour
{
    public static EarthPlatform instance;
    [Min(1)] public float speedSpawn;

    private bool isTotallyOut = false;
    private float scaleAxeX;
    private float scaleAxeZ;

    private void Awake()
    {
        // Store the prefab scale to make it expand
        scaleAxeX = transform.localScale.x;
        scaleAxeZ = transform.localScale.z;

        // --> From scale 0 to the prefab scale
        transform.localScale = new Vector3(0f, transform.localScale.y, 0f);

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

    void FixedUpdate()
    {
        // Extension of the platform if it is not totally out of the wall
        if (!isTotallyOut)
        {
            transform.localScale += new Vector3(scaleAxeX, 0f, scaleAxeZ) * speedSpawn * Time.fixedDeltaTime;

            // If the platform has finished expanding
            if(transform.localScale.x >= scaleAxeX && transform.localScale.z >= scaleAxeZ)
            {
                isTotallyOut = true;
            }
        }
    }
}
