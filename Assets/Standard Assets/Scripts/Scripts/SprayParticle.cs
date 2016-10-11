using UnityEngine;
using System.Collections;

public class SprayParticle : MonoBehaviour {
	
	public GridCreator.collisionLayer particleColor = GridCreator.collisionLayer.BLUE;
	
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void setParticleColor( GridCreator.collisionLayer paintColor )
	{
		particleColor = paintColor;
	}

	public GridCreator.collisionLayer getParticleColor() 
	{
		return particleColor;
	}
}
