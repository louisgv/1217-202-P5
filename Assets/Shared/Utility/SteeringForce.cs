using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Steering mode public enum.
/// </summary>
public enum SteeringMode
{
	SEEKING,
	FLEEING
}

/// <summary>
/// Steering force static class.
/// All force function assumed mass is 1 and force is applied every 1 second
/// This class implement the most basic of steering.
/// For steering behavior that's heavily dependent on its
/// parent system (flocker, path follower, flow field follower),
/// their force implementatioin is in their abstract class
/// Author: LAB
/// Attached to: N/A
/// </summary>
public static class SteeringForce
{
	internal delegate Vector3 SteeringFx (Vehicle vehicle, Vector3 target);

	internal static SteeringFx[] steeringFunctions = {
		GetSeekingForce,
		GetFleeingForce
	};

	/// <summary>
	/// Gets the steering force.
	/// </summary>
	/// <returns>The steering force.</returns>
	/// <param name="steeringFx">Steering type.</param>
	/// <param name="targetTransform">Target transform.</param>
	internal static Vector3 GetSteeringForce (Vehicle vehicle, Transform targetTransform, SteeringMode sMode)
	{
		return targetTransform == null
			? Vector3.zero
			: steeringFunctions [(int)sMode] (vehicle, targetTransform.position);
	}

	/// <summary>
	/// Gets the steering force based on desired velocity.
	/// </summary>
	/// <returns>The steering force.</returns>
	/// <param name="desiredVelocity">Desired velocity.</param>
	internal static Vector3 GetSteeringForce (Vehicle vehicle, Vector3 desiredVelocity)
	{
		var steeringForce = desiredVelocity - vehicle.Velocity;

		return steeringForce;
	}


	/// <summary>
	/// Gets the fleeing force.
	/// </summary>
	/// <returns>The fleeing force.</returns>
	/// <param name="target">Target.</param>
	internal static Vector3 GetFleeingForce (Vehicle vehicle, Vector3 target)
	{
		var diff = vehicle.transform.position - target;

		var desiredVelocity = diff.normalized * vehicle.MaxSteeringSpeed;

		return GetSteeringForce (vehicle, desiredVelocity);
	}

	/// <summary>
	/// Gets the seeking force.
	/// </summary>
	/// <returns>The seeking force.</returns>
	/// <param name="target">Target.</param>
	internal static Vector3 GetSeekingForce (Vehicle vehicle, Vector3 target)
	{
		var diff = target - vehicle.transform.position;

		var distanceSquared = diff.sqrMagnitude;

		// Arival implementation:
		float desiredSpeed = 
			(vehicle.seekingParams.ThresholdSquared > distanceSquared) 
			? ProcessingMap (
				distanceSquared, 0, 
				vehicle.seekingParams.ThresholdSquared, 0, 
				vehicle.MaxSteeringSpeed)
			: vehicle.MaxSteeringSpeed;

		var desiredVelocity = diff.normalized * desiredSpeed;

		return GetSteeringForce (vehicle, desiredVelocity);
	}

	/// <summary>
	/// Gets the wandering force.
	/// </summary>
	/// <returns>The wandering force.</returns>
	/// <param name="target">Target.</param>
	internal static Vector3 GetWanderingForce (Vehicle vehicle)
	{
		// Get a random rotation position
		vehicle.WanderAngle += new Vector2 (0, Random.Range (-1f, 1f) * vehicle.WanderRange);

		var wanderCircle = (vehicle.transform.forward + UnitRotationY (vehicle.WanderAngle.y));

		// Reconstruct the target position
		var finalTarget = vehicle.transform.position +
		                  wanderCircle * vehicle.wanderingParams.ThresholdSquared;

		//	Debug.DrawLine (vehicle.transform.position, finalTarget, Color.black);

		return GetSeekingForce (vehicle, finalTarget);
	}

	/// <summary>
	/// Gets the wandering force in 3 dimensions.
	/// </summary>
	/// <returns>The wandering force.</returns>
	/// <param name="target">Target.</param>
	internal static Vector3 GetWanderingForce3D (Vehicle vehicle)
	{
		vehicle.WanderAngle += new Vector2 (Random.Range (-1f, 1f), Random.Range (-1f, 1f)) * vehicle.WanderRange;

		var wanderSphere = vehicle.transform.forward +
		                   UnitRotationY (vehicle.WanderAngle.y) +
		                   UnitRotationX (vehicle.WanderAngle.x);

		// Reconstruct the target position
		var finalTarget = vehicle.transform.position +
		                  wanderSphere * vehicle.wanderingParams.ThresholdSquared;

		//	Debug.DrawLine (vehicle.transform.position, finalTarget, Color.black);

		return GetSeekingForce (vehicle, finalTarget);
	}

	/// <summary>
	/// An unit vector describing rotation at the specified angle.
	/// </summary>
	/// <returns>The rotation.</returns>
	/// <param name="angle">Angle.</param>
	internal static Vector3 UnitRotationY (float angle)
	{
		return new Vector3 (Mathf.Cos (angle), 0, Mathf.Sin (angle));
	}

	/// <summary>
	/// An unit vector describing rotation at the specified angle.
	/// </summary>
	/// <returns>The rotation.</returns>
	/// <param name="angle">Angle.</param>
	internal static Vector3 UnitRotationX (float angle)
	{
		return new Vector3 (0, Mathf.Cos (angle), Mathf.Sin (angle));
	}


