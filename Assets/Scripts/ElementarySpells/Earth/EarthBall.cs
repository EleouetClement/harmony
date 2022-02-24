using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EarthBall : MonoBehaviour
{
	public EarthMortar earthMortarRef;
	public EarthBallMarker earthMarkerRef;
	private bool launched;

	public GameObject decalPrefab;
	public GameObject explosionPrefab;

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

	private float range;
	/// <summary>
	///	Defines min and max range at max charge level when free aiming
	/// </summary>
	[Header("Free aiming min and max range")]
	public float minRange;
	public float maxRange;
	

	/// <summary>
	///	Defines min and max size at max charge level
	/// </summary>
	private Vector3 minSize;
	private Vector3 maxSize;


	/// <summary>
	///	Defines min and max radius of impact at max charge level
	/// </summary>
    [Space]
	public float minRadius;
	public float maxRadius;

	private float radius;

	/// <summary>
	///	Defines min and max impact strength at max charge level
	/// </summary>
	public float minImpactForce;
	public float maxImpactForce;

	private float impactforce;

	private Vector3 markerScale;

	private Rigidbody rig;



	private TrajectoryCalculator trajectoryCalculator;

	Cinemachine.CinemachineImpulseSource shakeSource;

	// Start is called before the first frame update
	void Start()
	{
		rig = GetComponent<Rigidbody>();
		trajectoryCalculator = GetComponent<TrajectoryCalculator>();

		if (displayTrajectory)
			earthMarkerRef.trajectoryCalculator = trajectoryCalculator;


		launched = false;
		minSize = transform.localScale;
		maxSize = minSize * 2f;

		launchVelocity = trajectoryCalculator.CalculateVelocity(transform.position, earthMarkerRef.Aim(), speed);
		transform.rotation = Quaternion.LookRotation(launchVelocity);

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

			range = minRange + charge * (maxRange - minRange);

			if(earthMarkerRef.freeAim)
				launchVelocity = earthMarkerRef.Aim() * range;
			else
				launchVelocity = trajectoryCalculator.CalculateVelocity(transform.position, earthMarkerRef.Aim(), speed);
			trajectoryCalculator.SetInitialVelocity(launchVelocity);
			trajectoryCalculator.CalculateTrajectory();
			earthMarkerRef.SetMarkerRadius(radius);

			impactforce = minImpactForce + charge * (maxImpactForce - minImpactForce);


			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(launchVelocity) , Time.deltaTime * 5);
		}
		else
		{
			transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(rig.velocity.normalized),Time.deltaTime * 5);
		}

	}

	private void Update()
	{
		
		if (earthMarkerRef && earthMarkerRef.markerInstance)
		{
			markerScale = earthMarkerRef.markerInstance.transform.localScale;
		}

	}

	public void Launch()
	{
		GetComponent<LineRenderer>().enabled = false;
		earthMortarRef.elementary.GetComponent<ElementaryController>().computePosition = false;
		rig.isKinematic = false;
		//GetComponent<SphereCollider>().isTrigger = false;
		rig.velocity = launchVelocity;
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
			//Apply decal
			ContactPoint contactPoint = collision.GetContact(0);
			GameObject decalInstance = Instantiate(decalPrefab, contactPoint.point + contactPoint.normal.normalized * 0.5f, Quaternion.LookRotation(-1f * contactPoint.normal));
			decalInstance.transform.GetComponent<DecalProjector>().size = new Vector3(markerScale.x,markerScale.z,Vector3.Distance(decalInstance.transform.position, contactPoint.point));

			//Apply explosion
			GameObject explosionInstance = Instantiate(explosionPrefab, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
			explosionInstance.transform.localScale = Vector3.one * radius;


			shakeSource.GenerateImpulseAt(transform.position,transform.forward);
			// Idamageable computations
			//Debug.Log("Earth mortal shockwave at : " + transform.position + " / radius : " + radius);
			Collider[] enemies = Physics.OverlapCapsule(transform.position + Vector3.down * 3, transform.position, radius, 1 << HarmonyLayers.LAYER_TARGETABLE);
			if (enemies.Length >= 1)
				foreach (Collider c in enemies)
				{
					c.gameObject.GetComponent<IDamageable>()?.OnDamage(new DamageHit(100f, GameEngineInfo.DamageType.Earth, Vector3.up));
				}
			//get position to move elementary
			earthMortarRef.lastBallCoord = transform.position;
			// Destroy the ball
			GetComponent<Rigidbody>().isKinematic = true;
			GetComponentInChildren<MeshRenderer>().enabled = false;
			GetComponentInChildren<MeshCollider>().enabled = false;
			//Destruction handled by component <SelfDestruct>
			GetComponent<SelfDestruct>().enabled = true;
			GetComponent<EarthBall>().enabled = false;
		}
	}
}
