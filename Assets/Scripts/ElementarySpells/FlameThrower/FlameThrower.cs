using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : AbstractSpell
{
    [Header("Visual")]
    [SerializeField] private GameObject flameEffectPrefab;
    [Header("Stats")]
    [SerializeField] [Min(0)] private float duration;
    [SerializeField] [Min(0)] private float destroyTimingOffset;
    [SerializeField] [Min(0)] private float SpeedOverTime;
    [SerializeField] [Min(0)] float knockBackPower;
    [SerializeField] [Min(0)] float colliderActivationTime;

    
    
    private Collider coll;
    private bool setTimer = false;
    private ParticleSystem flamesFlow;
    private GameModeSingleton gm;
    private GameObject flameEffectReference;
    private float timer = Mathf.Epsilon;

    private void Awake()
    {       
        if (flameEffectPrefab == null)
        {
            Debug.LogError("No Flame Effect linked");
            Destroy(gameObject);
        }
        gm = GameModeSingleton.GetInstance();
    }

    private void Update()
    {
        if(setTimer)
        {
            timer += Time.deltaTime;
        }
        if (timer >= colliderActivationTime && flameEffectReference)
        {
            coll.enabled = true;
        }
        //if (timer > (duration + destroyTimingOffset) && !flamesFlow.isStopped)
        //{
        //    Terminate();
        //}
        if (!flameEffectReference)
            Terminate();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="elemRef"></param>
    /// <param name="target">Camera forward vector</param>
    public override void init(GameObject elemRef, Vector3 target)
    {
        base.init(elemRef, target);
        flameEffectReference = Instantiate(flameEffectPrefab, elemRef.transform.position, Quaternion.identity);
        coll = flameEffectReference.GetComponent<BoxCollider>();
        coll.enabled = false;
        flamesFlow = flameEffectReference.GetComponent<ParticleSystem>();
        flamesFlow.Stop();
        var pouet = flamesFlow.main;
        pouet.duration = duration;
        pouet.startSpeed = SpeedOverTime;
        pouet = flamesFlow.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().main;
        pouet.duration = duration;

        flameEffectReference.transform.rotation = CalculateTrajectory();
        flamesFlow.Play();
        setTimer = true;
    }


    public override void Terminate()
    {
        elementary.GetComponent<ElementaryController>().Reset();
        //Destroy(flameEffectReference);
        Destroy(gameObject);
    }

    private Quaternion CalculateTrajectory()
    {
        Transform newTransform;
        newTransform = elementary.transform;
        newTransform.rotation = Quaternion.LookRotation(gm.GetCinemachineCameraController.GetViewDirection);
        return newTransform.rotation;
    }

}
