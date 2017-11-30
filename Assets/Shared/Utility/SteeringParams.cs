using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Steering parameters.
/// Author: LAB
/// Used to supply parameter for each steering behavior
/// </summary>
[SerializableAttribute]
public class SteeringParams
{
	[SerializeField, Range (0, 100f)]
	protected float forceScale = 2.0f;

	[SerializeField]
	private float thresholdSquared = 36.0f;

	/// <summary>
	/// Gets the force scale.
	/// </summary>
	/// <value>The force scale.</value>
	public float ForceScale {
		get {
			return forceScale;
		}
	}

	/// <summary>
	/// Gets the threshold squared.
	/// </summary>
	/// <value>The threshold squared.</value>
	public float ThresholdSquared {
		get {
			return thresholdSquared;
		}
	}
}
	