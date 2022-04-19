using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public abstract class ButtonHandler : MonoBehaviour, IPointerEnterHandler
{
	protected CanvasManager canvasManager;
	protected Button thisButton;

	protected void Start()
	{
		canvasManager = CanvasManager.GetInstance();
		thisButton = GetComponent<Button>();
		thisButton.onClick.AddListener(OnClick);
		thisButton.onClick.AddListener(ClickSound);
	}

	

	protected abstract void OnClick();


	private void ClickSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/Weapons/Pistol");
	}

	private void HoverSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Okay");
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		HoverSound();
	}
}
