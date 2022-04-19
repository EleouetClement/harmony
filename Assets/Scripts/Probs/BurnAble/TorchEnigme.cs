using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TorchEnigme : BurnableItem
{
    public GameObject porte;
    private bool activated = false;
    public float duration = 1f;
    private float elapsedTime = 0f;
    private Vector3 positionInitial = Vector3.zero;
    private Vector3 positionFinal = Vector3.zero;
    public bool ignited;


    public void Start()
    {
        if(ignited)
        {
            base.Consume();
        }
    }
    protected override void Update()
    {
        if (triggered && !activated)
        {
            if(positionInitial == Vector3.zero && positionFinal == Vector3.zero)
            {
                positionInitial = porte.transform.position;
                positionFinal = porte.transform.position + new Vector3(0, 0.65f, 0);
            }
            elapsedTime += Time.deltaTime;
            elapsedTime = Mathf.Clamp01(elapsedTime);
            porte.transform.position = Vector3.Lerp(positionInitial, positionFinal, elapsedTime / duration);
            if(elapsedTime >= duration)
            {
                activated = true;
            }
        }
    }

    public override void Consume()
    {
        base.Consume();
    }
}