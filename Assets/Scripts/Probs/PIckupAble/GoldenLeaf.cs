using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenLeaf : MonoBehaviour
{

    [SerializeField][Min(1)] int manaIncreaseAmount;
    private GameObject button;
    private bool triggered;

    private void Awake()
    {
        if (transform.childCount > 0)
        {
            button = transform.GetChild(0).gameObject;
            button.SetActive(false);
        }
        else
        {
            Debug.LogError("Leaf : No children for Leaf, canvas must be missing");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered)
        {
            //transform.LookAt(playersTransform);
            if (Input.GetKeyDown(KeyCode.E))
            {
                IncreaseMana();
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
        button.SetActive(true);
    }

    private void UnableInput()
    {
        //Ui button deactivation
        triggered = false;
        button.SetActive(false);
    }

    /// <summary>
    /// Increase player's mana max
    /// </summary>
    private void IncreaseMana()
    {
        GameModeSingleton.GetInstance()?.GetPlayerReference.GetComponent<PlayerGameplayController>()?.IncreaseMana(manaIncreaseAmount);
    }

    private void PlayPickUpSound()
    {
        //Activate pickup sound before destroying item
        //TO DO...
    }


}
