using UnityEngine;
using System.Collections;

public class ShotgunPickup : Pickup {
	
	protected override void Start()
	{
		base.Start();
		GetComponent<Renderer>().material.color = Color.clear;
	}
	
	protected override void PickUp (PlayerController player)
	{
		//Set the player invisible
		player.addShotgun();
		base.PickUp (player);
	}
}
