using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Path follower system.
/// It holds a path defined by several nodes
/// Each Path Follower then use the information to 
/// move to the next point
/// Author: LAB
/// Attached to: PathFollowerSystem
/// </summary>
public class PathFollowerSystem : VehicleSpawningSystem<PathFollower>
{
	public Transform path;

	public Transform puddles;

	public float pathRadiusSquared;

	private List<PathNode> nodes;

	private List<ResistanceArea> resistanceAreas;


	#region implemented abstract members of SpawningSystem

	/// <summary>
	/// Spawns an entity at the specified position.
	/// </summary>
	/// <param name="pos">Position.</param>
	public override void SpawnEntity (Vector3 pos)
	{
		var pathFollowerInstance = Instantiate (prefab, pos, Quaternion.identity, transform);

		pathFollowerInstance.ParentSystem = this;

		pathFollowerInstance.BoundingPlane = plane;

		pathFollowerInstance.Nodes = nodes;

		pathFollowerInstance.PathRadiusSquared = pathRadiusSquared;

		pathFollowerInstance.ResistanceAreas = resistanceAreas;

		RegisterVehicle (pathFollowerInstance);
	}

	#endregion

	/// <summary>
	/// Initialize the nodes
	/// </summary>
	protected override void Awake ()
	{
		nodes = new List<PathNode> ();

		resistanceAreas = new List<ResistanceArea> ();

		foreach (Transform node in path) {
			nodes.Add (node.GetComponent<PathNode> ());
		}

		foreach (Transform puddle in puddles) {
			resistanceAreas.Add (puddle.GetComponent <ResistanceArea> ());
		}

		base.Awake ();
	}

}
