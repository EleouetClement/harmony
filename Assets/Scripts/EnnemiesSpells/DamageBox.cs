using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageBox : MonoBehaviour
{
    public bool active = false;
    public float damages = 20;
    public bool shieldable = true;
    public bool constant = false;

    public UnityEvent OnDamage;
    public UnityEvent OnShielded;
    public UnityEvent OnPerfectShielded;

    private void OnTriggerEnter(Collider other)
    {
        if (!active) return;

        if (other.gameObject.layer == HarmonyLayers.LAYER_PLAYER)
        {
            if (!constant) active = false;
            other.gameObject.GetComponent<IDamageable>()?.OnDamage(new DamageHit(damages, AbstractSpell.Element.Physical));
            OnDamage.Invoke();
        }
        else if(shieldable && other.gameObject.layer == HarmonyLayers.LAYER_SHIELD)
        {
            if (!constant) active = false;
            OnShielded.Invoke();
        }
    }
}
