using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBarBehavior : MonoBehaviour
{
    /// <summary>
    /// Reference to the slider component, created at object start
    /// </summary>
    private Slider sl;

    private GameModeSingleton gm;

    // Start is called before the first frame update
    void Start()
    {
        sl = GetComponent<Slider>();
        gm = GameModeSingleton.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        float fill = 0.2f;
        PlayerGameplayController player = gm?.GetPlayerReference?.GetComponent<PlayerGameplayController>();
        if (player)
            fill = player.getDisplayMana();
        sl.value = fill;
    }
}
