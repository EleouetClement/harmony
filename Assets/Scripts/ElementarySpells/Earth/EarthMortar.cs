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
	private bool launched;

	GameObject hud;
	

	private void Start()
	{
		maxCastTime = 1.5f;
		blinkTiming = 1f;
		hud = GameObject.Find("GameManager").GetComponent<GameModeSingleton>().GetPlayerHUD;
		hud.transform.Find("Charge bar").gameObject.GetComponent<Slider>().enabled = true;
		launched = false;
	}

	public override void init(GameObject elemRef, Vector3 target)
	{
		base.init(elemRef, target);
		element = Element.Earth;
		earthMarker = GetComponent<EarthBallMarker>();
		earthMarker.Init(maxRange, earthMarker.transform.GetChild(0).gameObject);
		elementary.GetComponent<MeshRenderer>().enabled = false;
		//elementary.GetComponent<SphereCollider>().enabled = false;
		ball = Instantiate(earthBall, elementary.transform.position, Quaternion.identity);
		ball.transform.rotation.SetLookRotation(Quaternion.LookRotation(GameModeSingleton.GetInstance().GetPlayerMesh.forward, GameModeSingleton.GetInstance().GetPlayerMesh.up).eulerAngles);
		ball.GetComponent<EarthBall>().earthMortarRef = this;
		ball.GetComponent<EarthBall>().earthMarkerRef = earthMarker;
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();

		if (ball == null)
		{
			Terminate();
		}
		else if(!launched)
		{
			if (earthMarker.markerInstance)
			{
				ball.transform.rotation.SetLookRotation(earthMarker.markerInstance.transform.position - ball.transform.position);
				print("ici");
			}
			//refreshes charge level to earth ball
			ball.GetComponent<EarthBall>().charge = charge / maxCastTime;
			elementary.GetComponent<ElementaryController>().computePosition = true;
			hud.transform.Find("Charge bar").gameObject.GetComponent<Slider>().value = charge / maxCastTime;
		}
		else
		{
			//hud.transform.Find("Charge bar").gameObject.GetComponent<Slider>().enabled = false;
			hud.GetComponent<Canvas>().enabled = false;
		}

		
			

	}
	protected override void onChargeEnd(float chargetime)
	{
		earthMarker.DestroyMarker();

		if (chargetime >= 1.05f || chargetime <= 9.95f)
			//print("Blink!");
		ball.GetComponent<EarthBall>().Launch();
		launched = true;
	}

	public override void Terminate()
	{
		elementary.transform.position = lastBallCoord;
		elementary.GetComponent<MeshRenderer>().enabled = true;
		elementary.GetComponent<ElementaryController>().currentSpell = null;
		elementary.GetComponent<ElementaryController>().computePosition = true;
		elementary.GetComponent<ElementaryController>().readyToCast = true;
		Destroy(gameObject);
	}
}
