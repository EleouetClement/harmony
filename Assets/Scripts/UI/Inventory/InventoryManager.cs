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

    [Header("Journal attributes")]
    [SerializeField] private GameObject listButtonJournal; // Contains the list of journal page buttons
    [SerializeField] private GameObject contentJournal;
    [SerializeField] private TMP_Text textNbPagesJournal;
    [SerializeField] private List<PageGroup> listGroupPagesJournal; // Contains the list of journal "books"
    private List<Button> listButtonsPagesJournal;

    [Header("Tips attributes")]
    [SerializeField] private GameObject listButtonTips; // Contains the list of tips page buttons
    [SerializeField] private GameObject contentTips;
    [SerializeField] private List<PageGroup> listGroupPagesTips; // Contains the list of tips "books"
    private List<Button> listButtonsPagesTips;

    private PlayerInput playerInput;
    private bool isInventoryOpen = false;
    private bool canQuitInventory = false;
    private int currentJournalPageNumber = 0;
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
            button.onClick.AddListener(() => OnKeyPressed(ind));
            Debug.Log("ind = " + ind);
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




    private void OnKeyPressed(int keyIndex)
    {
        Debug.Log("INDEXXXXXXXX = " + keyIndex);
    }




    private void Start()
    {
        playerInput = GameModeSingleton.GetInstance().GetPlayerReference.GetComponent<PlayerInput>();
        textMoneyAmount.text = money.ToString();
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
        StartCoroutine(Wait());
    }

    public void OpenJournalTab()
    {
        currentJournalPageNumber = 0;

        listButtonJournal.SetActive(true);
        contentJournal.SetActive(true);

        listButtonTips.SetActive(false);
        contentTips.SetActive(false);
    }

    public void OpenTipsTab()
    {
        currentTipsPageNumber = 0;

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

    // Open the journal page with a button
    public void OpenJournalPage()
    {
        // index du bouton pour connaître l'index du journal
        

        //listButtonsPagesJournal
        //Debug.Log()

        // text du content = text de la 1ère page du journal à tel indice
    }


    public void NextJournalPage(int indexJournal)
    {
        if (listGroupPagesJournal[indexJournal].pages.Count > 0)
        {
            currentJournalPageNumber++;
            //textNbPagesJournal.text = listGroupPagesJournal[indexJournal].pages

            // Si currentJournalPageNumber >= 0, on active le bouton PreviousJournal
            // Si currentJournalPageNumber == nbPage.Count, on désactive le bouton NextJournal
        }


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
