using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private Transform playerSpawn;

    private void Awake()
    {
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("TRIGGERRRRRRRRRRRRRR");
        if(collision.CompareTag("Player"))
        {
            Debug.Log("TRIGGERRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR");
            collision.transform.position = playerSpawn.position;
        }
    }
}
