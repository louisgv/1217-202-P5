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
	public void Toggle ()
	{
		Vehicle.debugLine = !Vehicle.debugLine;
	}
}
