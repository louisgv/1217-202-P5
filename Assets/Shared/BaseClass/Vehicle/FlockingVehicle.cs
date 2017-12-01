using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Flocking vehicle.
/// It is a smart vehicle that also know how to move together with
/// its flock mate in the same spawning system
/// Author: LAB
/// </summary>
abstract public class FlockingVehicle  <V, C, S, O>: SmartVehicle <V,C,S,O> 
	where V : Vehicle // it's acyclic so it's fine
	where C : CustomBoxCollider
	where S : SpawningSystem<V>  // Wow, dangerous teritory here
	where O : ObstacleSystem
{
	// This param includes the safe distance
	[SerializeField]
	public SteeringParams alignParams;

	[SerializeField]
	public SteeringParams cohesionParams;

	[SerializeField]
	public SteeringParams separationParams;

	/// <summary>
	/// Gets the total neighbor separation force.
	/// </summary>
	/// <returns>The total neighbor separation force.</returns>
	protected Vector3 GetTotalNeighborSeparationForce ()
	{
		var nearbyNeighbors = ParentSystem.FindCloseProximityInstances (this, ColliderInstance.GetAverageXZLength ());

		if (nearbyNeighbors.Count == 0) {
			return Vector3.zero;
		}

		return SteeringForce.GetNeighborSeparationForce (this, nearbyNeighbors);
	}

	/// <summary>
	/// Gets the total neighbor alignment force.
	/// </summary>
	/// <returns>The total neighbor alignment force.</returns>
	protected Vector3 GetTotalNeighborAlignmentForce (Vector3 flockAverageVelocity)
	{
		if (flockAverageVelocity == Vector3.zero) {
			return Vector3.zero;
		}

		return SteeringForce.GetSteeringForce (this, flockAverageVelocity);
	}

	/// <summary>
	/// Gets the total neighbor cohesion force.
	/// </summary>
	/// <returns>The total neighbor cohesion force.</returns>
	protected Vector3 GetTotalNeighborCohesionForce (Vector3 flockAveragePosition)
	{
		if (flockAveragePosition == Vector3.zero) {
			return Vector3.zero;
		}

		return SteeringForce.GetSeekingForce (this, flockAveragePosition);
	}
}
