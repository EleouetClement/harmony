using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Gamemode controller that contains usefull references
/// </summary>
public class GameModeSingleton : MonoBehaviour
{
    private GameObject playerReference;
    private GameObject elementaryReference;
    private GameObject playerCameraReference;
    private GameObject playerHUD;

    public static GameModeSingleton _instance;

    private GameModeSingleton() { }

    private void Awake()
    {
        playerReference = GameObject.Find("Player");
        elementaryReference = GameObject.Find("Elementary");
        playerCameraReference = GameObject.Find("CameraRig");
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



}
