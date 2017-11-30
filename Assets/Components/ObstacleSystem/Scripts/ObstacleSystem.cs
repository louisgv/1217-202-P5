using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Obstacle system.
/// Spawns obstacles on the asigned plane
/// Author: LAB
/// Attached to: ObstacleSystem
/// </summary>
public class ObstacleSystem : SpawningSystem <Obstacle>
{
	#region implemented abstract members of SpawningSystem

	/// <summary>
	/// Spawns an entity at the specified position.
	/// </summary>
	/// <param name="pos">Position.</param>
	public override void SpawnEntity (Vector3 pos)
	{
		var instance = Instantiate (prefab, pos, Quaternion.identity, transform);

		instance.BoundingPlane = plane;

		RegisterVehicle (instance);
	}

	/// <summary>
	/// Spawns the obstacles.
	/// </summary>
	protected override void SpawnEntities ()
	{
		var minBound = plane.GetMinBound ();

		int xCount = initialSpawnCount / 2;

		int zCount = initialSpawnCount - xCount;

		float xStep = plane.Size.x / xCount;
		float zStep = plane.Size.z / zCount;

		var initialPos = new Vector3 (xStep, 0, zStep) / 2f;

		for (int x = 0; x < xCount; x++) {
			for (int z = 0; z < zCount; z++) {
				var spawnPos = minBound + new Vector3 (x * xStep, 0, z * zStep) + initialPos;
				
				SpawnEntity (spawnPos);
			}
		}
	}

	#endregion
	
}
