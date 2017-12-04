using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Resistance area.
/// Implement a resistance area that slow vehicle down
/// Author: LAB
/// Attached to: ResistanceArea
/// </summary>
public class ResistanceArea : CustomBoxCollider
{
	public float dragCoefficient;

	public static bool debugLine = false;

	public Material glLineMaterial;


	/// <summary>
	/// Draws a debug mark at current location.
	/// </summary>
	/// <param name="pos">Position.</param>
	/// <param name="color">Color.</param>
	protected void DrawDebugMark (Vector3 pos, Color color)
	{
		if (!debugLine) {
			return;
		}

		var zPos = transform.forward * size.z / 2;
		var xPos = transform.right * size.x / 2;

		var top = pos + zPos;
		var right = pos + xPos;
		var bottom = pos - zPos;
		var left = pos - xPos;

		GL.Begin (GL.LINES);
		GL.Color (color);
		GL.Vertex (top);
		GL.Vertex (right);

		GL.Vertex (right);
		GL.Vertex (bottom);

		GL.Vertex (bottom);
		GL.Vertex (left);

		GL.Vertex (left);
		GL.Vertex (top);
		GL.End ();
	}

	/// <summary>
	/// Raises the render object event.
	/// </summary>
	protected void OnRenderObject ()
	{
		glLineMaterial.SetPass (0);

		GL.PushMatrix ();

		DrawDebugMark (transform.position, Color.black);

		GL.PopMatrix ();
	}
}
