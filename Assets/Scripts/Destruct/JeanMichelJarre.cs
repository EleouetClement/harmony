using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class JeanMichelJarre : BreakableItem
{
    public GameObject breakableJarre;
    private GameObject destroyed;

    private float elapsedTime = 0f;
    public float fadeDelay = 10f;
    public float alphaValue = 0;
    public bool destroyGameObject = false;


    protected override void Update()
    {

    }

    public override void Break()
    {
        destroyed = Instantiate(breakableJarre, new Vector3(transform.position.x, (transform.position.y + 0.5f) * transform.localScale.y, transform.position.z), Quaternion.identity);
        Destroy(gameObject);
    }
}
