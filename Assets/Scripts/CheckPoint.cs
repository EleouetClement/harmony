using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Transform playerSpawn;

    private void Awake()
    {
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer == HarmonyLayers.LAYER_PLAYER)
        {
            playerSpawn.position = transform.position;
            Destroy(gameObject);
        }
    }
}
