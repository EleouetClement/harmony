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
    [SerializeField] [Min(1)] public float maxDistance;
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


    [Header("Debug")]
    [SerializeField] bool debug = false;
    [SerializeField] GameObject virtualTargetPrefab;
    private float aimDistance = 2000;

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
    private GameObject virtualTargetReference;
    private float speedStep;
    private float currentSpeed;
    private bool launched = false;
    private bool isExplosive = false;
    private GameModeSingleton gameManager;
    private ElementaryController elem;
    private int lastshrapnelspawn = 3;
    private bool trajcalculated = false;

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
        if (elementaryfollow) {
            fireOrbInstance.transform.position = elementary.transform.position;
        }
        if(fireOrbInstance.GetComponent<FireOrb>().hasExplode)
        {
            Terminate();
        }

        // Update when the fireball is charging
        if(!isReleased())
        {
            fireOrbInstance.transform.position = elementary.transform.position;
            target = CalculateTrajectory();
            if (fireOrbInstance.transform.localScale.x < projectileMaxSize)
            {
                fireOrbInstance.transform.localScale += new Vector3(projectileGrowth, projectileGrowth, projectileGrowth);
                lastshrapnelspawn--;
                if(lastshrapnelspawn <= 0) { 
                    fireOrbInstance.GetComponent<FireOrb>()?.addShrapnel();
                    lastshrapnelspawn = 3;
                }                
            }
            projectileTopSpeed += projectileTopSpeedGrowth;
            maxDistance += projectileMaxDistanceGrowth;
        }
        if (fireOrbInstance.GetComponent<FireOrb>().hasExplode)
        {
            Terminate();
        }
        else if (launched)
        {
            elem.computePosition = false;
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
        elementaryfollow = true;
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
            
            gameManager = GameModeSingleton.GetInstance();
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
            

        }
    }

    private Vector3 CalculateTrajectory()
    {
        trajcalculated = true;
        RaycastHit hit;
        int ignoreLayers = 1 << HarmonyLayers.LAYER_PLAYERSPELL;
        ignoreLayers = ~ignoreLayers;
        bool cast = Physics.Raycast(gameManager.GetCinemachineCameraController.GetViewPosition,
            gameManager.GetCinemachineCameraController.GetViewDirection,
            out hit,
            Mathf.Infinity,
            ignoreLayers
            );

        if (cast)
        {
            if(debug)
                Debug.DrawRay(gameManager.GetCinemachineCameraController.GetViewPosition, 
                gameManager.GetCinemachineCameraController.GetViewDirection * 20, 
                Color.green, 
                5);


            Vector3 newDirection = hit.point - elementary.transform.position;
            newDirection.Normalize();

            if (debug)
            {
                Debug.Log("Target spotted : " + hit.collider.gameObject.name);
                Debug.DrawRay(elementary.transform.position,
                newDirection * 20,
                Color.blue,
                5);
            }              
            return newDirection;
        }
        else
        {
            
            Vector3 virtualTarget = gameManager.GetCinemachineCameraController.GetViewPosition + 
                gameManager.GetCinemachineCameraController.GetViewDirection *
                aimDistance;
            if(debug)
            {
                if(virtualTargetReference == null)
                {
                    virtualTargetReference = Instantiate(virtualTargetPrefab, virtualTarget, Quaternion.identity);
                }
                else
                {
                    virtualTargetReference.transform.position = virtualTarget;
                }
                Debug.DrawRay(gameManager.GetCinemachineCameraController.GetViewPosition,
                virtualTarget * aimDistance,
                Color.red,
                5);
            }
            
            virtualTarget = virtualTarget - elementary.transform.position;
            virtualTarget.Normalize();
            if(debug)
            {
                Debug.DrawRay(elementary.transform.position,
                virtualTarget * aimDistance,
                Color.blue,
                5);
            }         
            return virtualTarget;
        }
    }


    /// <summary>
    /// Detroys fire Orbs and resets Elementary
    /// </summary>
    public override void Terminate()
    {
        if(canceled)
        {
            Destroy(marker.gameObject);
        }
        Destroy(fireOrbInstance);
        elem.Reset();
        Destroy(gameObject);
    }

    protected override void onChargeEnd(float chargetime)
    {
        base.onChargeEnd(chargetime);
        launched = true;
        if (!trajcalculated)
            target = CalculateTrajectory();
        elementaryfollow = false;
        //target = gameManager.GetCinemachineCameraController.GetViewDirection;        
        Destroy(marker.gameObject);
        if(isBlinked)
        {
            Debug.Log("Molotov � appliquer");
            isExplosive = true;
        }
        if(debug)
        {
            Destroy(virtualTargetReference);
        }
        
    }


}
