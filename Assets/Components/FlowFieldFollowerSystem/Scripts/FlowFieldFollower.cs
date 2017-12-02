using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Flow field follower.
/// Follow the flow field defined by its parent system
/// Author: LAB
/// Attached to: FlowFieldFollower
/// </summary>
public class FlowFieldFollower : AgileVehicle<FlowFieldFollower, FlowFieldFollowerSystem>
{
	#region implemented abstract members of Vehicle

	protected override Vector3 GetTotalSteeringForce ()
	{
		totalForce = Vector3.zero;

		totalForce += SteeringForce.GetWanderingForce (this) * wanderingParams.ForceScale;

		totalForce += SteeringForce.GetBoundingForce (this, BoundingPlane) * boundingParams.ForceScale;

		totalForce.y = 0;

		return Vector3.ClampMagnitude (totalForce, maxForce);
	}

	#endregion


}
