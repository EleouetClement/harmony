using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // Dialogue lines depending on the specific trigger
    public DialogueObject[] dialogues;
    public Transform pointToLookAt;
    public Vector3 pointToLookAtPosition;
    private CinemachineCameraController cameraController;

    private void Start()
    {
        cameraController = GameModeSingleton.GetInstance().GetCinemachineCameraController;
        pointToLookAtPosition = pointToLookAt.position;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == HarmonyLayers.LAYER_PLAYER)
        {
            DialogueUI.instance.SetDialogueObjectList(dialogues);
            DialogueUI.instance.SetCurrentIndexDialogue(0);
            cameraController.SetPointToLookAt(pointToLookAtPosition);
            DialogueUI.instance.ShowDialogue(dialogues[0]);
            Destroy(gameObject);
        }
    }
}
