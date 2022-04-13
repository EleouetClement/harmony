using System.Collections;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public CanvasGroup fadeScreen;
    public float fadeInTime = 3f;
    public float fadeOutTime = 1f;

    private Transform playerSpawn;
    private GameObject player;

    private float elapsedTime = 0f;
    private bool isFadingIn = false;
    private bool isFadingOut = false;

    private void Awake()
    {
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
        player = GameModeSingleton.GetInstance().GetPlayerReference;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer == HarmonyLayers.LAYER_PLAYER)
        {
            fadeScreen.alpha = 0f;
            fadeScreen.gameObject.SetActive(true);
            isFadingIn = true;
        }
    }

    private void Update()
    {
        if (isFadingIn)
        {
            elapsedTime += Time.deltaTime;
            elapsedTime = Mathf.Clamp(elapsedTime, 0f, fadeInTime);
            fadeScreen.alpha = 0f + (elapsedTime / fadeInTime);

            if (fadeScreen.alpha >= 1f)
            {
                player.SetActive(false);
                player.transform.position = playerSpawn.position;
                player.SetActive(true);

                isFadingIn = false;
                isFadingOut = true;
                elapsedTime = 0f;
            }
        }

        if (isFadingOut)
        {
            elapsedTime += Time.deltaTime;
            elapsedTime = Mathf.Clamp(elapsedTime, 0f, fadeOutTime);
            fadeScreen.alpha = 1f - (elapsedTime / fadeOutTime);

            if (fadeScreen.alpha <= 0)
            {
                isFadingOut = false;
                elapsedTime = 0f;
            }
        }
    }
}
