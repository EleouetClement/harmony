using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDisk : EnnemySpell
{
    [Header("Casting stats")]
    [SerializeField] [Min(0)] int damagesGrowthPerSec;
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
        if (waterDiskInstance.hitted)
            DealDamages(waterDiskInstance.objectHitted);
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
            waterDiskInstance.transform.position = summonerPosition.position;
        }
        else
        {
            Fly();
        }
    }

    public override void Terminate()
    {
        if (waterDiskInstance)
            Destroy(waterDiskInstance.gameObject);
        Destroy(gameObject);

    }

    protected override void OnChargeEnd()
    {
        if (customTarget)
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
    }

    private void Fly()
    {
        Vector3 velocity = trajectory * speed * Time.deltaTime;
        waterDiskInstance.transform.Translate(velocity);
    }

    protected override void DealDamages(GameObject objectHitted)
    {
        base.DealDamages(objectHitted);
    }
}
