using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOrb : MonoBehaviour
{
    [SerializeField] private GameObject firePrefab;
    private bool blinked = false;
    private bool environment = false;

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
        if(blinked)
        {
            BlinkBehaviour();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!hasExplode && manager.InFight)
        {
            Explode();
            if(collision.gameObject.layer == HarmonyLayers.LAYER_TARGETABLE)
            {
                Debug.Log("Pouet");
            }
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
        //Debug.Log("Explosion");
        Instantiate(firePrefab, transform.position, Quaternion.identity);
        hasExplode = true;
    }

    private void BlinkBehaviour()
    {
        //TO DO...
    }



}
