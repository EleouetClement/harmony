using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewManaBehaviour : MonoBehaviour
{
    [SerializeField][Range(0.01f, 0.05f)] private float minimumValue;
    [SerializeField][Min(0)] private float fadeSpeed;
    private GameModeSingleton gm;
    private PlayerGameplayController pgc;
    public static NewManaBehaviour instance = null;
    private Transform actualBarre;
    private float maxValue;

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
        maxValue = transform.GetChild(2).localScale.x;
        if (!pgc)
        {
            Debug.LogError("NewManaBehaviour : playerGameplayControllerReference is null");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        actualBarre.localScale = new Vector3(pgc.GetMana(), actualBarre.localScale.y, actualBarre.localScale.z);
        if (actualBarre.localScale.x <= 0)
        {
            Debug.Log("Allo !");
            actualBarre.localScale = new Vector3(minimumValue, actualBarre.localScale.y, actualBarre.localScale.z);
            FadeBar();
        }
        else
        {
            if(transform.GetChild(0).GetComponent<Image>().color.a != 1)
                AppearBar();
        }
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
        transform.GetChild(0).localScale += new Vector3(value, 0, 0);
        transform.GetChild(2).localScale += new Vector3(value, 0, 0);
        maxValue = transform.GetChild(2).localScale.x;
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

    private void FadeBar()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Color pouet = transform.GetChild(i).GetComponent<Image>().color;
            if (pouet.a > 0)
            {
                pouet.a -= fadeSpeed * Time.deltaTime;
                transform.GetChild(i).GetComponent<Image>().color = pouet;
            }
        }
    }

    private void AppearBar()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Color pouet = transform.GetChild(i).GetComponent<Image>().color;
            pouet.a = 1;
            transform.GetChild(i).GetComponent<Image>().color = pouet;
        }
    }
}
