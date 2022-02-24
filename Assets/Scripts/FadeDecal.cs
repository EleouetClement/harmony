using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

    [RequireComponent(typeof(SelfDestruct))]
    [RequireComponent(typeof(DecalProjector))]

public class FadeDecal : MonoBehaviour
{
    [TextArea]
    public string notes;

    private DecalProjector projector;
    private float lifeTime;
    private float timePassed = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        projector = GetComponent<DecalProjector>();
        lifeTime = GetComponent<SelfDestruct>().LivingTime;
    }

    // Update is called once per frame
    void Update()
    {
        projector.fadeFactor = 1f - timePassed / lifeTime;
        timePassed += Time.deltaTime;
    }
}
