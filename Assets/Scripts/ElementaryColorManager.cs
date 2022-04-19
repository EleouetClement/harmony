using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementaryColorManager : MonoBehaviour
{

    public Material WaterMatPointer;

    private static readonly Color COLOR_FIRE = new Color(1f, 80f / 255f, 0f);
    private static readonly Color COLOR_WATER = new Color(3f / 255f, 111f / 255f, 210f / 255f);
    private static readonly Color COLOR_EARTH = new Color(137f / 255f, 101f / 255f, 71f / 255f);

    private ElementaryController controller;

    void Start()
    {
        controller = GameModeSingleton.GetInstance().GetElementaryReference.GetComponent<ElementaryController>();
    }

    private float currentcolor_RED = 0.5f, currentcolor_GREEN = 0.5f, currentcolor_BLUE = 0.5f;
    private const float changespeed = 0.02f;

    void FixedUpdate()
    {
        //Computes the target window
        Color targetColor = Color.cyan;
        switch (controller.currentElement)
        {
            case (AbstractSpell.Element.Fire):
                targetColor = COLOR_FIRE;
                break;
            case (AbstractSpell.Element.Water):
                targetColor = COLOR_WATER;
                break;
            case (AbstractSpell.Element.Earth):
                targetColor = COLOR_EARTH;
                break;
        }
        float tolerance = changespeed * 1.7f;
        // Shifts the current rgb values towards target
        if (currentcolor_RED > targetColor.r + tolerance)
            currentcolor_RED -= changespeed;
        else if (currentcolor_RED < targetColor.r - tolerance)
            currentcolor_RED += changespeed;
        
        if (currentcolor_GREEN > targetColor.g + tolerance)
            currentcolor_GREEN -= changespeed;
        else if (currentcolor_GREEN < targetColor.g - tolerance)
            currentcolor_GREEN += changespeed;
        
        if (currentcolor_BLUE > targetColor.b + tolerance)
            currentcolor_BLUE -= changespeed;
        else if (currentcolor_BLUE < targetColor.b - tolerance)
            currentcolor_BLUE += changespeed;
        // Applies the color
        WaterMatPointer.SetColor("_ColorMultiplier", new Color(currentcolor_RED, currentcolor_GREEN, currentcolor_BLUE));
    }
}
