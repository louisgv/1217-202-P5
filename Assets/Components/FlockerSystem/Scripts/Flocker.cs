using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Flocker.
/// A flocker's movement is heavily influenced by 
/// the Flocker System it belongs to
/// Author: LAB
/// Attached to: Flocker
/// </summary>
public class Flocker : SmartBoundedVehicle<Flocker, FlockerCollider, FlockerSystem, ObstacleSystem>
{
	public Material glLineMaterial;

	private Vector3 totalForce;

	// This param includes the safe distance
	[SerializeField]
	public SteeringParams alignParams;

	[SerializeField]
	public SteeringParams cohesionParams;

	[SerializeField]
	public SteeringParams separationParams;


	/// <summary>
	/// Gets or sets the seeking target.
	/// </summary>
	/// <value>The seeking target.</value>
	public Transform SeekingTarget { get; set; }

	#region implemented abstract members of Vehicle

	/// <summary>
	/// Gets the steering force.
	/// </summary>
	/// <returns>The total steering force.</returns>
	protected override Vector3 GetTotalSteeringForce ()
	{
		totalForce = Vector3.zero;

		totalForce += SteeringForce.GetSeekingForce (this, SeekingTarget.position) * seekingParams.ForceScale;

		totalForce += GetTotalObstacleAvoidanceForce () * avoidingParams.ForceScale;

		totalForce += GetTotalNeighborSeparationForce () * separationParams.ForceScale;

		totalForce += GetTotalNeighborAlignmentForce () * alignParams.ForceScale;

		totalForce += GetTotalNeighborCohesionForce () * cohesionParams.ForceScale;

		totalForce += GetBoundingForce () * boundingParams.ForceScale;

		totalForce.y = 0;

		return Vector3.ClampMagnitude (totalForce, maxForce);
	}

	#endregion


	/// <summary>
	/// Gets the total neighbor separation force.
	/// </summary>
	/// <returns>The total neighbor separation force.</returns>
	protected Vector3 GetTotalNeighborSeparationForce ()
	{
		var nearbyNeighbors = ParentSystem.FindCloseProximityInstances (this, ColliderInstance.GetAverageXZLength ());

		if (nearbyNeighbors.Count == 0) {
			return Vector3.zero;
		}
			
		return SteeringForce.GetNeighborSeparationForce (this, nearbyNeighbors);
	}

	/// <summary>
	/// Gets the total neighbor alignment force.
	/// </summary>
	/// <returns>The total neighbor alignment force.</returns>
	protected Vector3 GetTotalNeighborAlignmentForce ()
	{
		
		var flockAverageVelocity = ParentSystem.FlockAverageVelocityMap [GridCoordinate];

		if (flockAverageVelocity == Vector3.zero) {
			return Vector3.zero;
		}

		return SteeringForce.GetSteeringForce (this, flockAverageVelocity);
	}

	/// <summary>
	/// Gets the total neighbor cohesion force.
	/// </summary>
	/// <returns>The total neighbor cohesion force.</returns>
	protected Vector3 GetTotalNeighborCohesionForce ()
	{
		var flockAveragePosition = ParentSystem.FlockAveragePositionMap [GridCoordinate];

		if (flockAveragePosition == Vector3.zero) {
			return Vector3.zero;
		}
			
		return SteeringForce.GetSeekingForce (this, flockAveragePosition);
	}

	protected override void Awake ()
	{
		base.Awake ();
	}

	protected override void Update ()
	{
		base.Update ();
		// Grab the direction from the system here
	}

	protected override void LateUpdate ()
	{
		base.LateUpdate ();

		transform.position = BoundingPlane.GetSampledPosition (transform.position, ColliderInstance);
	}


	/// <summary>
	/// Draw debug line to current target
	/// </summary>
	protected override void OnRenderObject ()
	{
		glLineMaterial.SetPass (0);

		GL.PushMatrix ();

		base.OnRenderObject ();

		GL.PopMatrix ();
	}
}
