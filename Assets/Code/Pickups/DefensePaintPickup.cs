using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefensePaintPickup : Pickup {

	protected override void Start()
	{
		base.Start();
		GetComponent<Renderer>().material.color = Color.yellow;
	}
	
	protected override void PickUp (PlayerController player)
	{
		List<PlayerController> playersToWorkAround = new List<PlayerController>();
		foreach( PlayerController playerController in FindObjectsOfType<PlayerController>() )
		{
			if( playerController.playerColor != player.playerColor )
			{
				playersToWorkAround.Add( playerController );
			}
		}

		// Paint the whole map in the color of the player.
		foreach( GridObject gridObject in FindObjectsOfType<GridObject>() )
		{
			bool needToColor = true;
			foreach( PlayerController playerController in playersToWorkAround )
			{
				if( Vector3.Distance( gridObject.transform.position, playerController.transform.position ) < gridObject.GetComponent<Collider>().bounds.size.x * 3 )
				{
					needToColor = false;
				}
			}

			if( needToColor )
		    {
				gridObject.hitByBullet( player.playerColor );
			}
		}

		base.PickUp (player);
	}
}
