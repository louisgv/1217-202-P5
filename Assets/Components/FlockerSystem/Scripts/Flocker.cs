using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Flocker.
/// A flocker's movement is heavily influenced by 
/// the Flocker System it belongs to
/// Author: LAB
/// Attached to: Flocker
/// </summary>
public class Flocker : FlockingVehicle <Flocker, FlockerCollider, FlockerSystem, ObstacleSystem>
{

	/// <summary>
	/// Gets or sets the seeking target.
	/// </summary>
	/// <value>The seeking target.</value>
	public Transform SeekingTarget { get; set; }

	#region implemented abstract members of Vehicle

	/// <summary>
	/// Gets the steering force.
	/// </summary>
	/// <returns>The total steering force.</returns>
	protected override Vector3 GetTotalSteeringForce ()
	{
		totalForce = Vector3.zero;

		totalForce += SteeringForce.GetSeekingForce (this, SeekingTarget.position) * seekingParams.ForceScale;

		totalForce += GetTotalObstacleAvoidanceForce () * avoidingParams.ForceScale;

		totalForce += GetTotalNeighborSeparationForce () * separationParams.ForceScale;

		totalForce += GetTotalNeighborAlignmentForce (ParentSystem.FlockAverageVelocityMap [GridCoordinate]) * alignParams.ForceScale;

		totalForce += GetTotalNeighborCohesionForce (ParentSystem.FlockAveragePositionMap [GridCoordinate]) * cohesionParams.ForceScale;

		totalForce += GetBoundingForce () * boundingParams.ForceScale;

//		totalForce.y = 0;

		return Vector3.ClampMagnitude (totalForce, maxForce);
	}

	#endregion

}
