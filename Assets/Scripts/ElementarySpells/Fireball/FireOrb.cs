using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOrb : MonoBehaviour
{
    [SerializeField] private GameObject firePrefab;
    private bool blinked = false;
    private bool environment = false;
    private bool hasExplode = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!environment && !hasExplode)
        {
            Explode();
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
        Debug.Log("Explosion");
        Instantiate(firePrefab, transform.position, Quaternion.identity);
        hasExplode = true;
    }

    private void BlinkBehaviour()
    {
        //TO DO...
    }

}
