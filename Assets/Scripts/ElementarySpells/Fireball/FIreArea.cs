using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIreArea : MonoBehaviour
{

    [SerializeField] [Min(0)] private float livingTime;
    private Vector3 initialSpawnScale;

    private float liveTimer = Mathf.Epsilon;
    void Start()
    {
        initialSpawnScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(liveTimer >= livingTime)
        {
            Destroy(gameObject);
        }
        //else if(liveTimer >= livingTime - 1)
        //{
        //    // Fade out
        //    transform.localScale = Vector3.Lerp(initialSpawnScale, Vector3.zero, liveTimer / livingTime);
        //}

        liveTimer += Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.layer);
        //TO DO...
    }

}
