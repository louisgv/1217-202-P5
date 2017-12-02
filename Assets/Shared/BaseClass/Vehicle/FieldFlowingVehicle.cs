using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Field flowing vehicle.
/// A Smart vehicle that implement method to Follow the flow field 
/// defined by its parent system
/// Author: LAB
/// </summary>
abstract public class FieldFlowingVehicle <V, S>: FlockingVehicle<V, S> 
	where V : Vehicle
	where S : SpawningSystem <V>
{

	[SerializeField]
	public SteeringParams flowfieldParams;


}
