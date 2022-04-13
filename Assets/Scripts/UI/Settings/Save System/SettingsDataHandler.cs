using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsDataHandler
{

	#region Fields
	public float _cameraSensitivity;
	public int _width, _height;
	public FullScreenMode _screenMode;
	public static SettingsDataHandler _instance;
	private string _filePath;
	private SettingsDataHandler twin;
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

	public  SettingsDataHandler(string path)
	{
		_instance = this;
		_filePath = path;
		LoadData();
	}

	public void SaveData()
	{
		DefaultCheck();
		string settingsData = JsonUtility.ToJson(this);
		System.IO.File.WriteAllText(_filePath, settingsData);
		Debug.Log("Paramètres sauvegardés dans " + _filePath);
	}

	public void LoadData()
	{
		if (System.IO.File.Exists(_filePath))
		{
			string settingsData = System.IO.File.ReadAllText(_filePath);
			twin = JsonUtility.FromJson<SettingsDataHandler>(settingsData);

			_cameraSensitivity = twin.CameraSensitivity;
			_width = twin.ScreenWidth;
			_height = twin.ScreenHeight;
			_screenMode = twin.ScreenMode;

			Screen.SetResolution(_width, _height, _screenMode);
		}
		DefaultCheck();
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
