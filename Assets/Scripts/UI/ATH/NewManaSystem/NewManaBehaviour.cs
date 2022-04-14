using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewManaBehaviour : MonoBehaviour
{
    [SerializeField][Range(0.01f, 0.05f)] private float minimumValue;
    private GameModeSingleton gm;
    private PlayerGameplayController pgc;
    public static NewManaBehaviour instance = null;
    private Transform actualBarre;

    private void Awake()
    {
        if(instance)
        {
            Debug.LogError("NewManaBehaviour : multiple manabar instances");
            Destroy(gameObject);
        }
        instance = this;
        actualBarre = transform.GetChild(1);
        if(!actualBarre)
        {
            Debug.LogError("NewManaBehaviour : missing manabar");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gm = GameModeSingleton.GetInstance();
        pgc = gm.GetPlayerReference?.GetComponent<PlayerGameplayController>();
        if(!pgc)
        {
            Debug.LogError("NewManaBehaviour : playerGameplayControllerReference is null");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        actualBarre.localScale = new Vector3(pgc.GetMana(), actualBarre.localScale.y, actualBarre.localScale.z);
        if (transform.localScale.x <= 0)
            actualBarre.localScale = new Vector3(minimumValue, actualBarre.localScale.y, actualBarre.localScale.z);
    }

    public void UseMana(float amount)
    {
        float value = amount / 100;
        actualBarre.localScale += new Vector3(amount, 0, 0);
    }

    public void GainMana(float amount)
    {
        float value = amount / 100;
        actualBarre.localScale -= new Vector3(amount, 0, 0);
    }

    /// <summary>
    /// Increase the scale of the mana border on the x axis by Amount/100;
    /// </summary>
    /// <param name="amount"></param>
    public void IncreaseManaMax(float amount)
    {
        float value = amount / 100;
        transform.localScale += new Vector3(amount, 0, 0);
    }

    /// <summary>
    /// Switch the color of the bar to orange
    /// </summary>
    public void SwitchToWarning()
    {

    }

    /// <summary>
    /// Switch the color of the bar to red
    /// </summary>
    public void SwitchToCritical()
    {

    }
}
