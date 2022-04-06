using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenModeDropwdown : MonoBehaviour
{

    Dropdown dropDown;
    Resolution[] resolutions;
    // Start is called before the first frame update
    void Awake()
    {
        dropDown = GetComponent<Dropdown>();
        resolutions = Screen.resolutions;
        dropDown.options.Clear();
		foreach (Resolution resolution in resolutions)
		{
            dropDown.options.Add(new Dropdown.OptionData(resolution.width + "x" + resolution.height));
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InputHandler(int val)
    {
        Screen.SetResolution(resolutions[val].width,resolutions[val].height,FullScreenMode.FullScreenWindow);
	}
}
