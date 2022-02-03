using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : AbstractSpell
{

    [Header("FireBall skin")]
    public GameObject FireballPrefab;

    [Header("Fireball stats")]
    /// <summary>
    /// The maximum distance before applying bullet drop
    /// </summary>
    [Min(1)]public float maxDistance;
    [Min(1)]public float livingTime;
    /// <summary>
    /// Distance from which the projectile reach its max speed
    /// </summary>
    [Min(0)] public float maxSpeedDistance;

    [SerializeField] [Min(0)] private float projectileTopSpeed;
    [SerializeField] [Min(0)] private float projectileStartSpeed;

    [Header("Projectile drop")]
    /// <summary>
    /// Force needed to be applied when bullet drop is on.
    /// it increases by 
    /// </summary>
    [SerializeField] [Range(-1, 0)] private float gravityForce;

    /// <summary>
    /// factor that is add to gravity each frame when forces are on.
    /// </summary>
    [SerializeField] [Min(0)] private float gravityIncreaseFactor;
    /// <summary>
    /// Store the origin position of the fireOrb before any translation
    /// </summary>
    private Vector3 origin;

    /// <summary>
    /// Store velocity for each frame
    /// </summary>
    private Vector3 velocity;

    /// <summary>
    /// Store the current fire ball object reference to apply the transforms
    /// </summary>
    private GameObject fireOrbInstance;

    private float speedStep;
    private float currentSpeed;
    public Fireball()
    {
        velocity = Vector3.zero;//Might be useless
        

    }
    /// <summary>
    /// Moves the fireball following the AbstractSpell target direction
    /// </summary>
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if(currentSpeed < projectileTopSpeed)
        {
            currentSpeed += speedStep;
            
        }
        Debug.Log(currentSpeed);
        if(Vector3.Distance(origin, transform.position) >= maxDistance)
        {
            ApplyForces();
        }
        velocity = target * currentSpeed * Time.fixedDeltaTime;
        fireOrbInstance.transform.Translate(velocity);
        elementary.transform.Translate(velocity);

    }
    

    /// <summary>
    /// Apply custom gravity on y direction axis
    /// </summary>
    private void ApplyForces()
    {
        target += new Vector3(0.0f, gravityForce, 0.0f);
        target.Normalize();
        gravityForce -= gravityIncreaseFactor;
    }

    public override void init(GameObject elemRef, Vector3 target)
    {
        base.init(elemRef, target.normalized);
        if (FireballPrefab == null)
        {
            Debug.LogError("Fireball() : FireBall skin missing");
        }
        else
        {
            fireOrbInstance = Instantiate(FireballPrefab, transform.position, Quaternion.identity);
            origin = fireOrbInstance.transform.position;
            elementary.GetComponent<ElementaryController>().computePosition = false;
            if(projectileStartSpeed > projectileTopSpeed)
            {
                Debug.LogWarning("Fireball.init : projectileStartSpeed > projectiletTopSpeed");
                currentSpeed = projectileTopSpeed;
            }
            else
            {
                speedStep = projectileTopSpeed / maxSpeedDistance;
                currentSpeed = projectileStartSpeed;
            }
            
        }
    }
    /// <summary>
    /// Detroys fire Orbs and resets Elementary
    /// </summary>
    public override void Terminate()
    {
        Destroy(fireOrbInstance);
        ElementaryController elemCtrl = elementary.GetComponent<ElementaryController>();
        elemCtrl.currentSpell = null;
        elemCtrl.computePosition = true;
    }

    protected override void onChargeEnd(float chargetime)
    {
        //if(chargetime >= maxCastTime)
        //{
        //    //TO DO...
        //}
        //else
        //{
        //    //TO DO...
        //}
    }
}
