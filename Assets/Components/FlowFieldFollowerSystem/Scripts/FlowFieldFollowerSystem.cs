using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Flow field follower.
/// It construct the vector field, leveraging the grid we already implemented
/// Author: LAB
/// Attached to: FlowFieldFollowerSystem
/// </summary>
public class FlowFieldFollowerSystem : VehicleSpawningSystem<FlowFieldFollower>
{
	/// <summary>
	/// A dynamic cache of the flow field for the flow field vehicle
	/// </summary>
	/// <value>The instance map.</value>
	public Dictionary <SpawningGridCoordinate, Vector3> FlowFieldMap {
		get;
		private set;
	}

	#region implemented abstract members of SpawningSystem

	/// <summary>
	/// Spawns an entity at the specified position.
	/// </summary>
	/// <param name="pos">Position.</param>
	public override void SpawnEntity (Vector3 pos)
	{
		
	}

	#endregion


}
