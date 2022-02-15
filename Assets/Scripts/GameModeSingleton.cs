using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Gamemode controller that contains usefull references
/// </summary>
public class GameModeSingleton : MonoBehaviour
{
    [SerializeField] private GameObject playerReference;
    [SerializeField] private GameObject elementaryReference;
    [SerializeField] private GameObject playerCameraReference;
    [SerializeField] private GameObject playerHUD;
    [SerializeField] private GameObject playerCrossAir;
    [SerializeField] private CinemachineCameraController cinemachineCameraControl;
    private CameraController playerCameraController;

    private static GameModeSingleton _instance;

    private GameModeSingleton() { }

    private void Awake()
    {
        Debug.Log("Initialisation du singleton");
        if (playerReference == null)
            playerReference = GameObject.Find("Player");
        if (elementaryReference == null)
            elementaryReference = GameObject.Find("Elementary");
        if (playerCameraReference == null)
            playerCameraReference = GameObject.Find("CameraRig");
        if (playerHUD == null)
            playerHUD = GameObject.Find("ATH");
        if (playerCrossAir == null)
            playerCrossAir = GameObject.Find("Reticle");
        playerCameraController = playerCameraReference.GetComponent<CameraController>();
        if (playerCameraController == null)
            Debug.LogWarning("no camera controller");
        if (cinemachineCameraControl == null && playerCameraReference != null)
        {
            cinemachineCameraControl = playerCameraReference.GetComponent<CinemachineCameraController>();
        }
        _instance = this;
    }

    public static GameModeSingleton GetInstance()
    {
        if(_instance == null)
        {
            Debug.LogError("GameModeSingleton : not initialized plz fix!!!!!!!!!!!!");
            _instance = new GameModeSingleton();
        }
        return _instance;
    }

    public GameObject GetPlayerReference
    {
        get
        {
            return playerReference;
        }
    }

    public GameObject GetEmentaryReference
    {
        get
        {
            return elementaryReference;
        }
    }

    public GameObject GetPlayerCameraReference
    {
        get
        {
            return playerCameraReference;
        }
    }

    public GameObject GetPlayerHUD
    {
        get
        {
            return playerHUD;
        }
    }
    public GameObject GetPlayerReticle
    {
        get
        {
            return playerCrossAir;
        }
    }

    public CameraController GetPlayerCameraController
    {
        get
        {
            return playerCameraController;
        }
    }

    public CinemachineCameraController GetCinemachineCameraController
    {
        get
        {
            return cinemachineCameraControl;
        }
    }

}
