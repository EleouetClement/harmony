using System;
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
    


    [Header("Projectile Casting upgrades")]
    /// <summary>
    /// by how many units the projectile size will increase per frame
    /// </summary>
    [SerializeField] [Range(0, 0.1f)] private float projectileGrowth;
    [SerializeField] [Min(0)] private float projectileMaxSize;
    [SerializeField] [Min(0)] private float projectileTopSpeedGrowth;
    [SerializeField] [Min(0)] private float projectileMaxDistanceGrowth;
    [SerializeField] [Min(0)] private float perfectCastTiming;
   

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

    [Header("CrossAir infos")]
    [SerializeField] [Range(50, 250)] private float reticleSize;
    [SerializeField] [Range(50, 250)] private float reticleMinimumSize;
    [SerializeField] [Min(0)] private float reticleDiminutionSpeed;
    [SerializeField] private GameObject crossAirPrefab;
    /// <summary>
    /// New fov value during aiming
    /// </summary>
    [SerializeField] [Min(0)] private float zoomPower;

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
    private bool launched = false;
    private bool isExplosive = false;

    private ElementaryController elem;
    public Fireball()
    {
        velocity = Vector3.zero;//Might be useless
    }
    /// <summary>
    /// Moves the fireball following the AbstractSpell target direction
    /// </summary>
    public override void FixedUpdate()
    {
        //Debug.Log(Vector3.Distance(origin, fireOrbInstance.transform.position));
        base.FixedUpdate();
        if(!isReleased())
        {
            if(fireOrbInstance.transform.localScale.x < projectileMaxSize)
            {
                fireOrbInstance.transform.localScale += new Vector3(projectileGrowth, projectileGrowth, projectileGrowth);
            }
            projectileTopSpeed += projectileTopSpeedGrowth;
            maxDistance += projectileMaxDistanceGrowth;
        }
        if(launched)
        {
            Move();
        }
        
    }
    private void Move()
    {
        if (currentSpeed < projectileTopSpeed)
        {
            currentSpeed += speedStep;

        }
        if (Vector3.Distance(origin, fireOrbInstance.transform.position) >= maxDistance)
        {
            ApplyForces();
        }

        velocity = target * currentSpeed * Time.deltaTime;
        fireOrbInstance.transform.Translate(velocity);
        elementary.transform.Translate(velocity);
    }

    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        Vector2 size = new Vector2(reticleSize, reticleSize);
        marker.DisplayTarget(size, Vector3.zero);
        if(reticleSize > reticleMinimumSize)
        {
            reticleSize -= reticleDiminutionSpeed * Time.deltaTime;
        }
        

    }
    /// <summary>
    /// Apply custom gravity on y direction axis
    /// </summary>
    private void ApplyForces()
    {
        target += new Vector3(0.0f, gravityForce * Time.fixedDeltaTime, 0.0f);
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
            elem = elementary.GetComponent<ElementaryController>();
            elem.computePosition = false;
            if (projectileStartSpeed > projectileTopSpeed)
            {
                Debug.LogWarning("Fireball.init : projectileStartSpeed > projectiletTopSpeed");
                currentSpeed = projectileTopSpeed;
            }
            else
            {
                speedStep = projectileTopSpeed / maxSpeedDistance;
                currentSpeed = projectileStartSpeed;
            }

            GameObject tmp = Instantiate(crossAirPrefab, Vector3.zero, Quaternion.identity);
            marker = tmp.GetComponent<CrossAir>();
            elem.playerCameraController.Aim(zoomPower);

        }
    }
    /// <summary>
    /// Detroys fire Orbs and resets Elementary
    /// </summary>
    public override void Terminate()
    {
        Destroy(fireOrbInstance);
        elem.currentSpell = null;
        elem.computePosition = true;
        Destroy(gameObject);
    }

    protected override void onChargeEnd(float chargetime)
    {
        base.onChargeEnd(chargetime);
        launched = true;
        target = elem.playerCameraController.GetViewDirection;        
        Destroy(marker.gameObject);
        if(isBlinked)
        {
            Debug.Log("Molotov à appliquer");
            isExplosive = true;
        }
        elem.playerCameraController.StopAim();
    }
}
