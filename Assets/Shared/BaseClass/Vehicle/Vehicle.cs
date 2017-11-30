using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Vehicle base class.
/// Author LAB
/// Attached to: N/A
/// </summary>
public abstract class Vehicle : SpawningGridComponent
{
	public static bool debugLine = true;
	
	[SerializeField]
	private float mass = 1.0f;

	[SerializeField, Range (0, 20f)]
	protected float maxForce = 5.0f;

	[SerializeField, Range (0, 20f)]
	protected float maxSpeed = 10.0f;

	/// <summary>
	/// The max fleeing velocity.
	/// </summary>
	[SerializeField, Range (0, 20f)]
	protected float maxSteeringSpeed = 1;

	/// <summary>
	/// Gets the max speed.
	/// </summary>
	/// <value>The max speed.</value>
	public float MaxSteeringSpeed {
		get {
			return maxSteeringSpeed;
		}
	}

	/// <summary>
	/// Gets the max steering speed squared.
	/// </summary>
	/// <value>The max steering speed squared.</value>
	public float MaxSteeringSpeedSquared {
		get {
			return maxSteeringSpeed * maxSteeringSpeed;
		}
	}

	/// <summary>
	/// Gets the max speed squared.
	/// </summary>
	/// <value>The max speed squared.</value>
	public float MaxSpeedSquared {
		get {
			return maxSpeed * maxSpeed;
		}
	}

	private Vector3 acceleration;

	[SerializeField]
	public SteeringParams fleeingParams;

	[SerializeField]
	public SteeringParams seekingParams;

	[SerializeField]
	public SteeringParams wanderingParams;

	private float wanderAngle = 0;

	/// <summary>
	/// Gets the wander angle.
	/// </summary>
	/// <value>The wander angle.</value>
	public float WanderAngle { get { return wanderAngle; } set { wanderAngle = value; } }

	private float wanderRange = 0.333f;

	/// <summary>
	/// Gets or sets the wander range.
	/// </summary>
	/// <value>The wander range.</value>
	public float WanderRange { get { return wanderRange; } }

	/// <summary>
	/// Gets the direction.
	/// </summary>
	/// <value>The direction.</value>
	public Vector3 Direction { get; protected set; }

	/// <summary>
	/// Gets the velocity.
	/// </summary>
	/// <value>The velocity.</value>
	public Vector3 Velocity { get ; protected set; }

	/// <summary>
	/// Gets the future position.
	/// </summary>
	/// <value>The future position.</value>
	public Vector3 FuturePosition { get; set; }

	/// <summary>
	/// Gets the steering force.
	/// </summary>
	protected abstract Vector3 GetTotalSteeringForce ();

	/// <summary>
	/// Applies ann add-on acceleration.
	/// </summary>
	/// <param name="addonAcceleration">Addon acceleration.</param>
	protected void ApplyAcceleration (Vector3 addonAcceleration)
	{
		acceleration += addonAcceleration;
	}

	/// <summary>
	/// Applies the force to our acceleartion.
	/// </summary>
	/// <param name="force">Force.</param>
	protected void ApplyForce (Vector3 force)
	{
		acceleration += force / mass;
	}

	/// <summary>
	/// Move this instance.
	/// </summary>
	protected void Move ()
	{
		Velocity += acceleration * Time.deltaTime;

		Velocity = Vector3.ClampMagnitude (Velocity, maxSpeed);

		Direction = Velocity.normalized;

		transform.position += Velocity * Time.deltaTime;
	}

	/// <summary>
	/// Faces the moving direction.
	/// </summary>
	protected void RotateTowardMovingDirection ()
	{
		var rotationAngle = Mathf.Atan2 (Direction.x, Direction.z) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.Euler (0, rotationAngle, 0);
	}

	/// <summary>
	/// Reset variable that has small rate of change, e.g acceleration
	/// </summary>
	protected virtual void Reset ()
	{
		acceleration = Vector3.zero;
	}

	#region Unity Lifecycle

	protected virtual void Awake ()
	{

	}

	protected virtual void Start ()
	{

	}

	protected virtual void Update ()
	{
		ApplyForce (GetTotalSteeringForce ());
	}

	protected virtual void LateUpdate ()
	{
		// Move in late update to ensure it won't be called if
		// object is destroyed
		Move ();

		RotateTowardMovingDirection ();

		Reset ();
	}

	#endregion

	/// <summary>
	/// Draws the future marker.
	/// </summary>
	public void DrawFutureMarker (Color color)
	{
		DrawDebugMark (FuturePosition, color);
	}

	/// <summary>
	/// Draws the debug line from transform' center.
	/// </summary>
	protected void DrawDebugLine (Vector3 end, Color color)
	{
		if (!debugLine) {
			return;
		}
		Debug.DrawLine (transform.position, end, color);
		GL.Begin (GL.LINES);
		GL.Color (color);
		GL.Vertex (transform.position);
		GL.Vertex (end);
		GL.End ();
	}

	/// <summary>
	/// Draws a debug mark at location infront.
	/// </summary>
	/// <param name="pos">Position.</param>
	/// <param name="color">Color.</param>
	protected void DrawDebugMark (Vector3 pos, Color color)
	{
		if (!debugLine) {
			return;
		}

		GL.Begin (GL.LINES);
		GL.Color (color);
		GL.Vertex (pos + transform.right);
		GL.Vertex (pos - transform.right);
		GL.Vertex (pos);
		GL.Vertex (pos + transform.forward);
		GL.Vertex (pos + transform.up);
		GL.Vertex (pos - transform.up);
		GL.End ();
	}

	/// <summary>
	/// Raises the render object event.
	/// </summary>
	protected virtual void OnRenderObject ()
	{
		DrawDebugLine (transform.position + transform.right * 3.0f, Color.blue);

		DrawDebugLine (transform.position + transform.forward * 3.0f, Color.green);
	}

}