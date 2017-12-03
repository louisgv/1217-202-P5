using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Path follower.
/// Following the path defined by its parent system
/// Author: LAB
/// Attached to: PathFollower
/// </summary>
public class PathFollower : PathFollowingVehicle<PathFollower, PathFollowerSystem>
{

	#region implemented abstract members of Vehicle

	protected override Vector3 GetTotalSteeringForce ()
	{
		totalForce = Vector3.zero;

		totalForce += GetPathFollowForce () * pathFollowParams.ForceScale;

		// Addng a dash of wandering force so that if it get stuck, it will move away
		totalForce += SteeringForce.GetWanderingForce (this) * wanderingParams.ForceScale;

		totalForce += GetTotalNeighborSeparationForce () * separationParams.ForceScale;

		totalForce += SteeringForce.GetBoundingForce (this, BoundingPlane) * boundingParams.ForceScale;

		totalForce.y = 0;

		return Vector3.ClampMagnitude (totalForce, maxForce);
	}

	#endregion

}
