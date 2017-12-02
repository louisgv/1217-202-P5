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
	#region implemented abstract members of SpawningSystem

	public override void SpawnEntity (Vector3 pos)
	{
		throw new System.NotImplementedException ();
	}

	#endregion

}
