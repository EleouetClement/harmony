using UnityEngine;

public class FIreArea : MonoBehaviour
{

    [SerializeField] [Min(0)] private float livingTime;
    [SerializeField] [Min(0)] private float timeToChangeScale = 3f;

    [HideInInspector] public bool isFadingAway = false;
    private Vector3 initialSpawnScale;
    private float scaleTimer = 0f;
    private float liveTimer = Mathf.Epsilon;
    void Start()
    {
        initialSpawnScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(liveTimer >= livingTime)
        {
            Destroy(gameObject);
        }
        else if (liveTimer >= livingTime - timeToChangeScale) // If it is time to make disappear the flames, we notice that
        {
            // Fade out
            isFadingAway = true;
        }

        if(isFadingAway)
        {
            ExtinguishFlames();
        }

        liveTimer += Time.deltaTime;
    }

    public void ExtinguishFlames()
    {
        // Make a scale from initial shape to 0 to make the disappear effect
        scaleTimer += Time.deltaTime;
        transform.localScale = Vector3.Lerp(initialSpawnScale, Vector3.zero, (scaleTimer / timeToChangeScale));
    }


    private void OnTriggerEnter(Collider collider)
    {
        //Debug.Log(other.gameObject.layer);
        //TO DO...
    }

}
