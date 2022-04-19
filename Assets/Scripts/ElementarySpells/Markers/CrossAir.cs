using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossAir : AbstractMarker
{
    private RectTransform reticle;
    private RectTransform newReticle;
    private float elapsedTime = 0f;
    public float fadeDelay = 1f;

    [Range(50, 250)] public float size;
    /// <summary>
    /// New fov value during aiming
    /// </summary>
    [SerializeField] [Min(0)] private float zoomPower;
    GameModeSingleton manager;

    private void Awake()
    {

        manager = GameModeSingleton.GetInstance();
        reticle = manager.GetPlayerReticle.GetComponent<RectTransform>();
        newReticle = manager.GetNewPlayerReticle.GetComponent<RectTransform>();
        GameObject crossAirExterieur = newReticle.transform.Find("CrosshairExt").gameObject;
        crossAirExterieur.SetActive(true);
        manager.GetCinemachineCameraController.ZoomIn();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void DisplayTarget(Vector3 direction, Vector3 origin)
    {
       reticle.sizeDelta = direction;
    }


    public override void OnDestroy()
    {
        manager.GetCinemachineCameraController.ZoomOut();
        manager.GetNewPlayerReticle.transform.Find("CrosshairExt").gameObject.SetActive(false);
    }
 }
