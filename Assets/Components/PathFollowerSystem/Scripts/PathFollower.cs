using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Path follower.
/// </summary>
public class PathFollower : PathFollowingVehicle<PathFollower, PathFollowerSystem>
{

	#region implemented abstract members of Vehicle

	protected override Vector3 GetTotalSteeringForce ()
	{
		totalForce = Vector3.zero;

		totalForce += GetPathFollowForce () * pathFollowParams.ForceScale;

		totalForce += GetTotalNeighborSeparationForce () * separationParams.ForceScale;

		totalForce += SteeringForce.GetBoundingForce (this, BoundingPlane) * boundingParams.ForceScale;

		totalForce.y = 0;

		return Vector3.ClampMagnitude (totalForce, maxForce);
	}

	#endregion

}
