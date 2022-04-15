using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryMenu;

    private int money { get; set; } = 0;
    public TMP_Text textMoneyAmount;

    public string lockedText = "?"; // Visible text when the page is still locked

    [Header("------ Journal attributes ------")]
    [SerializeField] private GameObject listButtonJournal; // Contains the list of journal page buttons
    [SerializeField] private GameObject contentJournal;
    [SerializeField] private TMP_Text titleJournal;
    [SerializeField] private TMP_Text textJournal;
    [SerializeField] private TMP_Text textNbPagesJournal;
    [SerializeField] private Button buttonPreviousArrowJournal;
    [SerializeField] private Button buttonNextArrowJournal;
    [SerializeField] private List<PageGroup> listGroupPagesJournal; // Contains the list of journal "books"
    private List<Button> listButtonsPagesJournal;

    [Header("------ Tips attributes ------")]
    [SerializeField] private GameObject listButtonTips; // Contains the list of tips page buttons
    [SerializeField] private GameObject contentTips;
    [SerializeField] private List<PageGroup> listGroupPagesTips; // Contains the list of tips "books"
    private List<Button> listButtonsPagesTips;

    private PlayerInput playerInput;
    private bool isInventoryOpen = false;
    private bool canQuitInventory = false;
    private int currentJournalPageNumber = 0; // ------------------------------------- ou 0
    private int currentTipsPageNumber = 0;

    public static InventoryManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one instance of InventoryManager in the scene");
            return;
        }
        instance = this;
        
        // Initialization of button lists
        listButtonsPagesJournal = new List<Button>();
        listButtonsPagesTips = new List<Button>();

        for (int i = 0; i < listGroupPagesJournal.Count; i++)
        {
            listGroupPagesJournal[i].isUnlocked = false;
        }

        // FOR JOURNAL PAGES :
        // For each button, get the text and modify it depending on whether they are blocked or not
        int ind = 0;
        foreach (Button button in listButtonJournal.GetComponentsInChildren<Button>())
        {
            TMP_Text txt = button.GetComponentInChildren<TMP_Text>();

            if (listGroupPagesJournal[ind].isUnlocked)
            {
                // If unlocked, modify the text button depending on its title
                txt.text = listGroupPagesJournal[ind].nameGroup;
            }
            else
            {
                // If locked, modify the text button depending on the locked text
                txt.text = lockedText;
            }

            // Add each button to the list to have a button list with their index
            button.GetComponent<ButtonPage>().SetIndexButton(ind);
            button.onClick.AddListener(() => OpenJournalPage(button.GetComponent<ButtonPage>().GetIndexButton()));
            //button.onClick.AddListener(() => PreviousJournalPage(button.GetComponent<ButtonPage>().GetIndexButton()));
            //button.onClick.AddListener(() => NextJournalPage(button.GetComponent<ButtonPage>().GetIndexButton()));
            buttonPreviousArrowJournal.onClick.AddListener(() => PreviousJournalPage(button.GetComponent<ButtonPage>().GetIndexButton()));
            buttonNextArrowJournal.onClick.AddListener(() => NextJournalPage(button.GetComponent<ButtonPage>().GetIndexButton()));
            listButtonsPagesJournal.Add(button);

            ind++;
        }

        // FOR TIPS PAGES :
        // For each button, get the text and modify it depending on whether they are blocked or not
        ind = 0;
        foreach (Button button in listButtonTips.GetComponentsInChildren<Button>())
        {
            TMP_Text txt = button.GetComponentInChildren<TMP_Text>();

            if (listGroupPagesTips[ind].isUnlocked)
            {
                // If unlocked, modify the text button depending on its title
                txt.text = listGroupPagesTips[ind].nameGroup;
            }
            else
            {
                // If locked, modify the text button depending on the locked text
                txt.text = lockedText;
            }

            // Add each button to the list to have a button list with their index
            listButtonsPagesTips.Add(button);

            ind++;
        }

        // Check if the amount of buttons and ScriptableObject (journal group / tips group) are equal, if not --> error
        if (listGroupPagesJournal.Count != listButtonsPagesJournal.Count)
        {
            Debug.LogError("Size of -List Group Pages Journal- is different from size of -Content Pages Journal- in InventoryManager");
        }
        if (listGroupPagesTips.Count != listButtonsPagesTips.Count)
        {
            Debug.LogError("Size of -List Group Pages Tips- is different from size of -Content Pages Tips- in InventoryManager");
        }

        //CloseInventory();
        OpenJournalTab();
    }

    private void Start()
    {
        playerInput = GameModeSingleton.GetInstance().GetPlayerReference.GetComponent<PlayerInput>();
        textMoneyAmount.text = money.ToString();
        ClearMenu();
    }

    public void ClearMenu()
    {
        buttonNextArrowJournal.interactable = false;
        buttonPreviousArrowJournal.interactable = false;
        titleJournal.text = lockedText;
        textJournal.text = lockedText;
        textNbPagesJournal.text = "?/?";
    }

    private void Update()
    {
        //if (canQuitInventory)
        //{
        //    Debug.Log("OUIIIIIIIIIIIIIIIIIIIIIIII");
        //    if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape))
        //    {
        //        Debug.Log("TESTTTTTTTTTTTTTTTTTTT");
        //        canQuitInventory = false;
        //        CloseInventory();
        //    }
        //}
        //else
        //{
        //    Debug.Log("NOOOOOOOOOOOOOOOOOOOOOOOON");

        //}

        //if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Debug.Log("KEYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY");
        //    if(canQuitInventory)
        //    {
        //        Debug.Log("TESTTTTTTTTTTTTTTTTTTT");
        //        canQuitInventory = false;
        //        CloseInventory();
        //    }
        //}

    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("WAIIIIIIIIIIIIIIIIIIIIIIIIIIIIT");
        canQuitInventory = true;
    }

    public void AddMoney(int money)
    {
        this.money += money;
        textMoneyAmount.text = money.ToString();
    }

    public void OpenInventory()
    {
        isInventoryOpen = true;
        playerInput.DeactivateInput();
        Time.timeScale = 0f;
        inventoryMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //StartCoroutine(Wait());
    }

    public void OpenJournalTab()
    {
        listButtonJournal.SetActive(true);
        contentJournal.SetActive(true);

        listButtonTips.SetActive(false);
        contentTips.SetActive(false);
    }

    public void OpenTipsTab()
    {
        listButtonJournal.SetActive(false);
        contentJournal.SetActive(false);

        listButtonTips.SetActive(true);
        contentTips.SetActive(true);
    }

    public void UpdateJournalPageList(int index)
    {
        listButtonsPagesJournal[index].GetComponentInChildren<TMP_Text>().text = listGroupPagesJournal[index].nameGroup;
    }

    public void UpdateTipsPageList(int index)
    {
        listButtonsPagesTips[index].GetComponentInChildren<TMP_Text>().text = listGroupPagesTips[index].nameGroup;
    }

    // Open the journal page when click on a button
    public void OpenJournalPage(int indexJournal)
    {
        currentJournalPageNumber = 0;
        buttonPreviousArrowJournal.interactable = false;
        buttonNextArrowJournal.interactable = false;

        // If the journal is unlocked, its informations will be showed, else they will be locked
        if (listGroupPagesJournal[indexJournal].isUnlocked)
        {
            titleJournal.text = listGroupPagesJournal[indexJournal].pages[currentJournalPageNumber].title;
            textJournal.text = listGroupPagesJournal[indexJournal].pages[currentJournalPageNumber].textContent;
            textNbPagesJournal.text = (currentJournalPageNumber + 1) + "/" + listGroupPagesJournal[indexJournal].pages.Count;

            if (listGroupPagesJournal[indexJournal].pages.Count > 1)
            {
                buttonNextArrowJournal.interactable = true;
            }
        }
        else
        {
            titleJournal.text = lockedText;
            textJournal.text = lockedText;
            textNbPagesJournal.text = "?/?";
        }
    }

    public void PreviousJournalPage(int indexJournal)
    {
        currentJournalPageNumber--;
        buttonNextArrowJournal.interactable = true;
        
        if (listGroupPagesJournal[indexJournal].isUnlocked)
        {


            titleJournal.text = listGroupPagesJournal[indexJournal].pages[currentJournalPageNumber - 1].title;
            textJournal.text = listGroupPagesJournal[indexJournal].pages[currentJournalPageNumber - 1].textContent;
            textNbPagesJournal.text = (currentJournalPageNumber) + "/" + listGroupPagesJournal[indexJournal].pages.Count;

            if (currentJournalPageNumber == 1)
            {
                buttonPreviousArrowJournal.interactable = false;
            }
        }
    }

    public void NextJournalPage(int indexJournal)
    {
        Debug.Log("currentJournalPageNumber before = " + currentJournalPageNumber);
        currentJournalPageNumber++;
        Debug.Log("currentJournalPageNumber after = " + currentJournalPageNumber);

        buttonPreviousArrowJournal.interactable = true;

        if (listGroupPagesJournal[indexJournal].isUnlocked)
        {
            titleJournal.text = listGroupPagesJournal[indexJournal].pages[currentJournalPageNumber].title;
            textJournal.text = listGroupPagesJournal[indexJournal].pages[currentJournalPageNumber].textContent;
            textNbPagesJournal.text = (currentJournalPageNumber + 1) + "/" + listGroupPagesJournal[indexJournal].pages.Count;

            if (currentJournalPageNumber >= listGroupPagesJournal[indexJournal].pages.Count - 1)
            {
                buttonNextArrowJournal.interactable = false;
            }
        }
        //else
        //{
        //    titleJournal.text = lockedText;
        //    textJournal.text = lockedText;
        //    textNbPagesJournal.text = "?/?";
        //}
    }

    public void CloseInventory()
    {
        isInventoryOpen = false;
        canQuitInventory = false;
        inventoryMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerInput.ActivateInput();
    }

    public int GetMoneyAmount
    {
        get
        {
            return money;
        }
    }
    #region tipsGetters&Setters
    public List<PageGroup> GetTipsPageGroups
    {
        get
        {
            return listGroupPagesTips;
        }
    }

    public PageGroup GetTipsPageGroup(int idx)
    {
        return listGroupPagesTips[idx];
    }

    public bool UnlockTipInInventory(int idx)
    {
        if(idx >= listGroupPagesTips.Count)
        {
            return false;
        }
        listGroupPagesTips[idx].isUnlocked = true;
        return true;
    }
    #endregion

    #region journalGetters&Setters
    public List<PageGroup> GetJournalPageGroups
    {
        get
        {
            return listGroupPagesJournal;
        }
    }

    public PageGroup GetJournalPageGroup(int idx)
    {
        return listGroupPagesJournal[idx];
    }

    public bool UnlockJournalInInventory(int idx)
    {
        if (idx >= listGroupPagesJournal.Count)
        {
            return false;
        }
        listGroupPagesJournal[idx].isUnlocked = true;
        UpdateJournalPageList(idx);
        return true;
    }
    #endregion

}
