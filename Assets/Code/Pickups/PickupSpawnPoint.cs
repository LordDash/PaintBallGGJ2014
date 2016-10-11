using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickupSpawnPoint : MonoBehaviour {
	static private readonly float timeToSpawn = 354.0f;
	static private readonly float minTime = 7.5f;
	static private readonly float maxTime = 21.0f;

	public List<Pickup> possiblePickups;

	private float randomTime = 0.0f;
	private float lastSpawnTime = 0.0f;

	void Start()
	{
		randomTime = Random.value * timeToSpawn;
		randomTime = Mathf.Clamp( randomTime, minTime, maxTime );
		lastSpawnTime = Time.time;
	}
	
	void Update()
	{
		float rand_pickup = Random.value;

		if( Time.time - randomTime > lastSpawnTime )
		{
			rand_pickup = Random.value;
			float number_pickups = (float)possiblePickups.Count;
			// rand_pickup is between 0.0 and 1.0, we need between 0 and number_pickups (=2 - 1 = 1 = either 0 or 1)
			rand_pickup *= 100;
			rand_pickup /= (100 / number_pickups);  
			int index = Mathf.Clamp((int)rand_pickup, 0, (int)number_pickups - 1);
			Debug.Log( "The index of the pickup is: " + index );
			Instantiate( possiblePickups[index], transform.position + Vector3.up * 0.5f, Quaternion.identity );

			randomTime = Random.value * timeToSpawn;
			randomTime = Mathf.Clamp( randomTime, minTime, maxTime );
			lastSpawnTime = Time.time;
		}
	}
}

