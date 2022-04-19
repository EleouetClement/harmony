using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BackButton : MonoBehaviour
{
    CanvasManager canvasManager;

	private void Awake()
	{
		canvasManager = CanvasManager.GetInstance();
		
	}

}
