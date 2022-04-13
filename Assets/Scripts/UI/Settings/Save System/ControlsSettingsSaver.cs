using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsSettingsSaver : ButtonHandler
{
    public static ControlsSettingsSaver Instance;
    public float cameraSensitivity;
    SettingsDataHandler dataHandler;
    new
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        Instance = this;
        dataHandler = SettingsDataHandler.Instance;
    }

    
	protected override void OnClick()
	{
        dataHandler.CameraSensitivity = cameraSensitivity;
        dataHandler.SaveData();
	}
}
