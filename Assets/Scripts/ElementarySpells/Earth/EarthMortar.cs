using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthMortar : AbstractSpell
{
	public GameObject earthBall;
	private GameObject ball;
	private EarthBallMarker earthMarker;
	public Vector3 lastBallCoord;
	[Min(1)] public float maxSpellLength;
	public float maxRange;
	

	

	private void Start()
	{
		maxCastTime = 1.5f;
		
	}

	public override void init(GameObject elemRef, Vector3 target)
	{
		base.init(elemRef, target);
		earthMarker = GetComponent<EarthBallMarker>();
		earthMarker.Init(maxRange, earthMarker.transform.GetChild(0).gameObject);
		elementary.GetComponent<MeshRenderer>().enabled = false;
		//elementary.GetComponent<SphereCollider>().enabled = false;
		ball = Instantiate(earthBall, elementary.transform.position, Quaternion.identity);
		ball.GetComponent<EarthBall>().earthMortarRef = this;
		ball.GetComponent<EarthBall>().earthMarkerRef = earthMarker;
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (ball == null)
		{
			elementary.transform.position = lastBallCoord;
			elementary.GetComponent<MeshRenderer>().enabled = true;
			elementary.GetComponent<ElementaryController>().currentSpell = null;
			elementary.GetComponent<ElementaryController>().computePosition = true;
			Destroy(gameObject);
		}
		else 
		{
			//refreshes charge level to earth ball
			ball.GetComponent<EarthBall>().charge = charge / maxCastTime;
		}

	}
	protected override void onChargeEnd(float chargetime)
	{
		earthMarker.DestroyMarker();
		ball.GetComponent<EarthBall>().Launch();
	}

	public override void Terminate()
	{
		
	}
}
