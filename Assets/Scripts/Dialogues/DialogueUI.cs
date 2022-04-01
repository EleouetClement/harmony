using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private DialogueObject testDialogue;
    [SerializeField] private DialogueObject[] dialogueObjectList;

    private int currentIndexDialogue = 0;
    private TypeWriterEffect typeWriterEffect;

    private void Start()
    {
        typeWriterEffect = GetComponent<TypeWriterEffect>();
        CloseDialogueBox();
        //ShowDialogue(testDialogue);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //ShowDialogue(testDialogue);
            if (currentIndexDialogue < dialogueObjectList.Length)
            {
                ShowDialogue(dialogueObjectList[currentIndexDialogue]);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("TESTTTTTTTTT");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CloseDialogueBox();
        }
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        nameLabel.text = dialogueObject.CharacterName;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
        currentIndexDialogue++;
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];

            yield return RunTypingEffect(dialogue);

            textLabel.text = dialogue;

            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        }

        CloseDialogueBox();
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        typeWriterEffect.Run(dialogue, textLabel);

        while (typeWriterEffect.isRunning)
        {
            yield return null;

            if(Input.GetKeyDown(KeyCode.Return))
            {
                typeWriterEffect.Stop();
            }
        }
    }

    public void CloseDialogueBox()
    {
        dialogueBox.SetActive(false);
        nameLabel.text = string.Empty;
        textLabel.text = string.Empty;
    }
}
