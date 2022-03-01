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

    private void OnCollisionEnter(Collision collision)
    {
        //TO DO...
    }
}
