using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveBurnable : MonoBehaviour, IDamageable
{

    private float dissolutionprogress = 0f;
    /// <summary>
    /// Animation burn time of this object, in seconds.
    /// </summary>
    public float burntime = 2f;
    private bool isBurning = false;

    void Start()
    {
        Debug.Log("Instance Burnable = " + dissolutionprogress);
        gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Dissolution", 0f);
    }

    void Update()
    {
        if (isBurning)
        {
            dissolutionprogress += Time.deltaTime / burntime;
            gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Dissolution", dissolutionprogress);
        }
        if (dissolutionprogress >= 1f) {
            Destroy(gameObject);
        }
    }

    public void OnDamage(DamageHit hit)
    {
        if (hit.type == AbstractSpell.Element.Fire)
            isBurning = true;
        Debug.Log("Burnable object started consuming, will be gone in " + burntime + " seconds.");
    }
}
