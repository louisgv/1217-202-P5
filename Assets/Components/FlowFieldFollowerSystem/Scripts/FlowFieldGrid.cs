using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Flow field grid.
/// Mainly used to visualize the flow field
/// </summary>
public class FlowFieldGrid : MonoBehaviour
{
	/// <summary>
	/// Gets or sets the flow direction.
	/// </summary>
	/// <value>The flow direction.</value>
	public Vector3 FlowDirection {
		get;
		set;
	}

	// Use this for initialization
	/// <summary>
	/// Raises the draw gizmos event.
	/// </summary>
	protected virtual void OnDrawGizmos ()
	{
		//TODO: Comment these out in prod

		Gizmos.color = Color.black;

		Gizmos.DrawLine (
			transform.position - FlowDirection / 2, 
			transform.position + FlowDirection / 2);
	}
}