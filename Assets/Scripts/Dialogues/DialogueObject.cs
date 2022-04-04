using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] private string characterName;
    [SerializeField] [TextArea] private string[] dialogue;

    public string[] Dialogue => dialogue;
    public string CharacterName => characterName;
}
