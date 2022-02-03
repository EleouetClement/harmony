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
    /// <summary>
    /// Force needed to be applied when bullet drop is on
    /// </summary>
    [SerializeField][Range(-20, 0)] private float gravityForce;
    [SerializeField] [Min(0)] private float baseProjectileSpeed;

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
    private GameObject fireBallInstance;
    /// <summary>
    /// true if the distance between the projectile and the character surpass maxDistance
    /// </summary>
    private bool applyProjectileDrop = false;
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
        if(Vector3.Distance(origin, transform.position) >= maxDistance)
        {
            ApplyForces();
        }
        velocity = target * baseProjectileSpeed * Time.fixedDeltaTime;
        Debug.Log(velocity);
        transform.Translate(velocity);
    }



    private void Move()
    {
        velocity += target * baseProjectileSpeed * Time.fixedDeltaTime;
    }
    

    private void ApplyForces()
    {
        //TO DO...
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
            fireBallInstance = Instantiate(FireballPrefab, transform.position, Quaternion.identity);
            origin = fireBallInstance.transform.position;
            elementary.GetComponent<ElementaryController>().computePosition = false;
        }
    }


    protected override void onChargeEnd(float chargetime)
    {
        
    }
}
