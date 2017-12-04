using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Debug line.
/// A wrapper to toggle the debug line 
/// as well as toggle the state of some 
/// game object
/// Attached to: Most buttons in the HUD
/// Author: LAB
/// </summary>
public class DebugLine : MonoBehaviour
{
	
	public GameObject target;

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

	/// <summary>
	/// Toggles the target active state.
	/// </summary>
	public void ToggleTarget ()
	{
		target.SetActive (!target.activeSelf);
	}

	/// <summary>
	/// Toggles the resistance area debug.
	/// </summary>
	public void ToggleResistanceArea ()
	{
		ResistanceArea.debugLine = !ResistanceArea.debugLine;
	}
}
