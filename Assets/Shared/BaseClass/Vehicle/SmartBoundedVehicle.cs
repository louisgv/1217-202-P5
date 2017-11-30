using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Smart bounded vehicle.
/// It is a bounded vehicle that avoids obstacles
/// It is also smart enought to know that it is in a grid
/// and it is awared of the plane it can walk on
/// It is also smart because it know to pursue or evade instead
/// of normal seek/flee
/// It also explode upon destroyed
/// Author: LAB
/// </summary>
[RequireComponent (typeof(C))]
abstract public class SmartBoundedVehicle <V, C, S, O>: BoundedVehicle 
	where V: Vehicle // it's acyclic so it's fine
	where C : CustomBoxCollider
	where S : SpawningSystem<V>  // Wow, dangerous teritory here
	where O : ObstacleSystem
{
	// This param includes the safe distance
	[SerializeField]
	public SteeringParams avoidingParams;

	[SerializeField]
	private GameObject explosionPrefab;

	/// <summary>
	/// Gets or sets the parent system.
	/// </summary>
	/// <value>The parent system.</value>
	public S ParentSystem { get; set; }

	/// <summary>
	/// Gets or sets the target obstacle system.
	/// </summary>
	/// <value>The target obstacle system.</value>
	public O TargetObstacleSystem { get; set; }

	/// <summary>
	/// Gets or sets the collider instance.
	/// </summary>
	/// <value>The collider instance.</value>
	public C ColliderInstance { get ; set ; }

	private float maxPredictionTimeSquared = 9f;

	#region Unity Lifecycle

	protected override void Awake ()
	{
		ColliderInstance = GetComponent <C> ();
	}

	#endregion

	/// <summary>
	/// Explode this instance.
	/// </summary>
	public void Explode ()
	{
		var explosionInstance = Instantiate (explosionPrefab, transform.position, Quaternion.identity);
		Destroy (explosionInstance, 1.8f);
	}

	protected override void Reset ()
	{
		base.Reset ();

		ParentSystem.RenewVehicle (this as V);
	}

	/// <summary>
	/// Gets the total obstacle avoidance force.
	/// </summary>
	/// <returns>The total obstacle avoidance force.</returns>
	protected Vector3 GetTotalObstacleAvoidanceForce ()
	{
		var mostThreateningObstacle = TargetObstacleSystem.FindNearestInstance (this, avoidingParams.ThresholdSquared, 2);

		if (mostThreateningObstacle == null) {
			return Vector3.zero;
		}

		var obstacleCollider = mostThreateningObstacle.GetComponent <CustomBoxCollider> ();

		float radiDistance = ColliderInstance.GetAverageXZLength () + obstacleCollider.GetAverageXZLength ();

		float halfFutureDistance = Velocity.sqrMagnitude / MaxSpeedSquared / 2;

		var halfFutureDiff = transform.forward * halfFutureDistance;

		// Front radar ahead of the vehicle
		var frontRadar = mostThreateningObstacle.position - transform.position;

		var halfFutureRadar = frontRadar - halfFutureDiff;

		var futureRadar = frontRadar - halfFutureDiff;

		return SteeringForce.GetAvoidanceForce (this, frontRadar, radiDistance) +
		SteeringForce.GetAvoidanceForce (this, futureRadar, radiDistance) +
		SteeringForce.GetAvoidanceForce (this, halfFutureRadar, radiDistance);
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