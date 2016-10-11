using UnityEngine;
using System.Collections;

public class BigBulletPickup : Pickup {
	
	protected override void Start()
	{
		base.Start();
		GetComponent<Renderer>().material.color = Color.blue;
	}
	
	protected override void PickUp (PlayerController player)
	{
		player.setBigBullets();
		
		base.PickUp (player);
	}
}
