using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Gamemode controller that contains usefull references
/// </summary>
public class GameModeSingleton : MonoBehaviour
{
    public SettingsDataHandler SDH;

    [SerializeField] private GameObject playerReference;

    private Transform playerMesh;
    [SerializeField] private GameObject elementaryReference;
    /// <summary>
    /// DEPRECATED, Use cinemachineCameraControl instead
    /// </summary>
    private GameObject playerCameraReference;
    [SerializeField] private GameObject playerHUD;
    [SerializeField] private GameObject playerCrossAir;
    [SerializeField] private CinemachineCameraController cinemachineCameraControl;

    public CanvasGroup blackScreen;

    public bool debug = false;

    /// <summary>
    /// True if the player enters in an arena or any fight area.
    /// </summary>
    public bool InFight { get; private set; } = false;

    private static GameModeSingleton _instance;
    private float _elapsedTime;
    private bool _fade_start;

    public bool FadeStart
    {
        get  
        {
			return _fade_start;
        }
        set
        {
            _fade_start = value;
        }
    }

    private GameModeSingleton() { }

    private void Awake()
    {
        _elapsedTime = 0f;
        _fade_start = false;

        //Debug.Log("Initialisation du singleton");
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
        if(cinemachineCameraControl == null && playerCameraReference != null)
        {
            cinemachineCameraControl = playerCameraReference.GetComponent<CinemachineCameraController>();
        }
        if(playerReference == null)
        {
            Debug.LogError("GameModeSingleton : No player reference");
        }
        else
        {
            playerMesh = playerReference.transform.GetChild(1);
        }
        _instance = this;

        SDH = new(Application.persistentDataPath + "/SettingsData.json");
    }

	private void Update()
	{
        if (_elapsedTime < 1f && _fade_start)
        {
            _elapsedTime += Time.deltaTime;
            _elapsedTime = Mathf.Clamp(_elapsedTime, 0f, 1f);
            blackScreen.alpha = 1f - (_elapsedTime / 1f);
        }
		else
		{
            playerReference.GetComponent<PlayerInput>().enabled = true;
		}
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
    #region Getters
    public GameObject GetPlayerReference
    {
        get
        {
            return playerReference;
        }
    }

    public Transform GetPlayerMesh
    {
        get
        {
            return playerMesh;
        }
    }

    public GameObject GetElementaryReference
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

    public CinemachineCameraController GetCinemachineCameraController
    {
        get
        {
            return cinemachineCameraControl;
        }
    }
    #endregion


}
