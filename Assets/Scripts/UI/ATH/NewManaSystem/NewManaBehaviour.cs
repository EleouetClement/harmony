using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewManaBehaviour : MonoBehaviour
{

    private GameModeSingleton gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameModeSingleton.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToWarning()
    {

    }

    public void SwitchToCritical()
    {

    }
}
