using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    // Dialogue lines depending on the specific trigger
    public TutorialObject[] tutorials;
    public Transform pointToLookAt;
    public Vector3 pointToLookAtPosition;
    public float speedCameraRotationDialogue = 1;

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
            Time.timeScale = 0;
            TutorialUI.instance.SetDialogueObjectList(tutorials);
            TutorialUI.instance.SetCurrentIndexDialogue(0);
            cameraController.SetSpeedCameraRotationDialogue(speedCameraRotationDialogue);
            cameraController.SetPointToLookAt(pointToLookAtPosition);
            TutorialUI.instance.ShowDialogue(tutorials[0]);
            Destroy(gameObject);
        }
    }
}
