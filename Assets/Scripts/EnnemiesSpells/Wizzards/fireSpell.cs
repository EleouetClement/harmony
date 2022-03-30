using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireSpell : EnnemySpell
{

    

    [Header("Casting stats")]
    [SerializeField][Range(0, 0.5f)] float projectileGrowthPerSec;
    [SerializeField][Min(0)] int damagesGrowthPerSec;

    //should be counted in number of hits according to the player health system
    [SerializeField][Range(1, 4)] int baseDamages;
    [SerializeField] float speed;
    [Header("Do not change")]
    [SerializeField] EnnemiFireBall fireOrbReference;
    
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


    public override void Charge(float chargeTime, Transform spellOrigin)
    {
        base.Charge(chargeTime, spellOrigin);
        fireOrbInstance = Instantiate(fireOrbReference, summonerPosition.position, Quaternion.identity);
        damagesDeal = new DamageHit(baseDamages, AbstractSpell.Element.Fire);
    }
    public override void Charge(float chargeTime, Transform spellOrigin, Vector3 targetPosition)
    {
        base.Charge(chargeTime, spellOrigin, targetPosition);
        fireOrbInstance = Instantiate(fireOrbReference, summonerPosition.position, Quaternion.identity);
        damagesDeal = new DamageHit(baseDamages, AbstractSpell.Element.Fire);
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(!charged)
        {
            fireOrbInstance.gameObject.transform.localScale += new Vector3(projectileGrowthPerSec, projectileGrowthPerSec, projectileGrowthPerSec);
            damagesDeal.damage += damagesGrowthPerSec;        
        }
        else
        {
            Fly();
        }
        
    }

    protected override void OnChargeEnd()
    {
        trajectory = target - summonerPosition.position;
        trajectory.Normalize();
        damagesDeal.direction = trajectory;
    }

    private void Fly()
    {
        Vector3 velocity = target * speed * Time.deltaTime;
        fireOrbInstance.transform.Translate(velocity);
    }

    public override void Terminate()
    {
        Destroy(fireOrbInstance);
        Destroy(gameObject);
    }


}
