using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Grid component.
/// A base MonoBehavior component that 
/// contains a definition for a SpawningGridCoordinate
/// Author: LAB
/// </summary>
public abstract class SpawningGridComponent : MonoBehaviour
{
	/// <summary>
	/// Gets the grid coordinate.
	/// </summary>
	/// <value>The grid coordinate.</value>
	public SpawningGridCoordinate GridCoordinate { get; set; }

	/// <summary>
	/// Gets or sets the bounding plane.
	/// </summary>
	/// <value>The bounding plane.</value>
	public CustomBoxCollider BoundingPlane { get; set; }

	/// <summary>
	/// Return an updated grid if the new grid is different from
	/// current grid position, else it return null
	/// as a checking mechanism
	/// </summary>
	/// <returns>Updated grid position, else null.</returns>
	public SpawningGridCoordinate UpdatedGrid ()
	{
		var localGridPos = transform.position - BoundingPlane.WorldCenter;

		if (GridCoordinate == null) {
			return null;
		}

		// NOTE: This can be done better with
		// A more thorough abstraction layer that is not 
		// Dependent on GridCoordinate being instantiated
		// Outside
		var updatedGrid = new SpawningGridCoordinate (
			                  localGridPos, GridCoordinate.GridSize,
			                  GridCoordinate.GridResolution
		                  );

		return  updatedGrid != GridCoordinate 
				? updatedGrid 
				: null;
	}
}
