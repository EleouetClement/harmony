using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class LineRendererGradient : MonoBehaviour
{
    Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;

    // Start is called before the first frame update
    void Awake()
    {
        gradient = new();
        colorKey = new GradientColorKey[2];
        alphaKey = new GradientAlphaKey[2];

        colorKey[0].color = Color.red;
        colorKey[0].time = 0f;
        colorKey[1].color = Color.red;
        colorKey[1].time = 1f;

        alphaKey[0].alpha = 0f;
        alphaKey[0].time = 0f;
        alphaKey[1].alpha = 1f;
        alphaKey[1].time = 1f;

        gradient.SetKeys(colorKey, alphaKey);
    }

	private void Start()
	{
        
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
