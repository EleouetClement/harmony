using UnityEngine;

public class EarthPillar : MonoBehaviour
{
    public static EarthPillar instance;
    [Min(0.1f)] public float timeToSpawn;
    public bool isTotallyOut { get; private set; } = false;
    public ParticleSystem groundPillarMovingEffect;

    private float timer = 0f;
    private Vector3 initialSpawnPoint;
    private Vector3 finalSpawnPoint;
    private Cinemachine.CinemachineImpulseSource shakeSource;

    private void Awake()
    {
        instance = this;

        groundPillarMovingEffect.Play();

        // Initial and final point values for moving the pillar from bottom to top
        finalSpawnPoint = transform.position;
        initialSpawnPoint = new Vector3(transform.position.x, transform.position.y - transform.localScale.y, transform.position.z);
        transform.position += initialSpawnPoint;

        // Definition of the shake system (to have shocks as long as the pillar extends)
        shakeSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
        shakeSource.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = timeToSpawn / 4; // Time to start of shaking
        shakeSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = timeToSpawn - (timeToSpawn/4); // Time in the highest intensity of shaking
        shakeSource.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = timeToSpawn / 4; // Time to end of shaking
        shakeSource.GenerateImpulseAt(transform.position, transform.forward);
    }

    void FixedUpdate()
    {
        // Extension of the pillar if it is not totally out of the ground
        if (!isTotallyOut)
        {           
            timer += Time.fixedDeltaTime;
            transform.position = Vector3.Lerp(initialSpawnPoint, finalSpawnPoint, timer/timeToSpawn);

            if(timer >= timeToSpawn)
            {
                isTotallyOut = true;
                groundPillarMovingEffect.Stop();
            }
        }
    }
}
