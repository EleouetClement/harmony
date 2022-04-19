using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ButtonHandler : MonoBehaviour
{
	CanvasManager canvasManager;

	protected void Awake()
	{
		canvasManager = CanvasManager.GetInstance();

	}
}
