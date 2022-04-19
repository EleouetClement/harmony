using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeanMichelJarreteDetruit : MonoBehaviour
{
    private float elapsedTime = 0f;
    public float fadeDelay = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        elapsedTime = Mathf.Clamp(elapsedTime, 0f, fadeDelay);

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            MeshRenderer piece = gameObject.transform.GetChild(i).GetComponent<MeshRenderer>();
            float alpha = 1f - (elapsedTime / fadeDelay);
            piece.material.color = new Color(1f, 1f, 1f, alpha);
            if(alpha == 0f)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
        }

        if(gameObject.transform.childCount == 0)
        {
            Destroy(this.gameObject);
        }
    }
}
