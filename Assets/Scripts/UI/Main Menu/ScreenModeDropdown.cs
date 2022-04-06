using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenModeDropdown : MonoBehaviour
{

    Dropdown dropDown;
    List<string> options = new();
    Resolution[] resolutions;
    // Start is called before the first frame update
    void Awake()
    {
        dropDown = GetComponent<Dropdown>();
        dropDown.onValueChanged.AddListener(InputHandler);
        resolutions = Screen.resolutions;
        dropDown.ClearOptions();
		foreach (Resolution resolution in resolutions)
		{
            options.Add(resolution.width + "x" + resolution.height);
		}
        dropDown.AddOptions(options);
    }


    public void InputHandler(int val)
    {
        Screen.SetResolution(resolutions[val].width,resolutions[val].height,FullScreenMode.FullScreenWindow);
	}
}
