using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Liana : BurnableItem
{
    [SerializeField] [Min(0)] private float delay;
    [SerializeField] [Min(0)] private float transformSpeed;
    [SerializeField] [Min(0)] private float scaleReductionSpeed;

    private float time = Mathf.Epsilon;
    protected override void Update()
    {
        
        if (triggered)
        {
            if(time >= delay)
            {
                if (transform.localScale.y > Mathf.Epsilon)
                {
                    Burn();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                time += Time.deltaTime;
            }
        }     
    }

    public override void Consume()
    {
        base.Consume();
        transformSpeed = scaleReductionSpeed + 0.6f;
        Debug.Log("Lianas on fire!");
    }

    private void Burn()
    {
        transform.Translate(Vector3.up * transformSpeed * Time.deltaTime);
        Vector3 newScale = Vector3.down * scaleReductionSpeed * Time.deltaTime;
        transform.localScale += newScale;

    }

}
