using UnityEngine;
using System.Collections;

public class InvinciblePaintPickup : Pickup {
	
	protected override void Start()
	{
		base.Start();
		GetComponent<Renderer>().material.color = Color.white;
	}
	
	protected override void PickUp (PlayerController player)
	{
		// The paint of this team can't be taken over by the other team.
		foreach( GridObject gridObject in FindObjectsOfType<GridObject>() )
		{
			if( gridObject.gameObject.layer == (int)player.playerColor )
			{
				gridObject.Invincible = true;
			}
		}

		base.PickUp (player);
	}
}
