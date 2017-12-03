using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Agile vehicle.
/// It is aware of the obstacle system it should avoid
/// It is also  aware of the parent system it belongs to
/// and would report to the parent system after each frame
/// Can be optimized to only report after certain amoun of time
/// Author: LAB
/// </summary>
[RequireComponent (typeof(CustomBoxCollider))]
abstract public class AgileVehicle <V, S>: Vehicle
	where V : Vehicle
	where S : SpawningSystem <V>
{
	/// <summary>
	/// Gets or sets the collider instance.
	/// </summary>
	/// <value>The collider instance.</value>
	public CustomBoxCollider ColliderInstance { get ; set ; }

	/// <summary>
	/// Gets or sets the parent system.
	/// </summary>
	/// <value>The parent system.</value>
	public S ParentSystem { get; set; }

	[SerializeField]
	private ObstacleSystem targetObstacleSystem;

	// This param includes the safe distance
	[SerializeField]
	public SteeringParams avoidingParams;

	public float futureLookupTime = 1.8f;

	/// <summary>
	/// Gets the future position.
	/// </summary>
	/// <returns>The future position.</returns>
	protected Vector3 GetFuturePosition ()
	{
		return transform.position + Velocity * futureLookupTime;
	}


	/// <summary>
	/// Gets or sets the target obstacle system.
	/// </summary>
	/// <value>The target obstacle system.</value>
	public ObstacleSystem TargetObstacleSystem {
		get {
			return targetObstacleSystem;
		}
		set {
			targetObstacleSystem = value;
		}
	}

	#region Unity Lifecycle

	protected override void Awake ()
	{
		ColliderInstance = GetComponent <CustomBoxCollider> ();
	}

	protected override void Reset ()
	{
		base.Reset ();
		if (ParentSystem) {
			ParentSystem.RenewVehicle (this as V);
		}
	}

	#endregion

	/// <summary>
	/// Gets the total obstacle avoidance force.
	/// </summary>
	/// <returns>The total obstacle avoidance force.</returns>
	protected Vector3 GetTotalObstacleAvoidanceForce ()
	{
		if (TargetObstacleSystem == null) {
			return Vector3.zero;
		}
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
