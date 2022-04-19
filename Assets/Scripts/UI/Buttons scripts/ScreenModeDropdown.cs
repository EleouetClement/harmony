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
		options.Add("Pleine �cran exclusif");
		options.Add("Pleine �cran fen�tr�");
		options.Add("Fen�tr� maximis�");
		options.Add("Fen�tr�");
		dropDown.AddOptions(options);
	}

	public override void InputHandler(int val)
	{
		Screen.fullScreenMode = (FullScreenMode)val;
	}
}
