using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Average marker.
/// Handle drawing debug line for the average marker
/// As well as rotate it toward the velocity
/// Author: LAB
/// Attached to: AverageMarker
/// </summary>
public class AverageMarker : Vehicle
{
	/// <summary>
	/// Gets or sets the marker position.
	/// </summary>
	/// <value>The marker position.</value>
	public Vector3 MarkerPosition { get; set; }

	#region implemented abstract members of Vehicle

	/// <summary>
	/// Return 0 as this will be set by the average fleet system
	/// </summary>
	/// <returns>The total steering force.</returns>
	protected override Vector3 GetTotalSteeringForce ()
	{
		return Vector3.zero;
	}

	#endregion

	protected override void LateUpdate ()
	{
		base.LateUpdate ();
	
		TeleportToMarker (MarkerPosition);
	}

	/// <summary>
	/// Sets the velocity.
	/// </summary>
	/// <param name="value">Value.</param>
	public void SetVelocity (Vector3 value)
	{
		Velocity = value;
	}

	/// <summary>
	/// Teleport if threshold is surpassed
	/// </summary>
	/// <param name="newPos">New position.</param>
	public void TeleportToMarker (Vector3 newPos)
	{
		transform.position = newPos;
	}
}
