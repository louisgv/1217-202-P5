using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Teleporter.
/// Used as a target for the flockers
/// Author: LAB
/// Attached to: Teleporter
/// </summary>
public class Teleporter : AgileVehicle<Teleporter, SpawningSystem<Teleporter>>
{
	#region implemented abstract members of Vehicle

	/// <summary>
	/// Gets the steering force.
	/// </summary>
	/// <returns>The total steering force.</returns>
	protected override Vector3 GetTotalSteeringForce ()
	{
		totalForce = Vector3.zero;

		totalForce += GetTotalObstacleAvoidanceForce () * avoidingParams.ForceScale;

		totalForce += SteeringForce.GetWanderingForce3D (this) * wanderingParams.ForceScale;

		totalForce += SteeringForce.GetBoundingForce3D (this, BoundingPlane) * boundingParams.ForceScale;

		//		totalForce.y = 0;

		return Vector3.ClampMagnitude (totalForce, maxForce);
	}

	#endregion
	
}
