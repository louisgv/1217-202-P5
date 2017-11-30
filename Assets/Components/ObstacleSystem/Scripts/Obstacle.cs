using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Obstacle.
/// Does nothing at the moment
/// Attached to: Obstacle
/// Author: LAB
/// </summary>
public class Obstacle : SpawningGridComponent
{

	private ObstacleCollider colliderInstance;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	private void Awake ()
	{
		colliderInstance = GetComponent <ObstacleCollider> ();
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	private void Start ()
	{
		transform.position = BoundingPlane.GetSampledPosition (transform.position, colliderInstance);
	}
}
