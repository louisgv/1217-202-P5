using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: LAB <lab@mail.rit.edu>
/// Description: Manage and cycle through a list of camera
/// Restriction: N/A
/// </summary>

public class CameraManager : MonoBehaviour
{
	public List<SmoothFollow> cameraPrefabs;

	private List<GameObject> cameras;

	private int currentCameraIndex;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	private void Awake ()
	{
		cameras = new List<GameObject> ();
	}

	/// <summary>
	/// Initialize current index, disable all camera except the first
	/// </summary>
	private void Start ()
	{
		currentCameraIndex = 0;

		foreach (var cameraPrefab in cameraPrefabs) {
			if (GameObject.FindGameObjectWithTag (cameraPrefab.targetTag) == null) {
				continue;
			}

			var cameraInstance = Instantiate (cameraPrefab, transform).gameObject;

			cameraInstance.SetActive (false);

			cameras.Add (cameraInstance);
		}

		if (cameras.Count > 0) {
			cameras [0].SetActive (true);
		}
	}

	/// <summary>
	/// Cycle through the available camera in the camera array
	/// </summary>
	/// <returns>The next camera index</returns>
	private int GetNextCameraIndexCyclic ()
	{
		// Cycle index using mod op
		return (currentCameraIndex + 1) % cameras.Count;
	}

	/// <summary>
	/// Cycle through the available camera in the camera array
	/// </summary>
	/// <returns>The next camera index</returns>
	private int GetPreviousCameraIndexCyclic ()
	{
		// Cycle index using mod op
		return (currentCameraIndex + cameras.Count - 1) % cameras.Count;
	}

	/// <summary>
	/// Disable camera at specified index
	/// </summary>
	/// <param name="index">camera index</param>
	private void DisableCamera (int index)
	{
		cameras [index].SetActive (false);
	}

	/// <summary>
	/// Enables the camera at specified index
	/// </summary>
	/// <param name="index">Index.</param>
	private void EnableCamera (int index)
	{
		cameras [index].SetActive (true);
	}


	/// <summary>
	/// Swap to the previous camera.
	/// </summary>
	public void SwapPreviousCamera ()
	{		
		DisableCamera (currentCameraIndex);

		currentCameraIndex = GetPreviousCameraIndexCyclic ();

		EnableCamera (currentCameraIndex);
	}

	/// <summary>
	/// Swap to the next camera.
	/// </summary>
	public void SwapNextCamera ()
	{
		DisableCamera (currentCameraIndex);

		currentCameraIndex = GetNextCameraIndexCyclic ();

		EnableCamera (currentCameraIndex);
	}

	/// <summary>
	/// Check for C key and cycle through the list of available camera
	/// </summary>
	private void Update ()
	{
		if (Input.GetKeyDown (KeyCode.C)) {
			SwapNextCamera ();
		}
	}
}
