using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Field flowing vehicle.
/// A Smart vehicle that implement method to Follow the flow field 
/// defined by its parent system
/// Author: LAB
/// </summary>
abstract public class FieldFlowingVehicle <V, S>: FlockingVehicle<V, S> 
	where V : Vehicle
	where S : VehicleSpawningSystem<V>
{
	// This param includes the safe distance
	[SerializeField]
	public SteeringParams flowFieldParams;

	public float futureLookupTime = 1.8f;

	/// <summary>
	/// Local reference to the flowfield map
	/// </summary>
	/// <value>The flow field map.</value>
	public Dictionary <Vector3, FlowFieldGrid> FlowFieldMap {
		private get;
		set;
	}

	/// <summary>
	/// Gets the future position.
	/// </summary>
	/// <returns>The future position.</returns>
	protected Vector3 GetFuturePosition ()
	{
		return transform.position + Velocity * futureLookupTime;
	}

	/// <summary>
	/// Gets the flow grid.
	/// </summary>
	/// <returns>The flow grid.</returns>
	/// <param name="pos">Position.</param>
	private FlowFieldGrid GetFlowGrid (Vector3 pos)
	{
		var gridPos = BoundingPlane.WorldCenter +
		              new Vector3 (Mathf.RoundToInt (pos.x), 0, Mathf.RoundToInt (pos.z));

		if (FlowFieldMap.ContainsKey (gridPos)) {
			return FlowFieldMap [gridPos];
		}

		return null;
	}

	/// <summary>
	/// Gets the field force at specified position.
	/// </summary>
	/// <returns>The field force.</returns>
	protected Vector3 GetFieldForce (Vector3 pos)
	{
		var flowGrid = GetFlowGrid (pos);

		if (flowGrid == null) {
			return Vector3.zero;
		}

		return SteeringForce.GetSteeringForce (this, flowGrid.FlowDirection * maxSteeringSpeed);
	}

	/// <summary>
	/// Gets the future field force.
	/// </summary>
	/// <returns>The future field force.</returns>
	protected Vector3 GetFutureFieldForce ()
	{
		return GetFieldForce (GetFuturePosition ());
	}
}
