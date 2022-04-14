using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : ButtonHandler
{
	
	// Start is called before the first frame update
	new void Start()
    {
		base.Start();
    }
	protected override void OnClick()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
		Application.Quit();
	}

}
