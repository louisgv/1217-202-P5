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
public class FlockerSystem : SpawningSystem <Flocker>
{
	[SerializeField]
	private ObstacleSystem targetObstacleSystem;

	[SerializeField]
	private Transform seekingTarget;

	[SerializeField]
	private AverageMarker averagePositionMarker;

	private Vector3 averagePosition;

	private Vector3 averageVelocity;

	private int localFlockGroupCount;

	/// <summary>
	/// Gets the instance map.
	/// </summary>
	/// <value>The instance map.</value>
	public Dictionary <SpawningGridCoordinate, Vector3> FlockAverageVelocityMap {
		get;
		private set;
	}

	/// <summary>
	/// Gets the flock average position map.
	/// </summary>
	/// <value>The flock average position map.</value>
	public Dictionary <SpawningGridCoordinate, Vector3> FlockAveragePositionMap {
		get;
		private set;
	}

	/// <summary>
	/// Batch spawn entity.
	/// </summary>
	/// <param name="count">Count.</param>
	public void SpawnEntities (int count)
	{
		for (int i = 0; i < count; i++) {
			SpawnEntity ();
		}
	}

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
	/// Refreshs the flocker average position map.
	/// </summary>
	private void RefreshFlockAverageDataMap ()
	{
		averagePosition = Vector3.zero;
		localFlockGroupCount = 0;

		foreach (var item in InstanceMap) {
			var instanceSet = item.Value;

			if (instanceSet == null || instanceSet.Count == 0) {
				FlockAverageVelocityMap [item.Key] = Vector3.zero;
				FlockAveragePositionMap [item.Key] = Vector3.zero;
				continue;
			}

			float maxSpeed = 0;
	
			var positionSum = Vector3.zero;
			var velocitySum = Vector3.zero;


			foreach (var instance in instanceSet) {
				if (maxSpeed < instance.MaxSteeringSpeed) {
					maxSpeed = instance.MaxSteeringSpeed;
				}

				positionSum += instance.transform.position;
				velocitySum += instance.Velocity;
			}

			var avgVelocity = velocitySum / (float)instanceSet.Count;

			FlockAverageVelocityMap [item.Key] = avgVelocity.normalized * maxSpeed;
			FlockAveragePositionMap [item.Key] = positionSum / (float)instanceSet.Count;

			averageVelocity += FlockAverageVelocityMap [item.Key];
			averagePosition += FlockAveragePositionMap [item.Key];
			localFlockGroupCount += 1;
		}

		averagePosition /= (float)localFlockGroupCount;
		averageVelocity /= (float)localFlockGroupCount;

		averagePositionMarker.SetVelocity (averageVelocity);

		averagePositionMarker.MarkerPosition = averagePosition;
	}

	protected override void Awake ()
	{
		base.Awake ();
		FlockAverageVelocityMap = new Dictionary<SpawningGridCoordinate, Vector3> ();
		FlockAveragePositionMap = new Dictionary<SpawningGridCoordinate, Vector3> ();
	}

	protected override void Update ()
	{
		base.Update ();
		RefreshFlockAverageDataMap ();
	}
}
