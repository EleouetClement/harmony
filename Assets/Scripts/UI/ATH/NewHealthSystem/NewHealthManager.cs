using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHealthManager : MonoBehaviour
{
    private GameObject lightDamages;
    private GameObject heavyDamages;

    private void Awake()
    {
        if(transform.childCount < 2)
        {
            Debug.LogError("NewHealthManager : no sprites for damages");
            Destroy(gameObject);
        }
        else
        {
            lightDamages = transform.GetChild(0).gameObject;
            heavyDamages = transform.GetChild(1).gameObject;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
