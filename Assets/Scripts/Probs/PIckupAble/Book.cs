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
    [SerializeField] PageType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Livre d'index " + index);
        InventoryManager inv = InventoryManager.instance;
        if(type.Equals(PageType.Journal))
        {
            inv.UnlockJournalInInventory(index);
        }
        else
        {
            inv.UnlockTipInInventory(index);
        }
        PlayPickUpSound();
    }

    private void PlayPickUpSound()
    {
        //Activate pickup sound before destroying item
        //TO DO...
    }
}
