using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Flocker system.
/// Used to spawn flocker and update their direction as well as managing their 
/// flock behavior
/// Author: LAB
/// Attached to: FlockerSystem
/// </summary>
public class FlockerSystem : VehicleSpawningSystem <Flocker>
{
	[SerializeField]
	private ObstacleSystem targetObstacleSystem;

	[SerializeField]
	private Transform seekingTarget;

	[SerializeField]
	private Transform averagePositionMarker;

	#region implemented abstract members of SpawningSystem

	/// <summary>
	/// Spawns an entity at the specified position.
	/// </summary>
	/// <param name="pos">Position.</param>
	public override void SpawnEntity (Vector3 pos)
	{
		var flockerInstance = Instantiate (prefab, pos, Quaternion.identity, transform);

		flockerInstance.ParentSystem = this;

		flockerInstance.TargetObstacleSystem = targetObstacleSystem;

		flockerInstance.BoundingPlane = plane;

		flockerInstance.SeekingTarget = seekingTarget;

		RegisterVehicle (flockerInstance);
	}

	#endregion

	/// <summary>
	/// Update marker's location and velocity
	/// </summary>
	override protected void OnAverageDataRefreshed ()
	{
		averagePositionMarker.position = averagePosition;

		averagePositionMarker.forward = averageVelocity.normalized;
	}

	protected override void Update ()
	{
		base.Update ();
		RefreshFlockAverageDataMap ();
	}
}
