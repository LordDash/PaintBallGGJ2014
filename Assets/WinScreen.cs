using UnityEngine;
using System.Collections;

public class WinScreen : MonoBehaviour {
	public UISprite winscreenBlue;
	public UISprite winscreenYellow;

	// Use this for initialization
	void Start () {
		Game.game.GameOver += HandleGameOver;

		winscreenBlue.gameObject.SetActive(false);
		winscreenYellow.gameObject.SetActive(false);
	}

	void HandleGameOver (GridCreator.collisionLayer winner)
	{
		switch(winner) {
		case GridCreator.collisionLayer.BLUE:
			winscreenBlue.gameObject.SetActive(false);
			break;
		case GridCreator.collisionLayer.RED:
			winscreenYellow.gameObject.SetActive(false);
			break;
		}
	}
}
