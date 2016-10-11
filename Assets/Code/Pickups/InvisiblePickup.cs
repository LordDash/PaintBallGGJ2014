using UnityEngine;
using System.Collections;

public class InvisiblePickup : Pickup {
	
	protected override void Start()
	{
		base.Start();
		GetComponent<Renderer>().material.color = Color.magenta;
	}
	
	protected override void PickUp (PlayerController player)
	{
		//Set the player invisible
		player.Invisible = true;
		base.PickUp (player);
	}
}
