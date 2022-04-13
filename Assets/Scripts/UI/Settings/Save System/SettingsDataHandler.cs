using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsDataHandler : MonoBehaviour
{
	#region Fields
	public float _cameraSensitivity;
	public int _width, _height;
	public FullScreenMode _screenMode;
	public static SettingsDataHandler _instance;
	#endregion

	#region Properties
	public float CameraSensitivity
	{
		get
		{
			return _cameraSensitivity;
		}
		set
		{ 
			_cameraSensitivity = value;
		}
	}

	public int ScreenWidth
	{
		get
		{
			return _width;
		}
		set
		{
			_width = value;
		}
	}

	public int ScreenHeight
	{
		get
		{
			return _height;
		}
		set
		{
			_height = value;
		}
	}

	public FullScreenMode ScreenMode
	{
		get
		{
			return _screenMode;
		}
		set
		{
			_screenMode = value;
		}
	}

	public static SettingsDataHandler Instance
	{
		get
		{
			return _instance;
		}
	}
	#endregion

	private void Awake()
	{
		_instance = this;
	}

	public void SaveData()
	{
		DefaultCheck();
		string settingsData = JsonUtility.ToJson(this);
		string filePath = Application.persistentDataPath + "/SettingsData.json";
		System.IO.File.WriteAllText(filePath, settingsData);
		Debug.Log("Paramètres sauvegardés dans " + filePath);
	}

	private void DefaultCheck()
	{
		if (_cameraSensitivity == 0)
		{
			_cameraSensitivity = .25f;
		}
		if (_width == 0 && _height == 0)
		{
			_width = Screen.currentResolution.width;
			_height = Screen.currentResolution.height;
		}
	}
}
