using UnityEngine;
using System.Collections;

public class StartUIPanel : MonoBehaviour {
	#region Fields
	private bool ready;
	#endregion

	void Start() {
		PlayerManager.Instance.PlayerJoined += HandlePlayerJoined;
		ready = (PlayerManager.Instance.JoinedPlayers.Count % 2 == 0 && PlayerManager.Instance.JoinedPlayers.Count != 0);

		for(int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetActive(false);
		}
	}

	void HandlePlayerJoined (int playerNumber)
	{
		if(PlayerManager.Instance.JoinedPlayers.Count == 1) { // % 2 == 0 && PlayerManager.Instance.JoinedPlayers.Count != 0) {
			ready = true;

			for(int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.SetActive(true);
			}
		}
	}

	void Update()
	{
		if(ready && Input.GetButtonDown("StartButton"))
		{
			DontDestroyOnLoad( this );
			Application.LoadLevel("game");
		}
	}
}
