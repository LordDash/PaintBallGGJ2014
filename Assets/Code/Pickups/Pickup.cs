using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Pickup : MonoBehaviour {

	#region Initilization
	protected virtual void Start()
	{
		GetComponent<Collider>().isTrigger = true;
	}
	#endregion

	#region Methods
	protected virtual void PickUp(PlayerController player)
	{
		Destroy(this.gameObject);
	}

	void OnTriggerEnter(Collider other)
	{

		PlayerController player = other.GetComponent<PlayerController>();
		if(player != null )
		{
			foreach( PlayerController playerController in FindObjectsOfType<PlayerController>() )
			{
				if( playerController.playerColor == player.playerColor && playerController.HP > 0 )
				{
					PickUp( playerController );
				}
			}
		}
	}
	#endregion
}
