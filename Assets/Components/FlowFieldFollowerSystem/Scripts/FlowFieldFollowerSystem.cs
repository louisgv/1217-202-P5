using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Flow field follower.
/// It constructs the vector field, leveraging the grid we already implemented
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

	private GameObject flowGridContainer;

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

	/// <summary>
	/// Spawns an entity.
	/// </summary>
	protected override void SpawnEntity ()
	{
		var spawnPos = plane.GetRandomPositionXZ ();

		SpawnEntity (spawnPos);
	}

	#endregion


	#region Unity Lifecycle

	protected override void Awake ()
	{
		flowGridContainer = new GameObject ("Flow Grid Container");

		flowGridContainer.transform.SetParent (transform);

		FlowFieldMap = new Dictionary<Vector3, FlowFieldGrid> ();

		base.Awake ();

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

				var grid = Instantiate (flowGridPrefab, gridPos, Quaternion.identity, flowGridContainer.transform);

				grid.FlowDirection = new Vector3 (
					Mathf.Cos (gridPos.magnitude / 9f) * gridPos.z, 0, 
					gridPos.x).normalized;

				FlowFieldMap.Add (gridPos, grid);
			}
		}
	}

	protected override void Update ()
	{
		base.Update ();
		RefreshFlockAverageVelocityMap ();
	}
}
