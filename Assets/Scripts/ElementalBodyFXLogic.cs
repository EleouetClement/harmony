using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalBodyFXLogic : MonoBehaviour
{

    private Vector3 StartPosRefLocale;
    private float timeLocale = 0f;
    private Transform playerref;

    public float BreatheAmp = 0.1f;
    public float BreathTime = 4f;

    public float FrontAmp = 0.05f;
    public float FrontTime = 10f;

    void Start()
    {
        StartPosRefLocale = transform.localPosition;
        playerref = GameModeSingleton.GetInstance().GetPlayerMesh;
    }

    void Update()
    {
        timeLocale += Time.deltaTime;

        float ratiobreathe = (timeLocale % BreathTime) / BreathTime;
        float newy = StartPosRefLocale.y + Mathf.Sin(ratiobreathe * Mathf.PI * 2) * BreatheAmp;

        float ratiofront = (timeLocale % FrontTime) / FrontTime;
        float newz = StartPosRefLocale.z + Mathf.Sin(ratiofront * Mathf.PI * 2) * FrontAmp;

        transform.localPosition = new Vector3(StartPosRefLocale.x, newy, newz);
        transform.rotation = playerref.rotation;
    }
}
