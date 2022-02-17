using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIreArea : MonoBehaviour
{

    [SerializeField] [Min(0)] private float livingTime;

    private float liveTimer = Mathf.Epsilon;
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        if(liveTimer >= livingTime)
        {
            Destroy(gameObject);
        }
        liveTimer += Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.layer);
        //TO DO...
    }

}
