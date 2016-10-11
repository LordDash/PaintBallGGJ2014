using UnityEngine;
using System.Collections;

public class GridObject : MonoBehaviour {

	public float timeToRevert = 5.0f;
	private float timeLeftToRevert = 0.0f;
	private bool isHit = false;

	private static readonly float invincibleTime = 10.0f;
	
	public GameObject decal { get;set; }
	public GameObject[] decalPrefabs;

	public bool Invincible{
		get{
			return _invincible;
		}

		set{
			_invincible = value;

			if( _invincible )
			{
				Invoke( "stopInvincible", invincibleTime );
			}
		}
	}

	private bool _invincible = false;

	// Use this for initialization
	void Start () {
		gameObject.layer = (int)GridCreator.collisionLayer.NEUTRAL;
	}
	
	// Update is called once per frame
	void Update () {
//		if( isHit )
//		{
//			timeLeftToRevert += Time.deltaTime;
//			
//			if( timeLeftToRevert >= timeToRevert )
//			{
//				gameObject.layer = (int)GridCreator.collisionLayer.NEUTRAL;
//				decal.renderer.material.color = Color.gray;
//				isHit = false;
//			}
//		}
	}

	void OnParticleCollision( GameObject other )
	{
		SprayParticle sprayParticle = other.GetComponent<SprayParticle>();
		if( sprayParticle != other )
		{
			hitByBullet( sprayParticle.getParticleColor() );
		}
	}

	public void hitByBullet( GridCreator.collisionLayer bulletColor )
	{
		if( Invincible == false )
		{


			if((int)bulletColor != gameObject.layer && decal != null)
			{
				Destroy(decal);
			}

			if((int)bulletColor != gameObject.layer)
			{
				GameObject chosenPrefab = decalPrefabs[Random.Range(0, decalPrefabs.Length)];
				decal = Instantiate(chosenPrefab, transform.position + (Vector3.one * .01f), chosenPrefab.transform.rotation) as GameObject;
				decal.transform.parent = this.transform;
				decal.transform.Rotate(Vector3.up, Random.Range(0,360), Space.World);
			}

			gameObject.layer = (int)bulletColor;

			if( bulletColor == GridCreator.collisionLayer.BLUE )
			{
				decal.GetComponent<Renderer>().material.color = Color.blue;
			}
			else if( bulletColor == GridCreator.collisionLayer.RED )
			{
				decal.GetComponent<Renderer>().material.color = Color.red;
			}
			
			timeLeftToRevert = 0.0f;
			isHit = true;
		}
	}

	void stopInvincible()
	{
		Invincible = false;
	}
}
