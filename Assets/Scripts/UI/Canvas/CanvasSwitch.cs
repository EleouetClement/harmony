using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CanvasSwitch : ButtonHandler
{
    public CanvasType nextCanvas;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }


    protected override void OnClick()
    {
        canvasManager.SwitchCanvas(nextCanvas);
    }
}
