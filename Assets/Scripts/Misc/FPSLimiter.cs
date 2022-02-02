using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    public bool disabled = true;
    private int lastFps = 120;

    // Start is called before the first frame update
    void Start()
    {
        if (!disabled)
            Application.targetFrameRate = lastFps;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.PageDown))
            Application.targetFrameRate -= 10;

        if (Input.GetKeyDown(KeyCode.PageUp))
            Application.targetFrameRate += 10;

        if (Input.GetKeyDown(KeyCode.End))
        {
            disabled = !disabled;
            if (disabled)
            {
                lastFps = Application.targetFrameRate;
                Application.targetFrameRate = 0;
            }
            else
            {
                Application.targetFrameRate = lastFps;
            }
        }
    }

    private void OnGUI()
    {
        GUI.color = Color.red;
        if(disabled)
            GUI.Label(new Rect(10,10,100,100),(1.0f/Time.deltaTime).ToString("0"));
        else
            GUI.Label(new Rect(10,10,100,100),Application.targetFrameRate.ToString());
    }
}
