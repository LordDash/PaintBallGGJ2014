using UnityEngine;
using System.Collections;

public class fastFirePickup : Pickup {
	
	protected override void Start()
	{
		base.Start();
		GetComponent<Renderer>().material.color = Color.black;
	}
	
	protected override void PickUp (PlayerController player)
	{
		player.changeFireRate();
		
		base.PickUp (player);
	}
}