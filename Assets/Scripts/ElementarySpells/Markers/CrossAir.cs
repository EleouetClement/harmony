using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossAir : AbstractMarker
{
    private RectTransform reticle;

    [Range(50, 250)] public float size;

    GameModeSingleton manager;

    private void Awake()
    {
        manager = GameModeSingleton.GetInstance();
        reticle = manager.GetPlayerReticle.GetComponent<RectTransform>();
        if (reticle == null)
            Debug.LogError("Recticle reference is null");
        manager.GetPlayerReticle.SetActive(true);
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
        manager.GetPlayerReticle.SetActive(false);
    }

}
