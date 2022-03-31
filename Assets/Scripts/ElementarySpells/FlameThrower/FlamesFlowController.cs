using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamesFlowController : MonoBehaviour
{
    private float knockBackForce;

    GameModeSingleton gm;
    private void Awake()
    {
        gm = GameModeSingleton.GetInstance();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        IDamageable collidedItem = other.gameObject.GetComponent<IDamageable>();
        if (collidedItem == null)
        {
            Debug.LogError("Item not damageable");
            return;
        }
        AbstractSpell spell = gm.GetElementaryReference.GetComponent<ElementaryController>().currentSpell;
        collidedItem.OnDamage(spell.SetDamages(transform.forward));
    }


}
