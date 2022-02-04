using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthMortar : AbstractSpell
{
	public GameObject earthBall;
	private GameObject ball;
	public Vector3 lastBallCoord;

	[Min(1)] public float maxSpellLength;


	

	private void Start()
	{
		maxCastTime = 1.5f;
		
	}

	public override void init(GameObject elemRef, Vector3 target)
	{
		base.init(elemRef, target);
		elementary.GetComponent<MeshRenderer>().enabled = false;
		//elementary.GetComponent<SphereCollider>().enabled = false;
		ball = Instantiate(earthBall, elementary.transform.position, Quaternion.identity);
		ball.GetComponent<EarthBall>().earthMortarRef = this;
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
			ball.GetComponent<EarthBall>().charge = charge / maxCastTime;
		}

	}
	protected override void onChargeEnd(float chargetime)
	{
		
		ball.GetComponent<EarthBall>().Launch();
	}

	public override void Terminate()
	{
		
	}
}
