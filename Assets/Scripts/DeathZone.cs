using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private Transform playerSpawn;
    public GameObject player;

    private void Awake()
    {
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer == HarmonyLayers.LAYER_PLAYER)
        {
            Debug.Log("TRIGGERRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR");
            //GameObject.FindGameObjectWithTag("Player").transform.position = transform.position;
            //GameModeSingleton.GetInstance().GetPlayerReference.transform.position = playerSpawn.position;
            //GameObject.FindGameObjectWithTag("Player").transform.position = playerSpawn.position;
            //collider.transform.position = playerSpawn.position;
            //player.transform.position = playerSpawn.position;
            //collider.transform.Translate(playerSpawn.position);
        }
    }
}
