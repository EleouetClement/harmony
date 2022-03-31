using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryCalculator : MonoBehaviour
{

	[Range(0f,2f)]
	public float length = 1f;

	/// <summary>
	/// Defines the number of points calculated to draw the preview trajectory
	/// </summary>
	public int accuracy;

	/// <summary>
	/// represents the time needed to reach max height in trajectory
	/// </summary>
	private float timeFlight;

	Vector3 initialVelocity;

	public void Start()
	{
	}

	/// <summary>
	/// Calculate and Draws a preview trajectory of projectile
	/// </summary>
	public void CalculateTrajectory()
	{
		timeFlight = (-1f * (initialVelocity.y + transform.position.y)) / Physics.gravity.y;
		timeFlight = length * timeFlight;
		//Debug.Log(timeFlight);
		GetComponent<LineRenderer>().positionCount = accuracy;
		Vector3 trajectoryPoint;

		for (int i = 0; i < GetComponent<LineRenderer>().positionCount; i++)
		{
			float time = timeFlight * i / (float)(GetComponent<LineRenderer>().positionCount);
			trajectoryPoint = transform.position + initialVelocity * time + 0.5f * Physics.gravity * time * time;
			GetComponent<LineRenderer>().SetPosition(i, trajectoryPoint);
			
		}
	}

	/// <summary>
	/// Returns vector velocity needed to reach target in time seconds
	/// </summary>
	/// <param name="origin"></param>
	/// <param name="target"></param>
	/// <param name="time"></param>
	/// <returns></returns>
	public Vector3 CalculateVelocity(Vector3 origin, Vector3 target, float time)
	{
		Vector3 distance = target - origin;
		Vector3 distanceXZ = distance;
		distanceXZ.y = 0f;

		float Dy = distance.y;
		float Dxz = distanceXZ.magnitude;

		float Vy = Dy / time + 0.5f * (-Physics.gravity.y) * time;
		float Vxz = Dxz / time;

		Vector3 result = distanceXZ.normalized;
		result *= Vxz;
		result.y = Vy;

		return result;
	}

	public void DisplayTrajectory(bool active)
	{
		GetComponent<LineRenderer>().enabled = active;
	}

	public void SetInitialVelocity(Vector3 velocity)
	{
		initialVelocity = velocity;
	}
}
