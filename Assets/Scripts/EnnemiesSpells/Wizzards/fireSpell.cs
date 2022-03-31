using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireSpell : EnnemySpell
{

    

    [Header("Casting stats")]
    [SerializeField][Range(0, 0.1f)] float projectileAdditionnalScale;
    [SerializeField][Min(0)] int damagesGrowthPerSec;
    private float projectileGrowthPerSec;

    //should be counted in number of hits according to the player health system
    [Header("behaviour stats")]
    [SerializeField][Range(1, 4)] int baseDamages;
    [SerializeField] float speed;
    

    [Header("Do not change")]
    [SerializeField] EnnemiFireBall fireOrbReference;
    [SerializeField] bool debug = false;
    [SerializeField] float explosionradius;
    [SerializeField] LayerMask blastEffect;
    [SerializeField] LayerMask blastCheck;

    private EnnemiFireBall fireOrbInstance;

    private Vector3 trajectory = Vector3.zero;

    private void Awake()
    {
        if(!fireOrbReference)
        {
            Debug.LogError("Ennemy fire Spell : no fire orb reference, spell cannot launch");
            Destroy(gameObject);
        }
        
    }

    public override void Charge(CastType chargeTime, Transform spellOrigin)
    {
        base.Charge(chargeTime, spellOrigin);     
        fireOrbInstance = Instantiate(fireOrbReference, summonerPosition.position, Quaternion.identity);
        damagesDeal = new DamageHit(baseDamages, AbstractSpell.Element.Fire);
        if(currentCast.Equals(CastType.charge))
        {
            projectileGrowthPerSec = projectileAdditionnalScale / CastObjective;
        }
            
    }
    public override void Charge(CastType chargeTime, Transform spellOrigin, Vector3 targetPosition)
    {
        base.Charge(chargeTime, spellOrigin, targetPosition);
        fireOrbInstance = Instantiate(fireOrbReference, summonerPosition.position, Quaternion.identity);
        damagesDeal = new DamageHit(baseDamages, AbstractSpell.Element.Fire);
        if (currentCast.Equals(CastType.charge))
            projectileGrowthPerSec = projectileAdditionnalScale / CastObjective;
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(!charged)
        {
            fireOrbInstance.transform.position = summonerPosition.position;
            fireOrbInstance.gameObject.transform.localScale += new Vector3(projectileGrowthPerSec, projectileGrowthPerSec, projectileGrowthPerSec);
            damagesDeal.damage += damagesGrowthPerSec;        
        }
        else
        {
            Fly();
        }
        if(fireOrbInstance.hitted)
        {
            DealDamages(fireOrbInstance.objectHitted);
        }
        
    }

    protected override void OnChargeEnd()
    {
        if(customTarget)
        {        
            trajectory = target - summonerPosition.position;
        }
        else
        {
            trajectory = DefaultTarget.position - summonerPosition.position;
        }
        trajectory.Normalize();
        if (debug)
            Debug.DrawRay(summonerPosition.position, trajectory * 200, Color.red, 10);     
        if(currentCast.Equals(CastType.charge))
        {
            damagesDeal.direction = trajectory;
            fireOrbInstance.explosionOn = true;
        }         
    }

    private void Fly()
    {
        Vector3 velocity = trajectory * speed * Time.deltaTime;
        fireOrbInstance.transform.Translate(velocity);
    }

    public override void Terminate()
    {
        if(fireOrbInstance.gameObject == null)
            Destroy(fireOrbInstance.gameObject);
        Destroy(gameObject);
    }

    protected override void DealDamages(GameObject objectHitted)
    {              
        if(currentCast.Equals(CastType.charge))
        {
            //fireOrbInstance.transform.position + Vector3.down * 3, transform.position + Vector3.up * 3, explosionradius, 1 << HarmonyLayers.LAYER_TARGETABLE
            Collider[] mightBePlayer = Physics.OverlapSphere(fireOrbInstance.transform.position, explosionradius, blastEffect);
            if (mightBePlayer.Length >= 1)
            {
                Debug.Log("fireSpell : stuff in Blast zone");
                foreach (Collider c in mightBePlayer)
                {
                    Debug.Log("fireSpell : " + c.name);
                    if(IsVisible(c))
                        c.gameObject.GetComponent<IDamageable>()?.OnDamage(damagesDeal);
                }
            }              
        }
        else
        {
            base.DealDamages(objectHitted);
        }
        Terminate();
    }

    private bool IsVisible(Collider collidedItem)
    {
        Vector3 direction = collidedItem.transform.position - fireOrbInstance.transform.position;
        direction.Normalize();
        RaycastHit hit;
        if (Physics.Raycast(fireOrbInstance.transform.position, direction * explosionradius, out hit, blastCheck))
        {
            return (hit.collider.GetComponent<IDamageable>() != null ? true : false);
        }
        return false;
    }

}
