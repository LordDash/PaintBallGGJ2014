using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class PlayerManager : MonoBehaviour {
	#region Fields
	private List<int> joinedPlayers;
	#endregion

	#region Properties
	public static PlayerManager Instance { get; private set; }

	public ReadOnlyCollection<int> JoinedPlayers { 
		get {
			return new ReadOnlyCollection<int>(joinedPlayers);
		}
	}
	#endregion

	#region Events
	public delegate void PlayerJoinedHandler(int playerNumber);
	public event PlayerJoinedHandler PlayerJoined;
	#endregion

	#region Initilization
	void Awake () {
		if(Instance == null)
			Reset();
		else
			Destroy(this.gameObject);
	}
	#endregion

	#region Methods
	// Update is called once per frame
	void Update () {
		for(int i = 1; i < 5; i++)
		{
			if(!PlayerIsJoined(i) && Input.GetAxis("Player"+i+"_Fire1") == 1) {
				joinedPlayers.Add(i);

				if(PlayerJoined != null)
					PlayerJoined(i);
			}
		}
	}

	public bool PlayerIsJoined(int playerNumber)
	{
		return joinedPlayers.Contains(playerNumber);
	}

	public void Reset()
	{
		joinedPlayers = new List<int>();
		
		Instance = this;
	}
	#endregion
}
