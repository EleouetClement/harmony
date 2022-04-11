using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    private int currentIndexMenu = 0;
    private int money { get; set; } = 0;

    public string lockedText = "?";

    [Header("Journal attributes")]
    [SerializeField] private GameObject contentPagesJournal;
    [SerializeField] private List<PageGroup> listGroupPagesJournal;
    private List<Button> listButtonsPagesJournal;

    [Header("Tips attributes")]
    [SerializeField] private GameObject contentPagesTips;
    [SerializeField] private List<PageGroup> listGroupPagesTips;
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

        listButtonsPagesJournal = new List<Button>();
        listButtonsPagesTips = new List<Button>();

        int ind = 0;
        foreach (Button button in contentPagesJournal.GetComponentsInChildren<Button>())
        {
            TMP_Text txt = button.GetComponentInChildren<TMP_Text>();

            if (listGroupPagesJournal[ind].isUnlocked)
            {
                txt.text = listGroupPagesJournal[ind].nameGroup;
            }
            else
            {
                txt.text = lockedText;
            }

            listButtonsPagesJournal.Add(button);
            ind++;
        }

        ind = 0;
        foreach (Button button in contentPagesTips.GetComponentsInChildren<Button>())
        {
            TMP_Text txt = button.GetComponentInChildren<TMP_Text>();

            if (listGroupPagesTips[ind].isUnlocked)
            {
                txt.text = listGroupPagesTips[ind].nameGroup;
            }
            else
            {
                txt.text = lockedText;
            }

            listButtonsPagesTips.Add(button);
            ind++;
        }


        if (listGroupPagesJournal.Count != listButtonsPagesJournal.Count)
        {
            Debug.LogError("Size of -List Group Pages Journal- is different from size of -Content Pages Journal- in InventoryManager");
        }
        if (listGroupPagesTips.Count != listButtonsPagesTips.Count)
        {
            Debug.LogError("Size of -List Group Pages Tips- is different from size of -Content Pages Tips- in InventoryManager");
        }




    }

    public void AddMoney(int money)
    {
        this.money += money;
        // TODO Update UI
    }

    public void OpenInventory()
    {

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

    public void ExitInventory()
    {

    }
}
