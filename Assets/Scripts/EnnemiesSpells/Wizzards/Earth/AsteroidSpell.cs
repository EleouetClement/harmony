using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpell : EnnemySpell
{
    [Header("Casting stats")]
    [SerializeField] [Range(0, 0.1f)] float projectileAdditionnalScale;
    [SerializeField] [Min(0)] int damagesGrowthPerSec;
    private float projectileGrowthPerSec;

    //should be counted in number of hits according to the player health system
    [Header("behaviour stats")]
    [SerializeField] [Range(1, 4)] int baseDamages;
    [SerializeField] float speed;
    [SerializeField] float gravity = -18;
    [SerializeField] float height = 25;



    [Header("Do not change")]
    [SerializeField] AsteroidController rockReference;
    [SerializeField] bool debug = false;
    [SerializeField] float explosionradius;
    [SerializeField] LayerMask blastEffect;
    [SerializeField] LayerMask blastCheck;

    private AsteroidController rockInstance;

    private Vector3 trajectory = Vector3.zero;

    private Rigidbody rockBody;

    private void Awake()
    {
        if (!rockReference)
        {
            Debug.LogError("Ennemy earth Spell : no rock reference, spell cannot launch");
            Destroy(gameObject);
        }

    }

    public override void Charge(CastType chargeTime, Transform spellOrigin)
    {
        base.Charge(chargeTime, spellOrigin);
        Setup();
    }

    public override void Charge(CastType chargeTime, Transform spellOrigin, Vector3 targetPosition)
    {
        base.Charge(chargeTime, spellOrigin, targetPosition);
        Setup();
    }

    private void Setup()
    {
        rockInstance = Instantiate(rockReference, summonerPosition.position, Quaternion.identity);
        damagesDeal = new DamageHit(baseDamages, AbstractSpell.Element.Earth);
        rockBody = rockInstance.GetComponent<Rigidbody>();
        rockBody.useGravity = false;
        if (currentCast.Equals(CastType.charge))
            projectileGrowthPerSec = projectileAdditionnalScale / CastObjective;

    }

    private void Update()
    {
        if(rockInstance.hitted)
        {
            DealDamages(rockInstance.objectHitted);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(!charged)
        {
            rockInstance.transform.position = summonerPosition.position;
            rockInstance.transform.localScale += new Vector3(projectileGrowthPerSec, projectileGrowthPerSec, projectileGrowthPerSec);
            damagesDeal.damage += damagesGrowthPerSec;
        }
    }

    public override void Terminate()
    {
        if (rockInstance.gameObject)
            Destroy(rockInstance.gameObject);
        Destroy(gameObject);
    }

    protected override void OnChargeEnd()
    {
        rockBody.velocity = CalculateLaunchVelocity();
        rockBody.useGravity = true;
    }

    protected override void DealDamages(GameObject objectHitted)
    {
        base.DealDamages(objectHitted);
        Terminate();
    }

    Vector3 CalculateLaunchVelocity()
    {
        float displacementY = target.y - rockBody.position.y;
        Vector3 displacementXZ = new Vector3(target.x - rockBody.position.x, 0, target.z - rockBody.position.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity));
        return velocityXZ + velocityY;
    }
}
