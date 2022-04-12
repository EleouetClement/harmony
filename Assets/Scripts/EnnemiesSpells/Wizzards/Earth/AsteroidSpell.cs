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
    [SerializeField] [Min(0)] float time = 1;



    [Header("Do not change")]
    [SerializeField] GameObject rockReference;
    [SerializeField] bool debug = false;
    [SerializeField] float explosionradius;
    [SerializeField] float aimOffset = 1;
    [SerializeField] LayerMask blastEffect;
    [SerializeField] LayerMask blastCheck;

    private AsteroidController rockInstance;
    private GameObject rock;

    private Vector3 launchVelocity = Vector3.zero;

    private Rigidbody rockBody;

    TrajectoryCalculator pouet;

    private void Awake()
    {
        if (!rockReference)
        {
            Debug.LogError("Ennemy earth Spell : no rock reference, spell cannot launch");
            Destroy(gameObject);
        }
        pouet = new TrajectoryCalculator();

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
        rock = Instantiate(rockReference, summonerPosition.position, Quaternion.identity);
        rockInstance = rock.GetComponentInChildren<AsteroidController>();
        damagesDeal = new DamageHit(baseDamages, AbstractSpell.Element.Earth);
        rockBody = rockInstance.GetComponentInChildren<Rigidbody>();
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
            
            launchVelocity = pouet.CalculateVelocity(summonerPosition.position, DefaultTarget.position, time);
            //Debug.Log(rock.name + " : " + Quaternion.Slerp(rock.transform.rotation, Quaternion.LookRotation(launchVelocity), Time.deltaTime * 5));
            rock.transform.rotation = Quaternion.Slerp(rock.transform.rotation, Quaternion.LookRotation(launchVelocity), Time.deltaTime * 5);
        }
        else
        {
            rock.transform.rotation = Quaternion.Slerp(rock.transform.rotation, Quaternion.LookRotation(rockBody.velocity.normalized), Time.deltaTime * 5);
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
        Vector3 tempTarget = DefaultTarget.position + new Vector3(0, aimOffset, 0);
        rockBody.velocity = pouet.CalculateVelocity(summonerPosition.position, DefaultTarget.position, time);
        //rockBody.velocity = pouet.CalculateVelocity(summonerPosition.position, tempTarget, time);
        rockBody.useGravity = true;
    }

    protected override void DealDamages(GameObject objectHitted)
    {
        base.DealDamages(objectHitted);
        Terminate();
    }
}
