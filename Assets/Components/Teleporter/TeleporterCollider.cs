using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Teleporter collider.
/// Check if the teleporter has collided with any of the prey in the assigned spawning system
/// If so, it will teleport to a random position on the provided terrain.
/// Author: LAB
/// Attached to: Teleporter
/// </summary>
public class TeleporterCollider : CustomBoxCollider
{
	private Teleporter teleporterInstance;

	[SerializeField]
	private FlockerSystem targetSystem;

	[SerializeField]
	private CustomBoxCollider plane;

	[SerializeField]
	private float teleportTime = 10f;

	private float timeout;

	public bool isTeleporting;

	/// <summary>
	/// Teleport this instance to a random position on the plane.
	/// </summary>
	public void Teleport ()
	{
		transform.position = plane.GetRandomPositionAbove (this);

		var newGridCoord = teleporterInstance.UpdatedGrid ();

		if (newGridCoord != null) {
			teleporterInstance.GridCoordinate = newGridCoord;
		}
	}

	protected override void Awake ()
	{
		base.Awake ();

		teleporterInstance = GetComponent <Teleporter> ();

		teleporterInstance.BoundingPlane = plane;
	}

	protected override void Start ()
	{
		base.Start ();
		Teleport ();

		var localGridPos = teleporterInstance.transform.position - plane.WorldCenter;

		teleporterInstance.GridCoordinate = 
			new SpawningGridCoordinate (localGridPos, 
			targetSystem.GridSize, 
			targetSystem.GridResolution);
	}

	protected override void Update ()
	{
		base.Update ();

		if (!isTeleporting) {
			return;
		}
		var nearbyTargets = targetSystem.FindCloseProximityInstances (teleporterInstance, 1);

		foreach (var target in nearbyTargets) {
			// Debug.DrawLine (transform.position, target.transform.position);

			var targetCollider = target.GetComponent <CustomBoxCollider> ();

			// If is colliding, wait for the telport time amount, then teleport
			if (IsCollidingWith (targetCollider)) {
				timeout += Time.deltaTime;

				if (timeout > teleportTime) {
					timeout = 0;
					Teleport ();
				}
				break;
			}
		}
	}
}
