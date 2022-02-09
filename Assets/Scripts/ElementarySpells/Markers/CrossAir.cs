using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossAir : AbstractMarker
{
    private RectTransform reticle;

    [Range(50, 250)] public float size;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        reticle = GetComponent<RectTransform>();
    }

    public override void DisplayTarget(Vector3 direction, Vector3 origin)
    {
        
    }


    public override void OnDestroy()
    {
        
    }

}