	/// <summary>
	/// Gets the bounding force.
	/// </summary>
	/// <returns>The bounding force.</returns>
	internal static Vector3 GetBoundingForce (Vehicle vehicle, CustomBoxCollider boundingPlane)
	{
		var minBound = boundingPlane.GetMinBound ();
		var maxBound = boundingPlane.GetMaxBound ();

		Vector3 desiredVelocity = Vector3.zero;

		float vehicleX = vehicle.transform.position.x;
		float vehicleZ = vehicle.transform.position.z;

		float maxSteeringSpeed = vehicle.MaxSteeringSpeed;

		if (vehicleX > maxBound.x) {
			desiredVelocity = new Vector3 (-maxSteeringSpeed, 0, vehicle.Velocity.z);
		} else if (vehicleX < minBound.x) {
			desiredVelocity = new Vector3 (maxSteeringSpeed, 0, vehicle.Velocity.z);
		}

		if (vehicleZ > maxBound.z) {
			desiredVelocity = new Vector3 (vehicle.Velocity.x, 0, -maxSteeringSpeed);
		} else if (vehicleZ < minBound.z) {
			desiredVelocity = new Vector3 (vehicle.Velocity.x, 0, maxSteeringSpeed);
		}

		if (desiredVelocity.Equals (Vector3.zero)) {
			return Vector3.zero;
		}

		desiredVelocity = desiredVelocity.normalized * maxSteeringSpeed;

		return GetSteeringForce (vehicle, desiredVelocity);
	}

	/// <summary>
	/// Gets the bounding force in 3D.
	/// </summary>
	/// <returns>The bounding force.</returns>
	internal static Vector3 GetBoundingForce3D (Vehicle vehicle, CustomBoxCollider boundingPlane)
	{
		var minBound = boundingPlane.GetMinBound ();
		var maxBound = boundingPlane.GetMaxBound ();

		Vector3 desiredVelocity = Vector3.zero;

		float vehicleX = vehicle.transform.position.x;
		float vehicleY = vehicle.transform.position.y;
		float vehicleZ = vehicle.transform.position.z;

		float maxSteeringSpeed = vehicle.MaxSteeringSpeed;

		if (vehicleX > maxBound.x) {
			desiredVelocity = new Vector3 (-maxSteeringSpeed, vehicle.Velocity.y, vehicle.Velocity.z);
		} else if (vehicleX < minBound.x) {
			desiredVelocity = new Vector3 (maxSteeringSpeed, vehicle.Velocity.y, vehicle.Velocity.z);
		}

		if (vehicleY > maxBound.y) {
			desiredVelocity = new Vector3 (vehicle.Velocity.x, -maxSteeringSpeed, vehicle.Velocity.z);
		} else if (vehicleY < minBound.y) {
			desiredVelocity = new Vector3 (vehicle.Velocity.x, maxSteeringSpeed, vehicle.Velocity.z);
		}

		if (vehicleZ > maxBound.z) {
			desiredVelocity = new Vector3 (vehicle.Velocity.x, vehicle.Velocity.y, -maxSteeringSpeed);
		} else if (vehicleZ < minBound.z) {
			desiredVelocity = new Vector3 (vehicle.Velocity.x, vehicle.Velocity.y, maxSteeringSpeed);
		}

		if (desiredVelocity.Equals (Vector3.zero)) {
			return Vector3.zero;
		}

		desiredVelocity = desiredVelocity.normalized * maxSteeringSpeed;

		return GetSteeringForce (vehicle, desiredVelocity);
	}

	/// <summary>
	/// Gets the obstacle avoidance force.
	/// </summary>
	/// <returns>The evasion force.</returns>
	/// <param name="target">Target.</param>
	internal static Vector3 GetAvoidanceForce (Vehicle vehicle, Vector3 diffRadar, float radiDistance)
	{
		var forwardProjection = Vector3.Dot (diffRadar, vehicle.transform.forward);

		// Object is behind, ignore
		if (forwardProjection < 0) {
			return Vector3.zero;
		}

		var rightProjection = Vector3.Dot (diffRadar, vehicle.transform.right);

		if (rightProjection > radiDistance) {
			return Vector3.zero;
		}

		var desiredDirection = (rightProjection > 0
		                       // Object to the right, turn left
			? -vehicle.transform.right
		                       // Object to the left, turn right
			: vehicle.transform.right);

		var desiredVelocity = desiredDirection.normalized * vehicle.MaxSteeringSpeed;

		return GetSteeringForce (vehicle, desiredVelocity);
	}

	/// <summary>
	/// Gets the neighbor separating force.
	/// </summary>
	/// <returns>The obstacle evading force.</returns>
	internal static Vector3 GetNeighborSeparationForce (Vehicle vehicle, List<Transform> nearbyNeighbors)
	{
		var sum = Vector3.zero;

		foreach (var neighbor in nearbyNeighbors) {
			// TODO: Add distance factor to each of these
			var diff = vehicle.transform.position - neighbor.transform.position;

			sum += diff / diff.sqrMagnitude;
		}

		var desiredVelocity = (sum / (float)nearbyNeighbors.Count).normalized * vehicle.MaxSteeringSpeed;

		return GetSteeringForce (vehicle, desiredVelocity);
	}

	/// <summary>
	/// The map function from Processing
	/// </summary>
	/// <returns>The map.</returns>
	/// <param name="value">Value.</param>
	/// <param name="istart">Istart.</param>
	/// <param name="istop">Istop.</param>
	/// <param name="ostart">Ostart.</param>
	/// <param name="ostop">Ostop.</param>
	public static float ProcessingMap (float value, 
	                                   float istart, 
	                                   float istop, 
	                                   float ostart, 
	                                   float ostop)
	{
		return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
	}

}
