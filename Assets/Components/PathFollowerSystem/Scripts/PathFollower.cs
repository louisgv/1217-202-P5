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
	/// <summary>
	/// Local reference of the resistance area. Unique to this 
	/// group only
	/// </summary>
	/// <value>The resistance areas.</value>
	public List<ResistanceArea> ResistanceAreas {
		get;
		set;
	}

	#region implemented abstract members of Vehicle

	/// <summary>
	/// Gets the steering force.
	/// </summary>
	/// <returns>The total steering force.</returns>
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

	#region Unity Lifecycle

	protected override void Update ()
	{
		ApplyDragForce ();

		base.Update ();
	}


	#endregion

	/// <summary>
	/// Checks if is inside any of the resistance area and apply drag
	/// accordingly.
	/// </summary>
	private void ApplyDragForce ()
	{
		foreach (var resistanceArea in ResistanceAreas) {
			if (resistanceArea.IsCollidingWith (ColliderInstance)) {
				ApplyDragForce (resistanceArea);
			}
		}
	}

	/// <summary>
	/// Applies the drag force.
	/// </summary>
	private void ApplyDragForce (ResistanceArea resistanceArea)
	{
		var dragMagnitude = resistanceArea.dragCoefficient * Velocity.sqrMagnitude;

		ApplyForce (-Velocity.normalized * dragMagnitude);
	}

}
