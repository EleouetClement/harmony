using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOrb : MonoBehaviour
{
    [SerializeField] private GameObject firePrefab;
    private bool blinked = false;
    private bool environment = false;
    private List<GameObject> fragments = new List<GameObject>(20);

    public GameObject explosionEffect;
    public GameObject shrapnelPrefab;

    /// <summary>
    /// True if the orb already hit something.
    /// </summary>
    public bool hasExplode { get; private set; } = false;

    private GameModeSingleton manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameModeSingleton.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if (blinked)
        {
            BlinkBehaviour();
        }
    }

    private void FixedUpdate()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasExplode)
        {
            Explode();
            /*if (collision.gameObject.layer == HarmonyLayers.LAYER_TARGETABLE)
            {
                hasExplode = true;
            }
            else
            {
                Explode();
            }*/
        }

    }

    /// <summary>
    /// Activate blinked spell stats
    /// </summary>
    public void SetBlinkBehaviour()
    {
        blinked = true;
    }

    public void IsEnvironmentSpell(bool environment)
    {
        this.environment = environment;
    }
    private void Explode()
    {
        float explosionradius = 2f;
        // Idamageable behavior
        //Debug.Log("Fireball expolosion at : " + transform.position + " / radius : " + explosionradius);
        Collider[] enemies = Physics.OverlapCapsule(transform.position + Vector3.down * 3, transform.position + Vector3.up * 3, explosionradius, 1 << HarmonyLayers.LAYER_TARGETABLE);
        AbstractSpell currSpell = GameModeSingleton.GetInstance().GetElementaryReference.GetComponent<ElementaryController>().currentSpell;
        if (enemies.Length >= 1)
            foreach(Collider c in enemies){             
                c.gameObject.GetComponent<IDamageable>()?.OnDamage(currSpell.SetDamages((transform.position - manager.GetPlayerReference.transform.position).normalized));
            }
        // VFX
        Instantiate(explosionEffect, transform.position + Vector3.up, transform.rotation);
        //Instantiate(firePrefab, transform.position, Quaternion.identity);
        hasExplode = true;
    }

    private void BlinkBehaviour()
    {
        //TO DO...
    }

    // Adds 1 shrapnel to the fireball, effectively increasing its size.
    public void addShrapnel()
    {
        GameObject shrapnel = Instantiate(shrapnelPrefab, transform.position + randomVector(), Random.rotation, transform);
        fragments.Add(shrapnel);
    }

    private static float expense = 0.25f;
    private Vector3 randomVector()
    {
        return new Vector3(Random.Range(-expense, expense), Random.Range(-expense, expense), Random.Range(-expense, expense));
    }

}
