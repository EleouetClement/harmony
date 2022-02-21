using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBall : MonoBehaviour
{
	public EarthMortar earthMortarRef;
	public EarthBallMarker earthMarkerRef;
	private bool launched;

	GameObject gameManager;

	public bool displayTrajectory;

	/// <summary>
	/// current charge level of the spell
	/// </summary>
	public float charge;

	/// <summary>
	/// Initial earth ball velocity
	/// </summary>
	private Vector3 launchVelocity;

	/// <summary>
	/// Defines the time it takes for the ball to reach target
	/// </summary>
	public float speed;

	/// <summary>
	///	Defines max mass of elementary at max charge level
	/// </summary>
	public float maxMass;

	//private float range;
	///// <summary>
	/////	Defines min and max range at max charge level
	///// </summary>
	//public float minRange;
	//public float maxRange;

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

	Cinemachine.CinemachineImpulseSource shakeSource;



	// Start is called before the first frame update
	void Start()
	{
		trajectoryCalculator = GetComponent<TrajectoryCalculator>();
		

		launched = false;
		minSize = earthMortarRef.elementary.transform.localScale;
		maxSize += minSize;

		gameManager = GameObject.Find("GameManager");

		shakeSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
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

			//range = minRange + charge * (maxRange - minRange);

			//launchVelocity = ((transform.forward+transform.up/2f)*charge) * range;
			trajectoryCalculator.SetInitialVelocity(launchVelocity);
			launchVelocity = trajectoryCalculator.CalculateVelocity(transform.position, earthMarkerRef.GetTarget(), speed);
			earthMarkerRef.SetMarkerRadius(radius);

			impactforce = minImpactForce + charge * (maxImpactForce - minImpactForce);

		}
		
	}

	private void Update()
	{
		if (displayTrajectory)
			earthMarkerRef.trajectoryCalculator = trajectoryCalculator;
		else
			earthMarkerRef.trajectoryCalculator = null;
	}

	public void Launch()
	{
		GetComponent<LineRenderer>().enabled = false;
		earthMortarRef.elementary.GetComponent<ElementaryController>().computePosition = false;
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<SphereCollider>().isTrigger = false;
		GetComponent<Rigidbody>().velocity = launchVelocity;
		launched = true;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.name != earthMortarRef.elementary.gameObject.name)
		{
			// Knockback computations
			Collider[] near = Physics.OverlapSphere(transform.position, radius);
			foreach (var collider in near)
			{
				Rigidbody rig = collider.GetComponent<Rigidbody>();
				if (rig != null)
				{
					rig.AddExplosionForce(impactforce, transform.position, radius, 1f, ForceMode.Impulse);
				}
			}
			shakeSource.GenerateImpulseAt(transform.position,transform.forward);
			earthMortarRef.lastBallCoord = transform.position;
			// Idamageable computations
			Debug.Log("Earth mortal shockwave at : " + transform.position + " / radius : " + radius);
			Collider[] enemies = Physics.OverlapCapsule(transform.position + Vector3.down * 3, transform.position, radius, 1 << HarmonyLayers.LAYER_TARGETABLE);
			if (enemies.Length >= 1)
				foreach (Collider c in enemies)
				{
					c.gameObject.GetComponent<IDamageable>()?.OnDamage(new DamageHit(100f, GameEngineInfo.DamageType.Earth, Vector3.up));
				}
			// Destroy the ball
			Destroy(gameObject);
		}
	}
}
