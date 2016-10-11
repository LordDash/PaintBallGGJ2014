using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerUIPanel : MonoBehaviour {
	#region Editor fields
	public int playerNumber;

	public UILabel hpLabel;
	public UILabel ammoLabel;
	#endregion

	#region Fields
	PlayerController player;
	#endregion

	#region Initilization
	// Use this for initialization
	void Start () {
		if(!PlayerManager.Instance.JoinedPlayers.Contains(playerNumber))
			this.gameObject.SetActive(false);
		else
			player = Game.game.players.Where(p => p.PlayerNumber == playerNumber).FirstOrDefault();
	}
	#endregion
	
	// Update is called once per frame
	void Update () {
		hpLabel.text = ""+(int)(player.HP * 100);
		ammoLabel.text = ""+player.Ammo;
	}
}
