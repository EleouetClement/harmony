using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDisk : EnnemySpell
{
    [Header("Casting stats")]
    [SerializeField] [Min(0)] int damagesGrowthPerSec;
    [SerializeField] [Min(0)] int bounsNumber;
    [Header("behaviour stats")]
    [SerializeField] [Range(1, 4)] int baseDamages;
    [SerializeField] float speed;

    [Header("Do not change")]
    [SerializeField] DiskManager waterDiskReference;
    [SerializeField] bool debug = false;

    private DiskManager waterDiskInstance;

    private Vector3 trajectory = Vector3.zero;

    private void Awake()
    {
        if (!waterDiskReference)
        {
            Debug.LogError("Ennemy water spell : no water disk reference, spell cannot launch");
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Charge(CastType chargeTime, Transform spellOrigin)
    {
        base.Charge(chargeTime, spellOrigin);
        waterDiskInstance = Instantiate(waterDiskReference, summonerPosition.position, Quaternion.identity);
        damagesDeal = new DamageHit(baseDamages, AbstractSpell.Element.Water);
    }

    public override void Charge(CastType chargeTime, Transform spellOrigin, Vector3 targetPosition)
    {
        base.Charge(chargeTime, spellOrigin, targetPosition);
        waterDiskInstance = Instantiate(waterDiskReference, summonerPosition.position, Quaternion.identity);
        damagesDeal = new DamageHit(baseDamages, AbstractSpell.Element.Water);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(!charged)
        {
            //Nothing to ad yet for the casting
        }
    }

    public override void Terminate()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnChargeEnd()
    {
        throw new System.NotImplementedException();
        //Trajectory calculation to do
    }

    private void Fly()
    {

    }

    protected override void DealDamages(GameObject objectHitted)
    {
        base.DealDamages(objectHitted);
    }
}
