using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Grid coordinate.
/// Only has x and z position, for batching traversal
/// Author: LAB
/// </summary>
public class SpawningGridCoordinate
{
	private static string[] DIRECTIONS = new string[]{ "N", "E", "S", "W" };

	private static readonly Dictionary<string, Vector3> AdjacentDirection
	= new Dictionary<string, Vector3> {
		{ "N", Vector3.forward },
		{ "E", Vector3.right },
		{ "S", Vector3.back },
		{ "W", Vector3.left }
	};

	private static readonly Dictionary<string, Vector3> OrthogonalDirection
	= new Dictionary<string, Vector3> {
		{ "N", Vector3.right },
		{ "E", Vector3.back },
		{ "S", Vector3.left },
		{ "W", Vector3.forward }
	};

	private Dictionary<string, int> maxTracingBound;

	/// <summary>
	/// Gets the x.
	/// </summary>
	/// <value>The x.</value>
	public int X { get; private set; }

	/// <summary>
	/// Gets the z.
	/// </summary>
	/// <value>The z.</value>
	public int Z { get; private set; }

	/// Casting to V3 operator
	/// <param name="c">C.</param>
	public static implicit operator Vector3 (SpawningGridCoordinate c)
	{	
		return c != null
			? new Vector3 (c.X, 0, c.Z)
			: Vector3.zero;
	}

	/// <summary>
	/// Gets the width of the grid.
	/// </summary>
	/// <value>The width of the grid.</value>
	public float GridSize { get; private set; }

	/// <summary>
	/// Gets the global grid resolution.
	/// </summary>
	/// <value>The grid resolution.</value>
	public int GridResolution { get; private set; }

	public int MaxTracingLevel { get; private set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="SpawningGridCoordinate"/> class.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="z">The z coordinate.</param>
	/// <param name="gridWith">Grid with.</param>
	public SpawningGridCoordinate (int x, int z, float gridSize, int gridResolution)
	{
		X = x;
		Z = z;
		GridSize = gridSize;
		GridResolution = gridResolution;

		// QUITE MEMORY INEFFICIENT... THESE CAN BE CACHED
		// AND USED BY ALL INSTANCES IN THE SAME GRID POSITION....
		// BUT OPTIMIZE THAT SHOULD BE TRIVIAL...
		maxTracingBound = new Dictionary<string, int> ();

		// Both the max value can be cached using a single store class
		// Since they are the same accross object with the same grid...
		// Make me wonder...
		// If each component should be entitled to a single grid object
		// Or multiple should share a grid object...

		// So the board will need to first instantiate all
		// of these grid, then assign 'em to each object...
		MaxTracingLevel = Mathf.Max (
			maxTracingBound ["N"] = gridResolution - z,
			maxTracingBound ["E"] = gridResolution - x,
			maxTracingBound ["S"] = gridResolution + z,
			maxTracingBound ["W"] = gridResolution + x
		);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="SpawningGridCoordinate"/> struct.
	/// </summary>
	/// <param name="pos">Position.</param>
	/// <param name="gridWidth">Grid width.</param>
	public SpawningGridCoordinate (Vector3 pos, float gridSize, int gridResolution)
		: this (
			// NOTE: In order to cache the entire grid board
			// before hand, there need yet..
			// Another DS to hold only X and Z
			// So that we can log them into a 2D matrix...
			Mathf.CeilToInt (pos.x / gridSize),
			Mathf.CeilToInt (pos.z / gridSize),
			gridSize,
			gridResolution
		)
	{
	}

	/// <summary>
	/// Gets the adjacent grid.
	/// </summary>
	/// <returns>The adjacent grid.</returns>
	/// <param name="dir">Dir.</param>
	/// <param name="levelX">Level x.</param>
	/// <param name="levelZ">Level z.</param>
	public SpawningGridCoordinate GetAdjacentGrid (
		Vector3 dir, Vector3 diff
	)
	{
		return new SpawningGridCoordinate (
			X + (int)dir.x + (int)diff.x, 
			Z + (int)dir.z + (int)diff.z, 
			GridSize, GridResolution);
	}


	/// <summary>
	/// Gets the adjacent grids.
	/// </summary>
	/// <returns>The adjacent grids.</returns>
	public List <SpawningGridCoordinate> GetAdjacentGrids (int level = 1)
	{
		if (level > MaxTracingLevel) {
			return null;
		}

		var output = new List<SpawningGridCoordinate> ();

		if (level == 0) {
			output.Add (this);
			return output;
		}

		foreach (string dir in DIRECTIONS) {
			if (level > maxTracingBound [dir]) {
				continue;
			}

			var adjacentDir = AdjacentDirection [dir];
			var adjacentDirScaled = adjacentDir * level;

			var orthogonalDir = OrthogonalDirection [dir];
			var orthogonalDirScaled = orthogonalDir * level;

			// The pattern of traversal looks like the flying wheel..

			for (int i = 0; i <= level; i++) {
				var adjacentGrid = GetAdjacentGrid (adjacentDirScaled, orthogonalDir * i);
				output.Add (adjacentGrid);
			}

			for (int i = 1; i <= level; i++) {
				var adjacentGrid = GetAdjacentGrid (
					                   adjacentDirScaled + orthogonalDirScaled, 
					                   -adjacentDir * i);
				output.Add (adjacentGrid);
			}
		}

		return output;
	}

	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="SpawningGridCoordinate"/>.
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="SpawningGridCoordinate"/>.</returns>
	public override string ToString ()
	{
		return string.Format ("{0}-{1}", X, Z);
	}

	/// <summary>
	/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="SpawningGridCoordinate"/>.
	/// </summary>
	/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="SpawningGridCoordinate"/>.</param>
	/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
	/// <see cref="SpawningGridCoordinate"/>; otherwise, <c>false</c>.</returns>
	public override bool Equals (object obj)
	{
		var otherGrid = obj as SpawningGridCoordinate;

		if (otherGrid == null) {
			// If it is null then it is not equal to this instance.
			return false;
		}

		return X == otherGrid.X && Z == otherGrid.Z;
	}

	/// <summary>
	/// Serves as a hash function for a <see cref="SpawningGridCoordinate"/> object.
	/// </summary>
	/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
	public override int GetHashCode ()
	{
		// Since it's a mono behavior anyway, this shoudl suffice
		return new Vector3 (X, 0, Z).GetHashCode ();
	}
}