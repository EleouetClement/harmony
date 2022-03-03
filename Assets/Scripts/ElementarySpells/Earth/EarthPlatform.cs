using UnityEngine;

public class EarthPlatform : MonoBehaviour
{
    public static EarthPlatform instance;
    [Min(0.1f)] public float timeToSpawn;
    public ParticleSystem wallPlatformMovingEffect;

    public bool isTotallyOut { get; private set; } = false;
    private float scaleAxeX;
    private float scaleAxeZ;

    private float timer = 0f;
    private Vector3 initialSpawnScale;
    private Vector3 finalSpawnScale;
    private ParticleSystem.ShapeModule particleShape;
    private Cinemachine.CinemachineImpulseSource shakeSource;

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

        // Expand the particle system
        particleShape = wallPlatformMovingEffect.shape;
        particleShape.scale = new Vector3(0f, wallPlatformMovingEffect.shape.scale.y, 0f);
        wallPlatformMovingEffect.Play();

        // Store the prefab scale to make it expand (initial and final scale values for expanding the platform)
        scaleAxeX = transform.localScale.x;
        scaleAxeZ = transform.localScale.z;
        finalSpawnScale = new Vector3(scaleAxeX, transform.localScale.y, scaleAxeZ);
        initialSpawnScale = new Vector3(0f, transform.localScale.y, 0f); // --> From scale 0 to the prefab scale
        transform.localScale = initialSpawnScale;

        // Definition of the shake system (to have shocks as long as the pillar extends)
        shakeSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
        shakeSource.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = timeToSpawn / 4; // Time to start of shaking
        shakeSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = timeToSpawn - (timeToSpawn / 4); // Time in the highest intensity of shaking
        shakeSource.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = timeToSpawn / 4; // Time to end of shaking
        shakeSource.GenerateImpulseAt(transform.position, transform.forward);
    }

    void FixedUpdate()
    {
        // Extension of the platform if it is not totally out of the wall
        if (!isTotallyOut)
        {
            timer += Time.fixedDeltaTime;
            transform.localScale = Vector3.Lerp(initialSpawnScale, finalSpawnScale, timer / timeToSpawn);
            particleShape.scale = Vector3.Lerp(initialSpawnScale, finalSpawnScale, timer / timeToSpawn);

            if (timer >= timeToSpawn)
            {
                isTotallyOut = true;
                wallPlatformMovingEffect.Stop();
            }
        }
    }
}
