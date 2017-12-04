using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Flow field grid.
/// Mainly used to visualize the flow field
/// Author: LAB
/// Attached to: FlowFieldGrid
/// </summary>
public class FlowFieldGrid : MonoBehaviour
{
	public static bool debugLine = false;
	public Material glLineMaterial;

	/// <summary>
	/// Gets or sets the flow direction.
	/// </summary>
	/// <value>The flow direction.</value>
	public Vector3 FlowDirection {
		get;
		set;
	}

	/// <summary>
	/// Draws the debug line from transform' center.
	/// </summary>
	protected void DrawDebugLine (Vector3 start, Vector3 end, Color color)
	{
		if (!debugLine) {
			return;
		}
		Debug.DrawLine (start, end, color);
		GL.Begin (GL.LINES);
		GL.Color (color);
		GL.Vertex (start);
		GL.Vertex (end);
		GL.End ();
	}

	/// <summary>
	/// Raises the render object extended event.
	/// </summary>
	protected virtual void DrawDebugLines ()
	{
		DrawDebugLine (transform.position, transform.position + FlowDirection, Color.black);
	}

	/// <summary>
	/// Raises the render object event.
	/// </summary>
	protected void OnRenderObject ()
	{
		glLineMaterial.SetPass (0);

		GL.PushMatrix ();

		DrawDebugLines ();

		GL.PopMatrix ();
	}

}