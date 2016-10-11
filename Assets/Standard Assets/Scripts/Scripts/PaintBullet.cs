using UnityEngine;
using System.Collections;

public class PaintBullet : MonoBehaviour {

	public GridCreator.collisionLayer bulletColor = GridCreator.collisionLayer.BLUE;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setPlayerColor( GridCreator.collisionLayer playerColor )
	{
		bulletColor = playerColor;
	}
}
