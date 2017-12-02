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
	public Dictionary <Vector3, FlowFieldGrid> FlowFieldMap {
		get;
		private set;
	}

	public FlowFieldGrid flowGridPrefab;

	#region implemented abstract members of SpawningSystem

	/// <summary>
	/// Spawns an entity at the specified position.
	/// </summary>
	/// <param name="pos">Position.</param>
	public override void SpawnEntity (Vector3 pos)
	{
		var flowFieldFollowerInstance = Instantiate (prefab, pos, Quaternion.identity, transform);

		flowFieldFollowerInstance.ParentSystem = this;

		flowFieldFollowerInstance.BoundingPlane = plane;

		flowFieldFollowerInstance.FlowFieldMap = FlowFieldMap;

		RegisterVehicle (flowFieldFollowerInstance);
	}

	#endregion


	#region Unity Lifecycle

	protected override void Awake ()
	{
		base.Awake ();

		FlowFieldMap = new Dictionary<Vector3, FlowFieldGrid> ();

		InitializeFlowFieldMap ();
	}

	protected override void Start ()
	{
		base.Start ();
	}

	#endregion

	/// <summary>
	/// Initializes the flow field map.
	/// </summary>
	private void InitializeFlowFieldMap ()
	{
		var minBound = plane.GetMinBound ();

		var maxBound = plane.GetMaxBound ();

		for (int x = (int)minBound.x; x <= (int)maxBound.x; x++) {
			for (int z = (int)minBound.z; z <= (int)maxBound.z; z++) {
				var gridPos = plane.WorldCenter + new Vector3 (x, 0, z);

				var grid = Instantiate (flowGridPrefab, gridPos, Quaternion.identity, transform);

				grid.FlowDirection = new Vector3 (
					Mathf.Cos (gridPos.magnitude) * gridPos.z, 0, 
					gridPos.x).normalized;

				FlowFieldMap.Add (gridPos, grid);
			}
		}
	}

}
