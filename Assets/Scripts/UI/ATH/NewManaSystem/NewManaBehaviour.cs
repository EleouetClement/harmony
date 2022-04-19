using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewManaBehaviour : MonoBehaviour
{
    [SerializeField][Range(0.01f, 0.05f)] private float minimumValue;
    [SerializeField][Min(0)] private float fadeSpeed;
    [SerializeField] Color baseColor = Color.white;
    [SerializeField] Color warningColor = Color.yellow;
    [SerializeField] Color CriticalColor = Color.red;
    
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
            
            actualBarre.localScale = new Vector3(minimumValue, actualBarre.localScale.y, actualBarre.localScale.z);
            FadeBar();
        }
        else
        {
            if(transform.GetChild(0).GetComponent<Image>().color.a != 1)
                AppearBar();
        }
        if(actualBarre.localScale.x >= pgc.GetCriticalTreshhold / 100 || pgc.IsManaBurnout)
        {
            SwitchToCritical();
        }
        else if(actualBarre.localScale.x >= pgc.GetWarningTreshhold / 100)
        {
            SwitchToWarning();
        }
        else if (actualBarre.GetComponent<Image>().color != baseColor && !(actualBarre.localScale.x <= minimumValue))
        {
            SwitchToBase();
        }

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
    private void SwitchToWarning()
    {
        actualBarre.GetComponent<Image>().color = warningColor;
    }

    /// <summary>
    /// Switch the color of the bar to red
    /// </summary>
    private void SwitchToCritical()
    {
        actualBarre.GetComponent<Image>().color = CriticalColor;
    }
    private void SwitchToBase()
    {
        actualBarre.GetComponent<Image>().color = baseColor;
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
