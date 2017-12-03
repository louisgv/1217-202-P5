using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Debug line.
/// A wrapper to toggle the debug line
/// </summary>
public class DebugLine : MonoBehaviour
{
	/// <summary>
	/// Toggle the static debug value
	/// </summary>
	public void ToggleVehicle ()
	{
		Vehicle.debugLine = !Vehicle.debugLine;
	}

	/// <summary>
	/// Toggles the flow field state.
	/// </summary>
	public void ToggleFlowField ()
	{
		FlowFieldGrid.debugLine = !FlowFieldGrid.debugLine;
	}
}
