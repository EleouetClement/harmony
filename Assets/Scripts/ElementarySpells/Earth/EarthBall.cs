using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBall : MonoBehaviour
{
	public EarthMortar earthMortarRef;
	private bool launched;
	public float charge;

	private Vector3 launchVelocity;

	private float range;
	/// <summary>
	///	Defines max mass of elementary at max charge level
	/// </summary>
	public float maxMass;

	/// <summary>
	///	Defines min and max range at max charge level
	/// </summary>
	public float minRange;
	public float maxRange;

	/// <summary>
	///	Defines min and max size at max charge level
	/// </summary>
	private Vector3 minSize;
	public Vector3 maxSize;

	
	/// <summary>
	///	Defines min and max radius of impact at max charge level
	/// </summary>
	public float minRadius;
	public float maxRadius;

	private float radius;

	/// <summary>
	///	Defines min and max impact strength at max charge level
	/// </summary>
	public float minImpactForce;
	public float maxImpactForce;

	private float impactforce;

	private TrajectoryCalculator trajectoryCalculator;
	private PhysicsSimulator physicsSimulator;


	// Start is called before the first frame update
	void Start()
	{
		trajectoryCalculator = GetComponent<TrajectoryCalculator>();

		//physicsSimulator = GetComponent<PhysicsSimulator>();
		//physicsSimulator.Init();

		launched = false;
		minSize = earthMortarRef.elementary.transform.localScale;
		maxSize += minSize;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (!launched)
		{
			//earth ball orbits player
			transform.position = earthMortarRef.elementary.transform.position;
			//earth ball increases in size with charge level
			transform.localScale = minSize + charge * (maxSize - minSize);
			//earth ball radius increases with charge level
			radius = minRadius + charge * (maxRadius - minRadius);

			range = minRange + charge * (maxRange - minRange);

			launchVelocity = ((transform.forward+transform.up/2f)*charge) * range;

			trajectoryCalculator.initialVelocity = launchVelocity;
			trajectoryCalculator.CalculateTrajectory();

			//physicsSimulator.currentVelocity = launchVelocity;
			//physicsSimulator.Init();

			impactforce = minImpactForce + charge * (maxImpactForce - minImpactForce);

		}
		else
		{

		}
		
	}

	public void Launch()
	{

		//bug
		//float mass = (charge / 100.0f) * maxMass; 
		//GetComponent<Rigidbody>().mass = mass;

		GetComponent<LineRenderer>().enabled = false;
		launched = true;
		Debug.Log("Range : " + range);
		Debug.Log("launchVelocity : " + launchVelocity);
		//Vector3 launchDirection = target - elementary.transform.position;
		earthMortarRef.elementary.GetComponent<ElementaryController>().computePosition = false;
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<SphereCollider>().isTrigger = false;
		//GetComponent<Rigidbody>().AddForce(transform.forward * range, ForceMode.Impulse);
		GetComponent<Rigidbody>().velocity = launchVelocity;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.name != earthMortarRef.elementary.gameObject.name)
		{
			Collider[] near = Physics.OverlapSphere(transform.position, radius);
			foreach (var collider in near)
			{
				Rigidbody rig = collider.GetComponent<Rigidbody>();
				if (rig != null)
				{
					rig.AddExplosionForce(impactforce, transform.position, radius, 1f, ForceMode.Impulse);
				}

			}
			earthMortarRef.lastBallCoord = transform.position;
			Destroy(gameObject);
		}
	}
}
