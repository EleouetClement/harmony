using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireSpell : EnnemySpell
{

    [SerializeField] EnnemiFireBall fireOrbReference;
    [SerializeField][Min(0)] float projectileGrowthPerSec;
    [SerializeField][Min(0)] float damagesGrowthPerSec;

    //should be counted in number of hits according to the player health system
    [SerializeField][Range(1, 4)] int baseDamages;
    private EnnemiFireBall fireOrbInstance;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Charge(float chargeTime, Transform spellOrigin)
    {
        base.Charge(chargeTime, spellOrigin);
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

        }
    }
}
