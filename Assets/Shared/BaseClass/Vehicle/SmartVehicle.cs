using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Smart  vehicle.
/// It is smart because it know to pursue or evade instead
/// of normal seek/flee
/// It also explode upon destroyed
/// Author: LAB
/// </summary>
abstract public class SmartVehicle <V, S> : AgileVehicle <V, S>
	where V : Vehicle
	where S : SpawningSystem <V>
{

	[SerializeField]
	private GameObject explosionPrefab;

	private float maxPredictionTimeSquared = 9f;

	/// <summary>
	/// Explode this instance.
	/// </summary>
	public void Explode ()
	{
		var explosionInstance = Instantiate (explosionPrefab, transform.position, Quaternion.identity);
		Destroy (explosionInstance, 1.8f);
	}

	/// <summary>
	/// Gets the target future position.
	/// </summary>
	/// <returns>The target future position.</returns>
	/// <param name="target">Target.</param>
	protected Vector3 GetTargetFuturePosition (Vehicle target)
	{
		var diff = target.transform.position - transform.position;

		var distanceSquared = diff.sqrMagnitude;
		// this will cramp naturally as the diff get smaller 
		float predictionTimeSquared = Mathf.Min (
			                              distanceSquared / MaxSteeringSpeedSquared, 
			                              maxPredictionTimeSquared);

		return target.transform.position + target.Velocity * Mathf.Sqrt (predictionTimeSquared);
	}

	/// <summary>
	/// Gets the pursuing force.
	/// </summary>
	/// <returns>The pursuing force.</returns>
	protected Vector3 GetPursuingForce (Vehicle target)
	{
		var finalTarget = GetTargetFuturePosition (target);

		target.FuturePosition = finalTarget;

		return SteeringForce.GetSeekingForce (this, finalTarget);
	}

	/// <summary>
	/// Gets the evading force.
	/// </summary>
	/// <returns>The evading force.</returns>
	/// <param name="target">Target.</param>
	protected Vector3 GetEvadingForce (Vehicle target)
	{
		var finalTarget = GetTargetFuturePosition (target);

		target.FuturePosition = finalTarget;

		return SteeringForce.GetFleeingForce (this, finalTarget);
	}
}