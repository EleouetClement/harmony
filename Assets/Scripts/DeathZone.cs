using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeathZone : MonoBehaviour
{
    public CanvasGroup fadeScreen;
    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;

    private Transform playerSpawn;
    private GameObject player;
    private PlayerInput playerInput;

    private float elapsedTime = 0f; // current time during the fade
    private bool isFadingIn = false;
    private bool isFadingOut = false;

    private void Start()
    {
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
        player = GameModeSingleton.GetInstance().GetPlayerReference;
        playerInput = GameModeSingleton.GetInstance().GetPlayerReference.GetComponent<PlayerInput>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer == HarmonyLayers.LAYER_PLAYER)
        {
            fadeScreen.alpha = 0f;
            fadeScreen.gameObject.SetActive(true);
            isFadingIn = true;
            playerInput.DeactivateInput(); // Stop player's movements
        }
    }

    private void Update()
    {
        if (isFadingIn)
        {
            // Fade in
            elapsedTime += Time.deltaTime;
            elapsedTime = Mathf.Clamp(elapsedTime, 0f, fadeInTime);
            fadeScreen.alpha = 0f + (elapsedTime / fadeInTime);

            if (fadeScreen.alpha >= 1f)
            {
                // Replace the player to the spawn position
                player.SetActive(false);
                player.transform.position = playerSpawn.position;
                player.SetActive(true);
                playerInput.DeactivateInput(); // Stop player's movements

                isFadingIn = false;
                isFadingOut = true;
                elapsedTime = 0f;
            }
        }

        if (isFadingOut)
        {
            // Fade out
            elapsedTime += Time.deltaTime;
            elapsedTime = Mathf.Clamp(elapsedTime, 0f, fadeOutTime);
            fadeScreen.alpha = 1f - (elapsedTime / fadeOutTime);

            if (fadeScreen.alpha <= 0)
            {
                isFadingOut = false;
                elapsedTime = 0f;
            }
            else if (fadeScreen.alpha <= 0.4f)
            {
                playerInput.ActivateInput(); // Allow player's movements
            }

        }
    }
}
