using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySettingsSaver : ButtonHandler
{
	SettingsDataHandler dataHandler;
	new
	private void Start()
	{
		base.Start();
		dataHandler = SettingsDataHandler.Instance;
	}
	protected override void OnClick()
	{
		dataHandler.ScreenWidth = Screen.currentResolution.width;
		dataHandler.ScreenHeight = Screen.currentResolution.height;

		dataHandler.ScreenMode = Screen.fullScreenMode;

		dataHandler.SaveData();

    }
}
