using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImprisonmentPickup : Pickup {
	
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
			bool needToColor = false;
			foreach( PlayerController playerController in playersToWorkAround )
			{
				float dist = Vector3.Distance( gridObject.transform.position, playerController.transform.position );
				if( dist < 0.3 * 12 && dist > 0.3 * 5 )
				{
					needToColor = true;
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

