using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthMortar : AbstractSpell
{
	public GameObject earthBall;
	private GameObject ball;
	private EarthBallMarker earthMarker;
	public Vector3 lastBallCoord;
	[Min(1)] public float maxSpellLength;
	public float maxRange;

	GameObject hud;
	

	private void Start()
	{
		maxCastTime = 1.5f;
		blinkTiming = 1f;
		hud = GameObject.Find("GameManager").GetComponent<GameModeSingleton>().GetPlayerHUD;
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
			hud.transform.Find("Charge bar").gameObject.GetComponent<Slider>().enabled = false;
			Destroy(gameObject);
		}
		else 
		{
			//refreshes charge level to earth ball
			ball.GetComponent<EarthBall>().charge = charge / maxCastTime;
			hud.transform.Find("Charge bar").gameObject.GetComponent<Slider>().enabled = true;
			print("non");
			hud.transform.Find("Charge bar").gameObject.GetComponent<Slider>().value = charge / maxCastTime;
		}

		
			

	}
	protected override void onChargeEnd(float chargetime)
	{
		earthMarker.DestroyMarker();

		if (chargetime >= 1.05f || chargetime <= 9.95f)
			print("Blink!");
		Debug.Log("yes");
		//hud.transform.Find("Charge bar").gameObject.GetComponent<Slider>().enabled = false;
		ball.GetComponent<EarthBall>().Launch();
	}

	public override void Terminate()
	{
		
	}
}
