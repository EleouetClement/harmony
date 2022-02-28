using UnityEngine;

public class Thruster : MonoBehaviour
{
    public float jumpForceAdded;
    public bool canPropel = true;
    private GameModeSingleton gms;
    private PlayerMotionController playerMotionController;

    void Awake()
    {
        gms = GameModeSingleton.GetInstance();
        playerMotionController = gms.GetPlayerReference.GetComponent<PlayerMotionController>();
    }

    void Update()
    {
        // If the player is in the air and the pillar finished to expand, the player can not be propelled by the pillar
        if(EarthPillar.instance != null && EarthPillar.instance.isTotallyOut)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.layer == HarmonyLayers.LAYER_PLAYER)
        {
            // If the player hits the ground when the pillar finished to expand, he will be propelled by the pillar
            if(EarthPillar.instance != null && EarthPillar.instance.isTotallyOut && playerMotionController.onGround && canPropel)
            {
                playerMotionController.onGround = false;
                playerMotionController.velocity.y = playerMotionController.jumpForce + jumpForceAdded;
            }
        }
    }
}
