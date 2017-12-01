using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Obstacle avoidance vehicle.
/// It is aware of the obstacle system it should avoid
/// It also explode upon destroyed
/// Author: LAB
/// </summary>
[RequireComponent (typeof(C))]
abstract public class ObstacleAvoidingVehicle <O, C> : Vehicle
	where O : ObstacleSystem
	where C : CustomBoxCollider
{
	/// <summary>
	/// Gets or sets the collider instance.
	/// </summary>
	/// <value>The collider instance.</value>
	public C ColliderInstance { get ; set ; }

	// This param includes the safe distance
	[SerializeField]
	public SteeringParams avoidingParams;

	/// <summary>
	/// Gets or sets the target obstacle system.
	/// </summary>
	/// <value>The target obstacle system.</value>
	public O TargetObstacleSystem { get; set; }

	#region Unity Lifecycle

	protected override void Awake ()
	{
		ColliderInstance = GetComponent <C> ();
	}

	#endregion

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
}
