using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTaker : MonoBehaviour
{
    [SerializeField][Min(1)] int value = 1;
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
        ShutUpAndTakeMyMoney();
    }


    private void ShutUpAndTakeMyMoney()
    {
        InventoryManager.instance.AddMoney(value);
        PlayPickupSound();
        Destroy(gameObject);
    }

    private void PlayPickupSound()
    {
        //TO DO...
    }
}
