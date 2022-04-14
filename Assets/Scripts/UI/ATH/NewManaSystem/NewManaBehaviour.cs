using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewManaBehaviour : MonoBehaviour
{
    [SerializeField][Range(0.01f, 0.05f)] private float minimumValue;

    private GameModeSingleton gm;
    private PlayerGameplayController pgc;
    public static NewManaBehaviour instance;


    private void Awake()
    {
        if(instance)
        {
            Debug.LogError("NewManaBehaviour : multiple manabar instances");
            Destroy(gameObject);
        }
        instance = this;
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
        transform.localScale = new Vector3(pgc.GetMana(), transform.localScale.y, transform.localScale.z);
        if (transform.localScale.x <= 0)
            transform.localScale = new Vector3(minimumValue, transform.localScale.y, transform.localScale.z);
    }

    public void UseMana(float amount)
    {
        float value = amount / 100;
        transform.localScale += new Vector3(amount, 0, 0);
    }

    public void GainMana(float amount)
    {
        float value = amount / 100;
        transform.localScale -= new Vector3(amount, 0, 0);
    }

    public void SwitchToWarning()
    {

    }

    public void SwitchToCritical()
    {

    }
}
