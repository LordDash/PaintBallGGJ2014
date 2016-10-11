using UnityEngine;
using System.Collections;

public class speedPickup : Pickup {
	
	protected override void Start()
	{
		base.Start();
		GetComponent<Renderer>().material.color = Color.gray;
	}
	
	protected override void PickUp (PlayerController player)
	{
		player.changeSpeed();
		
		base.PickUp (player);
	}
}
