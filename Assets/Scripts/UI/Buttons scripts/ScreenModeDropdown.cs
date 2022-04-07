using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenModeDropdown : DropdownHandler
{
	
	// Start is called before the first frame update
	void Awake()
    {
		base.Awake();
		options.Add("Pleine écran exclusif");
		options.Add("Pleine écran fenêtré");
		options.Add("Fenêtré maximisé");
		options.Add("Fenêtré");
		dropDown.AddOptions(options);
	}

	public override void InputHandler(int val)
	{
		Screen.fullScreenMode = (FullScreenMode)val;
	}
}
