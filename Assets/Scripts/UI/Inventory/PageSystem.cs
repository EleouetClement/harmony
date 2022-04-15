using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageSystem : ScriptableObject
{
    private int index;
    public string title;
    [TextArea] public string textContent;
}
