using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    public enum PageType
    {
        Journal,
        Tips,
    }
    [SerializeField] [Min(0)] int index = 0;
    [SerializeField] PageType type = PageType.Journal;
    [SerializeField] [Min(0)] float height;
    private bool triggered = false;
    private GameObject button;
    private Transform playersTransform;

    private void Awake()
    {
        if(transform.childCount > 0)
        {
            button = transform.GetChild(0).gameObject;
            button.SetActive(false);      
        }
        else
        {
            Debug.LogError("Book : No children for book, canvas must be missing");
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered)
        {
            //transform.LookAt(playersTransform);
            if (Input.GetKeyDown(KeyCode.E))
            {
                ActivateBook();
            }
        }
           
    }

    private void OnTriggerEnter(Collider other)
    {
        EnableInput();
        playersTransform = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        UnableInput();
    }

    private void EnableInput()
    {
        //Ui button activation
        triggered = true;
        button.SetActive(true);
    }

    private void UnableInput()
    {
        //Ui button deactivation
        triggered = false;
        button.SetActive(false);

    }

    private void ActivateBook()
    {
        Debug.Log("Livre d'index " + index);
        InventoryManager inv = InventoryManager.instance;
        if (type.Equals(PageType.Journal))
        {
            inv.UnlockJournalInInventory(index);
        }
        else
        {
            inv.UnlockTipInInventory(index);
        }

        PlayPickUpSound();
        Destroy(gameObject);
    }

    private void PlayPickUpSound()
    {
        //Activate pickup sound before destroying item
        //TO DO...
    }
}
