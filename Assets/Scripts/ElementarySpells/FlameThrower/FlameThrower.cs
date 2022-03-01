using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : AbstractSpell
{
    [Header("Visual")]
    [SerializeField] private GameObject flameEffectPrefab;
    [Header("Stats")]
    [SerializeField] [Min(0)] private float duration;
    [SerializeField] [Min(0)] private float SpeedOverTime;
    [SerializeField] [Min(0)] float knockBackPower;
    [SerializeField] [Min(0)] float colliderActivationTime;

    private Collider coll;
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
        if(timer >= colliderActivationTime)
        {
            coll.enabled = true;
        }
        else
        {
            timer += Time.deltaTime;
        }
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
        flamesFlow.transform.TransformDirection(CalculateTrajectory()); 
        flamesFlow.Play();
    }


    public override void Terminate()
    {
        elementary.GetComponent<ElementaryController>().Reset();
        Destroy(flameEffectReference);
        Destroy(gameObject);
    }

    private Vector3 CalculateTrajectory()
    {
        return gm.GetCinemachineCameraController.GetViewDirection;
    }

}
