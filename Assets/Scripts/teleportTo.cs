using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportTo : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            Debug.Log(player.transform.position);
            player.transform.position = gameObject.transform.position;
        }
    }
}
