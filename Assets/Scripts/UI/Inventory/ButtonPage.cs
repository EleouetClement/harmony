using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPage : MonoBehaviour
{
    private int indexButton;

    public int GetIndexButton()
    {
        return indexButton;
    }

    public void SetIndexButton(int index)
    {
        indexButton = index;
    }
}
