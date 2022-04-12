using System;
using System.Collections;
using System.Collections.Generic;
using Harmony.AI;
using UnityEngine;
using UnityEngine.UI;


public class EntityHealthBar : MonoBehaviour
{
    public AIAgent entity;
    public Image bar;

    private void Awake()
    {
        if (!entity)
        {
            entity = GetComponentInParent<AIAgent>();
        }
    }

    private void LateUpdate()
    {
        bar.fillAmount = Mathf.Clamp01(entity.health / entity.maxHealth);
        
        transform.rotation = Camera.main.transform.rotation;
    }
}
