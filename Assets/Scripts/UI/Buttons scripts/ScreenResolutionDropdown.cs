using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenResolutionDropdown : DropdownHandler
{

    Resolution[] resolutions;

	// Start is called before the first frame update
	new void Start()
	{
		base.Start();
		resolutions = Screen.resolutions;
		foreach (Resolution resolution in resolutions)
		{
			options.Add(resolution.width + "x" + resolution.height);
		}
		dropDown.AddOptions(options);
		dropDown.value = options.IndexOf(Screen.currentResolution.width + "x" + Screen.currentResolution.height);
	}


	public override void InputHandler(int val)
    {
        Screen.SetResolution(resolutions[val].width,resolutions[val].height,Screen.fullScreenMode);
	}
}
