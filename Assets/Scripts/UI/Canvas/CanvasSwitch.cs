using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CanvasSwitch : MonoBehaviour
{
    public CanvasType nextCanvas;

    CanvasManager canvasManager;
    Button thisButton;

    // Start is called before the first frame update
    void Start()
    {
        canvasManager = CanvasManager.GetInstance();
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnClick);
    }


    private void OnClick()
    {
        canvasManager.SwitchCanvas(nextCanvas);
    }
}
