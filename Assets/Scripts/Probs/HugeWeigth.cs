using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugeWeigth : MonoBehaviour
{
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
        
        if(collision.gameObject.layer.Equals(HarmonyLayers.LAYER_PLAYER))
        {
            IDamageable item = collision.gameObject.GetComponent<PlayerGameplayController>() as IDamageable;
            
            if(item == null)
            {
                Debug.LogError("PLayer is Not Damageable");
            }
            else
            {
                Debug.Log("Ouch");
                DamageHit damage = new DamageHit(125.0f);
                item.OnDamage(damage);
            }
        }
    }
}
