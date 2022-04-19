using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageSystem : ScriptableObject
{
    private int index;
    [SerializeField] private string title;
    [SerializeField] [TextArea] private string textContent;
}
