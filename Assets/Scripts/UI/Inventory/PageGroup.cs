using UnityEngine;

[CreateAssetMenu(menuName = "Page/PageGroup")]
public class PageGroup : ScriptableObject
{
    [TextArea] public string nameGroup;
    public PageSystem[] pages;
    public bool isUnlocked;
}
