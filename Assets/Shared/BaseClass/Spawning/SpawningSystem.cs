using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;
using System.ComponentModel;

/// <summary>
/// Spawning system.
/// Author: LAB
/// Attached to: N/A
/// </summary>
public abstract class SpawningSystem <T>: MonoBehaviour 
	where T : SpawningGridComponent // Ack... Seems like T cannot be just plain component...
{
	public int initialSpawnCount = 9;

	public KeyCode spawnKey;

	public T prefab;

	protected CustomBoxCollider prefabCollider;

	[SerializeField]
	protected CustomBoxCollider plane;

	// A x A grid
	protected int gridResolution = 9;

	// size is max plane size / res
	protected float gridSize;

	/// <summary>
	/// Gets the grid resolution.
	/// </summary>
	/// <value>The grid resolution.</value>
	public int GridResolution {
		get {
			return gridResolution;
		}
	}

	/// <summary>
	/// Gets the size of the grid.
	/// </summary>
	/// <value>The size of the grid.</value>
	public float GridSize {
		get {
			return gridSize;
		}
	}

	/// <summary>
	/// Gets the instance map.
	/// </summary>
	/// <value>The instance map.</value>
	protected Dictionary <SpawningGridCoordinate, HashSet<T>> InstanceMap {
		get;
		private set;
	}

	/// <summary>
	/// Gets the collider instance map.
	/// </summary>
	/// <value>The collider instance map.</value>
	protected Dictionary <SpawningGridCoordinate, HashSet<CustomBoxCollider>> ColliderInstanceMap {
		get;
		private set;
	}

	/// <summary>
	/// Spawns an entity.
	/// </summary>
	protected virtual void SpawnEntity ()
	{
		var spawnPos = plane.GetRandomPositionAbove (prefabCollider);

		SpawnEntity (spawnPos);
	}

	/// <summary>
	/// Spawns the entity above plane.
	/// </summary>
	/// <param name="pos">Position.</param>
	public virtual void SpawnEntityAbovePlane (Vector3 pos)
	{
		var spawnPos = plane.GetSampledPosition (pos, prefabCollider);

		SpawnEntity (spawnPos);
	}

	/// <summary>
	/// Spawns an entity at the specified position.
	/// </summary>
	/// <param name="pos">Position.</param>
	public abstract void SpawnEntity (Vector3 pos);

	/// <summary>
	/// Spawns the entities.
	/// </summary>
	protected virtual void SpawnEntities ()
	{
		for (int i = 0; i < initialSpawnCount; i++) {
			SpawnEntity ();
		}
	}

	protected virtual void Awake ()
	{
		InstanceMap = new Dictionary<SpawningGridCoordinate, HashSet<T>> ();

		ColliderInstanceMap = new Dictionary<SpawningGridCoordinate, HashSet<CustomBoxCollider>> ();

		prefabCollider = prefab.GetComponent <CustomBoxCollider> ();

		var planeSize = plane.Size;

		gridSize = Mathf.Max (planeSize.x, planeSize.z) / gridResolution;

		SpawnEntities ();
	}


	/// <summary>
	/// Add the specified instance.
	/// </summary>
	/// <param name="instance">Instance.</param>
	public void RegisterVehicle (T instance)
	{
		var localGridPos = instance.transform.position - plane.WorldCenter;

		var gridCoord = new SpawningGridCoordinate (localGridPos, gridSize, gridResolution);

		RegisterVehicle (gridCoord, instance);
	}

	/// <summary>
	/// Registers the vehicle.
	/// </summary>
	/// <param name="gridKey">Grid key.</param>
	/// <param name="instance">Instance.</param>
	public void RegisterVehicle (SpawningGridCoordinate gridKey, T instance)
	{
		if (!InstanceMap.ContainsKey (gridKey) ||
		    !ColliderInstanceMap.ContainsKey (gridKey)) {
			InstanceMap.Add (gridKey, new HashSet<T> ());
			ColliderInstanceMap.Add (gridKey, new HashSet<CustomBoxCollider> ());
		}

		instance.GridCoordinate = gridKey;

		InstanceMap [gridKey].Add (instance);

		ColliderInstanceMap [gridKey].Add (instance.GetComponent <CustomBoxCollider> ());
	}

	/// <summary>
	/// Removes the vehicle.
	/// </summary>
	/// <param name="gridKey">Grid key.</param>
	/// <param name="instance">Instance.</param>
	public void RemoveVehicle (T instance)
	{
		var gridKey = instance.GridCoordinate;
		
		InstanceMap [gridKey].Remove (instance);

		ColliderInstanceMap [gridKey].Remove (instance.GetComponent <CustomBoxCollider> ());
	}

	/// <summary>
	/// Renews the vehicle's grid position.
	/// </summary>
	/// <param name="instance">Instance.</param>
	public void RenewVehicle (T instance)
	{
		var newGrid = instance.UpdatedGrid ();

		if (newGrid == null) {
			return;
		}

		RemoveVehicle (instance);

		RegisterVehicle (newGrid, instance);
	}

	#region Unity Lifecycle

	protected virtual void Start ()
	{

	}

	protected virtual void Update ()
	{
		if (spawnKey != KeyCode.None && Input.GetKeyDown (spawnKey)) {
			SpawnEntity ();
		}
	}

	#endregion

	public List<T> FindCloseProximityInstances (
		SpawningGridComponent inst,
		int maxLevel
	)
	{
		if (InstanceMap == null || InstanceMap.Count == 0) {
			return null;
		}

		// TODO: Add a delegate parameter here so we can define and reuse the same for loop instead
		var targets = new List<T> ();

		for (int level = 0; level <= maxLevel; level++) {
			var adjacentCoords = inst.GridCoordinate.GetAdjacentGrids (level);

			if (adjacentCoords == null) {
				continue;
			}

			foreach (var coord in adjacentCoords) {
				if (!InstanceMap.ContainsKey (coord)) {
					continue;
				}

				var instanceSet = InstanceMap [coord];

				foreach (var instance in instanceSet) {
					// Skip itself
					if (GameObject.ReferenceEquals (instance.gameObject, inst.gameObject)) {
						continue;
					}

					targets.Add (instance);
				}
			}
		}

		return targets;
	}

	/// <summary>
	/// Finds a list of Transform surrounding a certain position.
	/// </summary>
	/// <returns>The nearest instance.</returns>
	/// <param name="pos">Position.</param>
	public List<Transform> FindCloseProximityInstances (
		SpawningGridComponent inst,
		float minDistanceSquared
	)
	{
		if (InstanceMap == null || InstanceMap.Count == 0) {
			return null;
		}

		// TODO: Add a delegate parameter here so we can define and reuse the same for loop instead
		var targets = new List<Transform> ();

		bool thresholdSurpassed = false;

		for (int level = 0; level <= inst.GridCoordinate.MaxTracingLevel; level++) {
			var adjacentCoords = inst.GridCoordinate.GetAdjacentGrids (level);

			if (adjacentCoords == null) {
				continue;
			}

			foreach (var coord in adjacentCoords) {
				if (!InstanceMap.ContainsKey (coord)) {
					continue;
				}

				var instanceSet = InstanceMap [coord];

				foreach (var instance in instanceSet) {
					// Skip itself
					if (GameObject.ReferenceEquals (instance.gameObject, inst.gameObject)) {
						continue;
					}

					var diffVector = instance.transform.position - inst.transform.position;

					diffVector.y = 0;
					
					float distanceSquared = diffVector.sqrMagnitude;

					if (minDistanceSquared > distanceSquared) {
						targets.Add (instance.transform);
					} else {
						thresholdSurpassed = true;
					}
				}
			}
			// If we found a potential target within an inner level,
			// or none of the element closeby are within the dangerous zone
			// then we don't have to check the outer level
			if (targets.Count > 0 || thresholdSurpassed) {
				break;
			}
		}
		return targets;
	}

	/// <summary>
	/// Finds the nearest instance to given pos.
	/// </summary>
	/// <returns>The nearest instance.</returns>
	/// <param name="pos">Position.</param>
	public Transform FindNearestInstance (
		SpawningGridComponent inst, 
		float minDistanceSquared = float.MaxValue,
		int maxTracingLevel = -1
	)
	{

		if (InstanceMap == null || InstanceMap.Count == 0) {
			return null;
		}

		if (maxTracingLevel == -1) {
			maxTracingLevel = inst.GridCoordinate.MaxTracingLevel;
		}

		// Default to null
		Transform target = null;

		for (int level = 0; level <= maxTracingLevel; level++) {
			var adjacentCoords = inst.GridCoordinate.GetAdjacentGrids (level);

			if (adjacentCoords == null) {
				continue;
			}

			foreach (var coord in adjacentCoords) {
				if (!InstanceMap.ContainsKey (coord)) {
					continue;
				}
				var instanceSet = InstanceMap [coord];

				foreach (var instance in instanceSet) {
					if (instance == null) {
						continue;
					}
					var diffVector = instance.transform.position - inst.transform.position;

					// HACK: 2D position for now
					diffVector.y = 0;

					float distanceSquared = diffVector.sqrMagnitude;

					if (minDistanceSquared > distanceSquared) {
						minDistanceSquared = distanceSquared;

						target = instance.transform;
					}
				}
			}
			// If we found a potential target within an inner level,
			// then we don't have to check the outer level
			if (target != null) {
				break;
			}
		}

		return target;
	}
}

