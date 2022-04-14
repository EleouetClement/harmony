using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private float timeBetweenDialogue = 0.3f; // Blank time between the text of 2 characters
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private TMP_Text textLabel;
    private TutorialObject[] tutorialObjectList;

    private int currentIndexDialogue = 0;
    private TypeWriterEffect typeWriterEffect;
    private PlayerInput player;

    public static TutorialUI instance;

    protected void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one instance of DialogueUI in the scene");
            return;
        }
        instance = this;

    }

    protected void Start()
    {
        typeWriterEffect = GetComponent<TypeWriterEffect>();
        player = GameModeSingleton.GetInstance().GetPlayerReference.GetComponent<PlayerInput>();

        //CloseDialogueBox();
        dialogueBox.SetActive(false);
        nameLabel.text = string.Empty;
        textLabel.text = string.Empty;

        //ShowDialogue(testDialogue);
    }

    protected void Update()
    {
        // DEBUG
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //ShowDialogue(testDialogue);
            if (currentIndexDialogue < tutorialObjectList.Length)
            {
                ShowDialogue(tutorialObjectList[currentIndexDialogue]);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            CloseDialogueBox();
        }
    }

    // Open dialogue box and start the dialogue
    public void ShowDialogue(TutorialObject tutorialObject)
    {
        player.DeactivateInput(); // Disable all the input player movement during the dialogue

        nameLabel.text = tutorialObject.Title;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(tutorialObject));
        currentIndexDialogue++;
    }

    // Show each line of the dialogue that you can skip with the Return key on the keyboard
    private IEnumerator StepThroughDialogue(TutorialObject tutorialObject)
    {
        for (int i = 0; i < tutorialObject.Text.Length; i++)
        {
            string dialogue = tutorialObject.Text[i];

            yield return RunTypingEffect(dialogue);

            textLabel.text = dialogue;

            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        }

        CloseDialogueBox();
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        // Effect of letters appearing one after the other
        typeWriterEffect.Run(dialogue, textLabel);

        while (typeWriterEffect.isRunning)
        {
            yield return null;

            // Cancel text typing effect to skip the dialogue
            if (Input.GetKeyDown(KeyCode.Return))
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

        // If there are another dialogue in the same cinematic, it will start the next dialogue
        if (currentIndexDialogue < tutorialObjectList.Length)
        {
            if (timeBetweenDialogue > 0)
            {
                StartCoroutine(WaitBetweenDialogue());
            }
            else
            {
                ShowDialogue(tutorialObjectList[currentIndexDialogue]);
            }

        }
        else
        {
            // Enable the player input
            player.ActivateInput();
            Time.timeScale = 1;
        }
    }

    // Waiting between 2 dialogue box apparearing
    private IEnumerator WaitBetweenDialogue()
    {
        yield return new WaitForSeconds(timeBetweenDialogue);
        ShowDialogue(tutorialObjectList[currentIndexDialogue]);
    }

    // Set the new dialogue lines
    public void SetDialogueObjectList(TutorialObject[] tutorials)
    {
        tutorialObjectList = tutorials;
    }

    // Start the dialogue to this index (usually index of 0)
    public void SetCurrentIndexDialogue(int index)
    {
        currentIndexDialogue = index;
    }
}
