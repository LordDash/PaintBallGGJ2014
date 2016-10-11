using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {
	#region Editor fields
	public List<Transform> spawnPoints;
	public List<CapturePoint> capturePoints;
	public CameraShake mapCamera;
	#endregion

	#region Fields


	List<Transform> listOfSpawnPointsToChooseFrom;
	#endregion

	#region Initilization
	void Awake() 
	{
		listOfSpawnPointsToChooseFrom = new List<Transform>(spawnPoints);
	}
	#endregion

	#region Methods

	public Transform GetSpawnPoint()
	{
		if(listOfSpawnPointsToChooseFrom.Count == 0)
			listOfSpawnPointsToChooseFrom = new List<Transform>(spawnPoints);

		Transform chosenSpawnPoint = listOfSpawnPointsToChooseFrom[Random.Range(0, listOfSpawnPointsToChooseFrom.Count)];

		listOfSpawnPointsToChooseFrom.Remove(chosenSpawnPoint);

		return chosenSpawnPoint;
	}
	#endregion
}
