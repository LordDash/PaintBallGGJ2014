using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GridCreator.collisionLayer playerColor = GridCreator.collisionLayer.BLUE;
	public ParticleSystem paintPrefab = null;
	public GameObject paintBulletPrefab = null;
	public float fireRate = 0.3f;

	private bool canFire = true;

	// Use this for initialization
	void Start () {
		gameObject.layer = (int)GridCreator.collisionLayer.BLUE;//(int)playerColor;
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetMouseButton( 1 ) )
		{
			// Fire the particles.
			ParticleSystem sprayComponent = (ParticleSystem)Instantiate( paintPrefab, transform.position, transform.rotation );
			SprayParticle sprayParticle = sprayComponent.GetComponent<SprayParticle>();
			sprayParticle.setParticleColor( playerColor );
		}
		else if( Input.GetMouseButton( 0 ) )
		{
			if( canFire )
			{
				// Fire the paint bullet
				Vector3 paintPosition = transform.position;
				paintPosition += ( transform.forward * 0.4f );
				paintPosition += ( transform.up * 0.1f );
				GameObject paintBullet = (GameObject)Instantiate( paintBulletPrefab, paintPosition, transform.rotation );
				paintBullet.GetComponent<Rigidbody>().AddForce( transform.forward * 1000.0f );

				canFire = false;
				Invoke( "reload", fireRate );
			}
		}
	}

	void reload() 
	{
		canFire = true;
	}
}
