using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class CameraCenterer : MonoBehaviour {

	private List<PlayerController> players = new List<PlayerController>();
	public Vector3 distance = Vector3.zero;
	public float maxDistance = 150.0f;

	void Start()
	{

	}

	void Update()
	{
		float lowestX = 0.0f;
		float lowestY = 0.0f;
		float lowestZ = 0.0f;

		float highestX = 0.0f;
		float highestY = 0.0f;
		float highestZ = 0.0f;

		if( players.Count <= 0 )
		{
			// Find the min and max of all players
			foreach( PlayerController player in FindObjectsOfType<PlayerController>() )
			{
				players.Add( player );
			}
		}

		if( players.Count > 1 )
		{
			foreach( PlayerController player in players )
			{
				if( player.transform.position.x < lowestX )
				{
					lowestX = player.transform.position.x;
				}

				if( player.transform.position.y < lowestY )
				{
					lowestY = player.transform.position.y;
				}

				if( player.transform.position.z < lowestZ )
				{
					lowestZ = player.transform.position.z;
				}

				if( player.transform.position.x > highestX )
				{
					highestX = player.transform.position.x;
				}

				if( player.transform.position.y > highestY )
				{
					highestY = player.transform.position.y;
				}

				if( player.transform.position.z > highestZ )
				{
					highestZ = player.transform.position.z;
				}
			}

			// We now have the bounds
			Vector3 lowestPos = new Vector3( lowestX, lowestY, lowestZ );
			Vector3 highestPos = new Vector3( highestX, highestY, highestZ );

			// The camera must look at the center of this.
			Vector3 middle = ( lowestPos + highestPos )/ 2;

			float dist = ( highestPos - lowestPos ).sqrMagnitude;
			dist = Mathf.Clamp( dist, 0.0f, maxDistance );

			GetComponent<Camera>().transform.position = middle + ( distance - ( GetComponent<Camera>().transform.forward * (0.1f * dist ) ) );
		}
	}

}

