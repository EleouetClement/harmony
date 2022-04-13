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
    private bool triggered = false;
    private 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                ActivateBook();
            }
        }
           
    }

    private void OnTriggerEnter(Collider other)
    {
        EnableInput();
    }

    private void OnTriggerExit(Collider other)
    {
        UnableInput();
    }

    private void EnableInput()
    {
        //Ui button activation
        triggered = true;

    }

    private void UnableInput()
    {
        //Ui button deactivation
        triggered = false;

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
