using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Custom box collider.
/// It uttilizes AABB collision algorithm to check for collision
/// It is also used to calculate the square extend of an obstacle
/// </summary>
public class CustomBoxCollider : MonoBehaviour
{
	[SerializeField]
	protected Vector3 center;

	[SerializeField]
	protected Vector3 size;

	/// <summary>
	/// Gets the center.
	/// </summary>
	/// <value>The center.</value>
	public Vector3 Center {
		get {
			return center;
		}
	}

	/// <summary>
	/// Gets the size.
	/// </summary>
	/// <value>The size.</value>
	public Vector3 Size {
		get {
			return size;
		}
	}

	/// <summary>
	/// Gets the extend squared.
	/// </summary>
	/// <value>The extend squared.</value>
	public float ExtendSquared {
		get {
			return Vector3.Dot (size, size);
		}
	}

	/// <summary>
	/// Determines whether this instance is colliding using AABB algorithm.
	/// </summary>
	/// <returns><c>true</c> if this instance is colliding; otherwise, <c>false</c>.</returns>
	public bool IsCollidingWith (CustomBoxCollider other)
	{
		var ourMinBound = GetMinBound ();
		var otherMinBound = other.GetMinBound ();

		var ourMaxBound = GetMaxBound ();
		var otherMaxBound = other.GetMaxBound ();

		return GreaterComponentCompare (otherMaxBound, ourMinBound) &&
		GreaterComponentCompare (ourMaxBound, otherMinBound);
	}

	/// <summary>
	/// Return true if all of a's component are greater than b.
	/// </summary>
	/// <returns><c>true</c>, if component compare was greatered, <c>false</c> otherwise.</returns>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	internal bool GreaterComponentCompare (Vector3 a, Vector3 b)
	{
		return 	
		a.x > b.x &&
		a.y > b.y &&
		a.z > b.z;
	}

	/// <summary>
	/// Gets the world center.
	/// </summary>
	/// <value>The world center.</value>
	internal Vector3 WorldCenter {
		get {
			return transform.position + center;
		}
	}

	/// <summary>
	/// Gets the minimum bound.
	/// </summary>
	/// <returns>The minimum bound.</returns>
	internal Vector3 GetMinBound ()
	{
		return WorldCenter - GetHalfSize ();
	}

	/// <summary>
	/// Gets the max bound.
	/// </summary>
	/// <returns>The max bound.</returns>
	internal Vector3 GetMaxBound ()
	{
		return WorldCenter + GetHalfSize ();
	}

	/// <summary>
	/// Gets the half size of the collider.
	/// </summary>
	/// <returns>The half size.</returns>
	public Vector3 GetHalfSize ()
	{
		return size / 2f;
	}

	/// <summary>
	/// Gets the average 2D length, used for radius
	/// </summary>
	/// <returns>The largest side.</returns>
	public float GetAverageXZLength ()
	{
		return (Size.x + Size.z) / 2;
	}

	/// <summary>
	/// Samples the height.
	/// </summary>
	/// <returns>The height.</returns>
	/// <param name="pos">Position.</param>
	/// <param name="col">Col.</param>
	public Vector3 GetSampledPosition (Vector3 pos, CustomBoxCollider col)
	{
		pos.y += size.y + col.Size.y / 2;
		return pos;
	}

	/// <summary>
	/// Gets a random position along the XZ axis.
	/// </summary>
	/// <returns>The random position above.</returns>
	public Vector3 GetRandomPositionXZ ()
	{
		var randomX = Random.Range (-0.45f, 0.45f) * Size.x;

		var randomZ = Random.Range (-0.45f, 0.45f) * Size.z;

		return WorldCenter + new Vector3 (randomX, 0, randomZ);
	}

	/// <summary>
	/// Gets a random position above the plane collider.
	/// </summary>
	/// <returns>The random position above.</returns>
	/// <param name="col">Col.</param>
	public Vector3 GetRandomPositionAbove (CustomBoxCollider col)
	{
		return GetSampledPosition (GetRandomPositionXZ (), col);
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
		
	}

	protected virtual void LateUpdate ()
	{
		
	}

	#endregion


	/// <summary>
	/// Raises the draw gizmos event.
	/// </summary>
	protected virtual void OnDrawGizmos ()
	{
		Gizmos.color = Color.black;

		Gizmos.DrawWireCube (WorldCenter, size);
	}
}
