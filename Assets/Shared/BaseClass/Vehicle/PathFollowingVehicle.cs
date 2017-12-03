using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Path following vehicle.
/// A flocking vehicle that implement method to Follow the Path
/// defined by its parent system
/// Author: LAB
/// </summary>
abstract public class PathFollowingVehicle <V, S>: FlockingVehicle<V, S> 
	where V : Vehicle
	where S : VehicleSpawningSystem<V>
{

	// This param includes the safe distance
	[SerializeField]
	public SteeringParams pathFollowParams;

	/// <summary>
	/// Local reference to the node list assigned by the parent system
	/// </summary>
	/// <value>The nodes.</value>
	public List<PathNode> Nodes {
		private get;
		set;
	}

	/// <summary>
	/// Local reference to the path radius
	/// </summary>
	/// <value>The path radius squared.</value>
	public float PathRadiusSquared {
		private get;
		set;
	}

	/// <summary>
	/// Cycle through the available camera in the camera array
	/// </summary>
	/// <returns>The next camera index</returns>
	protected int GetNextNodeIndexCyclic (int currentNodeIndex)
	{
		// Cycle index using mod op
		return (currentNodeIndex + 1) % Nodes.Count;
	}

	/// <summary>
	/// Cycle through the available camera in the camera array
	/// </summary>
	/// <returns>The next camera index</returns>
	protected int GetPreviousNodeIndexCyclic (int currentNodeIndex)
	{
		// Cycle index using mod op
		return (currentNodeIndex + Nodes.Count - 1) % Nodes.Count;
	}


	/// <summary>
	/// Finds the normal point on the path.
	/// </summary></summary>
	/// <returns>The normal point.</returns>
	/// <param name="futurePos">Future position.</param>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	protected Vector3 FindNormalPoint (Vector3 futurePos, Vector3 start, Vector3 end)
	{
		var startToPos = futurePos - start;

		var path = end - start;

		var pathDir = path.normalized;

		// Project the future position on the path
		return start + pathDir * Vector3.Dot (startToPos, pathDir);
	}

	/// <summary>
	/// Gets the path follow force.
	/// </summary>
	/// <returns>The path follow force.</returns>
	protected Vector3 GetPathFollowForce ()
	{
		var futurePos = GetFuturePosition ();

		Vector3 target = Vector3.zero;

		Vector3 normalPoint = Vector3.zero;

		float maxDistanceSquared = float.MaxValue;

		// Find normal point

		for (int i = 0; i < Nodes.Count; i++) {
			var a = Nodes [i];
			var b = Nodes [GetNextNodeIndexCyclic (i)];

			normalPoint = FindNormalPoint (futurePos, 
				a.transform.position, 
				b.transform.position);

			var dir = b - a;

			if (normalPoint.x < PathNode.MinX (a, b) ||
			    normalPoint.x > PathNode.MaxX (a, b) ||
			    normalPoint.y < PathNode.MinY (a, b) ||
			    normalPoint.y > PathNode.MaxY (a, b)) {

				normalPoint = b.transform.position;

				a = b;
				b = Nodes [GetNextNodeIndexCyclic (i + 1)];

				dir = b - a;
			}


			var distanceSquared = (futurePos - normalPoint).sqrMagnitude;

			if (distanceSquared < maxDistanceSquared) {
				maxDistanceSquared = distanceSquared;

				target = normalPoint + dir.normalized * pathFollowParams.ThresholdSquared;
//					* Velocity.sqrMagnitude / distanceSquared;
			}
		}

		if (maxDistanceSquared < PathRadiusSquared) {
			return Vector3.zero;
		}

		return SteeringForce.GetSeekingForce (this, target);
	}

}
