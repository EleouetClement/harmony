using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenModeDropdown : DropdownHandler
{
	
	// Start is called before the first frame update
	new void Start()
    {
		base.Start();
		options.Add("Pleine écran exclusif");
		options.Add("Pleine écran fenêtré");
		options.Add("Fenêtré maximisé");
		options.Add("Fenêtré");
		dropDown.AddOptions(options);
		dropDown.value = (int)Screen.fullScreenMode;
	}

	public override void InputHandler(int val)
	{
		Screen.fullScreenMode = (FullScreenMode)val;
	}
}
