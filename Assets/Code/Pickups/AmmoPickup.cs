using UnityEngine;
using System.Collections;

public class AmmoPickup : Pickup {
	private static readonly int ammoGain = 100;

	protected override void Start()
	{
		base.Start();
		GetComponent<Renderer>().material.color = Color.green;
	}

	protected override void PickUp (PlayerController player)
	{
		player.Ammo += ammoGain;

		base.PickUp (player);
	}
}
