using UnityEngine;
using System.Collections;

public class InvinciblePickup : Pickup {
	
	protected override void Start()
	{
		base.Start();
		GetComponent<Renderer>().material.color = Color.cyan;
	}
	
	protected override void PickUp (PlayerController player)
	{
		//Set the player invincibility
		player.Invincible = true;
		base.PickUp (player);
	}
}