using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManagerUIPanel : MonoBehaviour {
	#region Editor fields
	public List<PlayerJoinUIPanel> panels;
	#endregion

	// Use this for initialization
	void Start () {
		PlayerManager.Instance.PlayerJoined += HandlePlayerJoined;

		foreach(PlayerJoinUIPanel panel in panels)
		{
			panel.joinedLabel.gameObject.SetActive(false);
		}
	}

	void HandlePlayerJoined (int playerNumber)
	{
		panels[PlayerManager.Instance.JoinedPlayers.Count-1].joinLabel.gameObject.SetActive(false);
		panels[PlayerManager.Instance.JoinedPlayers.Count-1].joinedLabel.gameObject.SetActive(true);
		panels[PlayerManager.Instance.JoinedPlayers.Count-1].joinedLabel.text = "Player "+playerNumber+"\njoined";
	}
}
