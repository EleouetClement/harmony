using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/TutorialObject")]
public class TutorialObject : ScriptableObject
{
    [SerializeField] private string title;
    [SerializeField] [TextArea(minLines:1,maxLines:100)] private string[] text;

    public string[] Text => text;
    public string Title => title;
}
