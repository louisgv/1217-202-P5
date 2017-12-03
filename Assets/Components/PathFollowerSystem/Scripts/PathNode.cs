using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Path node.
/// Has some helper static method used to check normal boundary
/// Author: LAB
/// Attached to: PathNode
/// </summary>
public class PathNode : MonoBehaviour
{
	/// Overide the minus operator
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	public static Vector3 operator - (PathNode a, PathNode b)
	{
		return a.transform.position - b.transform.position;
	}

	/// <summary>
	/// Get the min X of 2 nodes
	/// </summary>
	/// <returns>The x.</returns>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	public static float MinX (PathNode a, PathNode b)
	{
		return Mathf.Min (a.transform.position.x, b.transform.position.x);
	}

	/// <summary>
	/// Get the max X of 2 nodes
	/// </summary>
	/// <returns>The x.</returns>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	public static float MaxX (PathNode a, PathNode b)
	{
		return Mathf.Max (a.transform.position.x, b.transform.position.x);
	}

	/// <summary>
	/// Get the min Y of 2 nods
	/// </summary>
	/// <returns>The y.</returns>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	public static float MinY (PathNode a, PathNode b)
	{
		return Mathf.Min (a.transform.position.y, b.transform.position.y);
	}

	/// <summary>
	/// Get the max y of 2 nods
	/// </summary>
	/// <returns>The y.</returns>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	public static float MaxY (PathNode a, PathNode b)
	{
		return Mathf.Max (a.transform.position.y, b.transform.position.y);
	}

}
