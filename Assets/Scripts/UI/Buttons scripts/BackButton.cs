using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BackButton : ButtonHandler
{

	new protected void Start()
	{
		base.Start();
		
	}
	protected override void OnClick()
	{
		canvasManager.SwitchCanvas(canvasManager.PreviousCanvas.canvasType);
	}

}
