using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vehicle Spawning system.
/// This spawning system can keep track of the average velocity and position
/// of the vehicle it controlled
/// Author: LAB
/// Attached to: N/A
/// </summary>
abstract public class VehicleSpawningSystem <V>: SpawningSystem <V>
	where V: Vehicle
{

	protected Vector3 averagePosition;

	protected Vector3 averageVelocity;

	private int localFlockGroupCount;

	/// <summary>
	/// Gets the flock average velocity map.
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

	#region Unity Lifecycle

	protected override void Awake ()
	{
		base.Awake ();

		FlockAverageVelocityMap = new Dictionary<SpawningGridCoordinate, Vector3> ();

		FlockAveragePositionMap = new Dictionary<SpawningGridCoordinate, Vector3> ();
	}

	#endregion

	/// <summary>
	/// Callback method when the average map is refreshed.
	/// </summary>
	protected virtual void OnAverageDataRefreshed ()
	{
	}

	/// <summary>
	/// Refreshs the flocker average position map.
	/// </summary>
	protected void RefreshFlockAverageDataMap ()
	{
		averagePosition = Vector3.zero;
		averageVelocity = Vector3.zero;

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

		if (localFlockGroupCount > 0) {
			averagePosition /= (float)localFlockGroupCount;
			averageVelocity /= (float)localFlockGroupCount;
		} else {
			averagePosition = Vector3.zero;
			averageVelocity = Vector3.forward;
		}

		OnAverageDataRefreshed ();
	}

}
