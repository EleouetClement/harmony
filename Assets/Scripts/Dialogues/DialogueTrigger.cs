using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // Dialogue lines depending on the specific trigger
    public DialogueObject[] dialogues;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == HarmonyLayers.LAYER_PLAYER)
        {
            DialogueUI.instance.SetDialogueObjectList(dialogues);
            DialogueUI.instance.SetCurrentIndexDialogue(0);
            DialogueUI.instance.ShowDialogue(dialogues[0]);
            Destroy(gameObject);
        }
    }
}
