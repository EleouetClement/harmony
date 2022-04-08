using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskManager : MonoBehaviour
{
    public bool hitted = false;
    public GameObject objectHitted { get; private set; }

    public bool lauched { get;  private set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hitted)
        {
            objectHitted = other.gameObject;
            hitted = true;
        }    
    }

    public void SetLaunch()
    {
        lauched = true;
    }
}
