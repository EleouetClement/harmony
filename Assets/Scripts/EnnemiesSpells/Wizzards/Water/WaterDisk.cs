using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDisk : EnnemySpell
{
    [Header("Casting stats")]
    [SerializeField] [Min(0)] int damagesGrowthPerSec;
    [SerializeField] [Min(1)] int diskNumber;
    [Header("behaviour stats")]
    [SerializeField] [Range(1, 4)] int baseDamages;
    [SerializeField] float speed;
    

    [Header("Do not change")]
    [SerializeField] DiskManager waterDiskReference;
    [SerializeField] [Min(0)] float diskDelayInSeconds;
    [SerializeField] bool debug = false;
    

    private DiskManager waterDiskInstance;

    private Vector3 trajectory = Vector3.zero;

    private int diskCount;
    private float startCoolDown = 0.0f;
    private bool allLaunched = false;

    List<DiskManager> allDisks;

    private void Awake()
    {
        if (!waterDiskReference)
        {
            Debug.LogError("Ennemy water spell : no water disk reference, spell cannot launch");
            Destroy(gameObject);
        }
        allDisks = new List<DiskManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if(currentCast.Equals(CastType.charge) && charged)
        {
            if(startCoolDown < diskDelayInSeconds)
            {
                startCoolDown += Time.deltaTime;
            }
            else
            {
                if(!allLaunched)
                {
                    StartDisks();
                }
            }
        }
        foreach (DiskManager d in allDisks)
        {
            if (d.hitted)
            {
                d.hitted = false;
                DealDamages(d.objectHitted);
            }
        }
            
    }

    public override void Charge(CastType chargeTime, Transform spellOrigin)
    {
        base.Charge(chargeTime, spellOrigin);
        allDisks.Add(Instantiate(waterDiskReference, summonerPosition.position, Quaternion.identity));
        damagesDeal = new DamageHit(baseDamages, AbstractSpell.Element.Water);
    }

    public override void Charge(CastType chargeTime, Transform spellOrigin, Vector3 targetPosition)
    {
        base.Charge(chargeTime, spellOrigin, targetPosition);
        allDisks.Add(Instantiate(waterDiskReference, summonerPosition.position, Quaternion.identity));
        damagesDeal = new DamageHit(baseDamages, AbstractSpell.Element.Water);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(!charged || !allLaunched)
        {
            //Nothing to ad yet for the casting
            foreach (DiskManager d in allDisks)
            {
                if(!d.lauched)
                    d.transform.position = summonerPosition.position;
            }
            if(currentCast.Equals(CastType.charge) && allDisks.Count < diskNumber - 1)
            {
                allDisks.Add(Instantiate(waterDiskReference, summonerPosition.position, Quaternion.identity));
            }         
        }
        else
        {
            Fly();
        }
    }

    public override void Terminate()
    {
        if (allDisks.Count > 0)
        {
            foreach(DiskManager d in allDisks)
            {
                Destroy(d.gameObject);
            }
        }
            
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
        StartDisks();
        if (debug)
            Debug.DrawRay(summonerPosition.position, trajectory * 200, Color.red, 10);
    }

    private void Fly()
    {
        Vector3 velocity = trajectory * speed * Time.deltaTime;
        //waterDiskInstance.transform.Translate(velocity);
        int pouet = 0;
        foreach(DiskManager disk in allDisks)
        {
            if(disk.lauched)
            {
                disk.transform.Translate(velocity);
                Debug.Log(pouet++);
            }
            else
            {
                Debug.Log("Launched false");
            }
                
        }
    }

    protected override void DealDamages(GameObject objectHitted)
    {
        base.DealDamages(objectHitted);

    }

    private void StartDisks()
    {
        
        if(diskCount < allDisks.Count)
        {
            Debug.Log("Launching a disk : " + diskCount);
            allDisks[diskCount++].SetLaunch();
            startCoolDown = 0.0f;
        }
        else
        {
            allLaunched = true;
        }
    }

}
