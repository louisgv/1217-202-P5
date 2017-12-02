using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Flow field follower.
/// Follow the flow field defined by its parent system
/// Author: LAB
/// Attached to: FlowFieldFollower
/// </summary>
public class FlowFieldFollower : FieldFlowingVehicle<FlowFieldFollower, FlowFieldFollowerSystem>
{
	#region implemented abstract members of Vehicle

	/// <summary>
	/// Gets the steering force.
	/// </summary>
	/// <returns>The total steering force.</returns>
	protected override Vector3 GetTotalSteeringForce ()
	{
		totalForce = Vector3.zero;

		totalForce += GetTotalNeighborSeparationForce () * separationParams.ForceScale;

		totalForce += GetFutureFieldForce () * flowFieldParams.ForceScale;

		totalForce += SteeringForce.GetBoundingForce (this, BoundingPlane) * boundingParams.ForceScale;

		totalForce.y = 0;

		return Vector3.ClampMagnitude (totalForce, maxForce);
	}

	#endregion


}
