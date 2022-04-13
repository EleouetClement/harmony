using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryMenu;

    private int money { get; set; } = 0;

    public string lockedText = "?"; // Visible text when the page is still locked

    [Header("Journal attributes")]
    [SerializeField] private GameObject contentPagesJournal; // Contains the list of journal page buttons
    [SerializeField] private List<PageGroup> listGroupPagesJournal; // Contains the list of journal "books"
    private List<Button> listButtonsPagesJournal;

    [Header("Tips attributes")]
    [SerializeField] private GameObject contentPagesTips; // Contains the list of tips page buttons
    [SerializeField] private List<PageGroup> listGroupPagesTips; // Contains the list of tips "books"
    private List<Button> listButtonsPagesTips;

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

        // FOR JOURNAL PAGES :
        // For each button, get the text and modify it depending on whether they are blocked or not
        int ind = 0;
        foreach (Button button in contentPagesJournal.GetComponentsInChildren<Button>())
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
            listButtonsPagesJournal.Add(button);
            ind++;
        }

        // FOR TIPS PAGES :
        // For each button, get the text and modify it depending on whether they are blocked or not
        ind = 0;
        foreach (Button button in contentPagesTips.GetComponentsInChildren<Button>())
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

    public void AddMoney(int money)
    {
        this.money += money;
        // TODO Update UI
    }

    public void OpenInventory()
    {
        Time.timeScale = 0f;
        inventoryMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OpenJournalTab()
    {
        contentPagesJournal.SetActive(true);
        contentPagesTips.SetActive(false);
    }

    public void OpenTipsTab()
    {
        contentPagesJournal.SetActive(false);
        contentPagesTips.SetActive(true);
    }

    public void CloseInventory()
    {
        inventoryMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
        return true;
    }
    #endregion

}
