using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBall : MonoBehaviour
{
    public EarthMortar earthMortarRef;
    private bool launched;
    public float range;
    /// <summary>
    ///	Defines max mass of elementary at max charge level
    /// </summary>
    public float maxMass;

    /// <summary>
    ///	Defines max range at max charge level
    /// </summary>
    [Min(0)] public float maxRange;

    // Start is called before the first frame update
    void Start()
    {
        launched = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!launched)
        {
            transform.position = earthMortarRef.elementary.transform.position;
        }
		else
		{
            
		}
    }

    public void Launch(float charge)
    {
        Debug.Log("oui");

        //bug
        //float mass = (charge / 100.0f) * maxMass; 
        //GetComponent<Rigidbody>().mass = mass;

        range = charge * maxRange;
        Debug.Log(range);
        launched = true;
        //Vector3 launchDirection = target - elementary.transform.position;
        earthMortarRef.elementary.GetComponent<ElementaryController>().computePosition = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(earthMortarRef.elementary.transform.forward * range, ForceMode.Impulse);
    }

	private void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.name != earthMortarRef.elementary.gameObject.name)
        {
            
            earthMortarRef.lastBallCoord = collision.transform.position;
            Destroy(gameObject);
        }
	}
}
