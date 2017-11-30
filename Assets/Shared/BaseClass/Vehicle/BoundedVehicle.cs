using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BoundedVehicle bounce itself using a bounding force upon overstepping 
/// its assinged terrain area allowed to move within.
/// Author LAB
/// Attached to: N/A
/// </summary>
public abstract class BoundedVehicle : Vehicle
{
	[SerializeField]
	public SteeringParams boundingParams;

	/// <summary>
	/// Gets the bounding force.
	/// </summary>
	/// <returns>The bounding force.</returns>
	protected Vector3 GetBoundingForce ()
	{
		return SteeringForce.GetBoundingForce (this, BoundingPlane);
	}
}