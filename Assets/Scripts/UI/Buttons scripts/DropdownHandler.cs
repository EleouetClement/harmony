using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public abstract class DropdownHandler : MonoBehaviour
{
    protected Dropdown dropDown;
    protected List<string> options = new();

	protected void Start()
	{
		dropDown = GetComponent<Dropdown>();
		dropDown.onValueChanged.AddListener(InputHandler);
		dropDown.ClearOptions();
	}

	public abstract void InputHandler(int val);

	public Dropdown Dropdown
	{
		get
		{
			return dropDown;
		}
	}
}
