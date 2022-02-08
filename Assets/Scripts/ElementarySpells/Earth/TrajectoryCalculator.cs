using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryCalculator : MonoBehaviour
{

	public int accuracy;
	private float timeFlight;

	public Vector3 initialVelocity;


	public void Start()
	{
	}

	public void CalculateTrajectory()
	{
		timeFlight = (-1f * initialVelocity.y) / Physics.gravity.y;
		timeFlight = 0.5f * timeFlight;
		Debug.Log(timeFlight);
		GetComponent<LineRenderer>().positionCount = accuracy;
		Vector3 trajectoryPoint;

		for (int i = 0; i < GetComponent<LineRenderer>().positionCount; i++)
		{
			float time = timeFlight * i / (float)(GetComponent<LineRenderer>().positionCount);
			trajectoryPoint = transform.position + initialVelocity * time + 0.5f * Physics.gravity * time * time;
			GetComponent<LineRenderer>().SetPosition(i, trajectoryPoint);
			
		}
	}
}
