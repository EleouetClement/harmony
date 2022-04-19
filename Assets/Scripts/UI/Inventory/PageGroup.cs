using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Page/PageGroup")]
public class PageGroup : ScriptableObject
{
    [TextArea] public string nameGroup;
    public List<PageSystem> pages;
    public bool isUnlocked;
}
