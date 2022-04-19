using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewHealthManager : MonoBehaviour
{
    [SerializeField][Min(0)] private float lightFadeSpeed = 1;
    [SerializeField][Min(0)] private float heavyFadeSpeed = 1;
    private GameObject lightDamages;
    private GameObject heavyDamages;
    private bool fadeLight = false;
    private bool fadeHeavy = false;

    private Image lightDmgImage;
    private Image heavyDmgImage;


    private void Awake()
    {
        if(transform.childCount < 2)
        {
            Debug.LogError("NewHealthManager : no sprites for damages");
            Destroy(gameObject);
        }
        else
        {
            lightDamages = transform.GetChild(0).gameObject;
            heavyDamages = transform.GetChild(1).gameObject;
            lightDmgImage = lightDamages.GetComponent<Image>();
            heavyDmgImage = heavyDamages.GetComponent<Image>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(fadeHeavy)
        {
            fadeHeavy = FadeDmg(heavyDmgImage, heavyFadeSpeed);
        }
        else if(fadeLight)
        {
            fadeLight = FadeDmg(lightDmgImage, lightFadeSpeed);
        }
    }

    /// <summary>
    /// Set the lightDamages UI gameObject to "state"
    /// </summary>
    /// <param name="state"></param>
    public void SetLightDamages(bool state)
    {
        lightDamages.SetActive(state);
    }
    /// <summary>
    /// Set the heavyDamages UI gameObject to "state"
    /// </summary>
    /// <param name="state"></param>
    public void SetHeavyDamages(bool state)
    {
        heavyDamages.SetActive(state);
    }

    /// <summary>
    /// Make the heavy damages sprite fade to light
    /// </summary>
    public void FadeHeavyDamages()
    {
        fadeHeavy = true;
    }
    /// <summary>
    /// Make the light damages sprite fade to base state
    /// </summary>
    public void FadeLightDamages()
    {
        fadeLight = true;
    }
    
    private bool FadeDmg(Image image, float fadeSpeed = 1)
    {
        Color pouet = image.color;
        pouet.a = pouet.a <= 0 ? 0 : (pouet.a - (fadeSpeed * Time.deltaTime));
        image.color = pouet;
        return pouet.a > 0;
    }

}
