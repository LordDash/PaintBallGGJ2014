using UnityEngine;
using System.Collections;
using System.Linq;
using XInputDotNetPure;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	private static readonly float speed = 5f;
	private static readonly float fireForce = 2000f;
	private static readonly float paintForce = 100f;
	private static readonly float fireRate = .25f;
	private static readonly float fastFireRate = 0.12f;
	private static readonly float paintRate = 0.07f;
	private static readonly float explodeRate = 1.5f;
	private static readonly float pickupTimer = 10.0f;

	private static readonly float bulletHitPower = 1f/3f;
	private static readonly int bulletAmmoCost = 1;
	private static readonly int sprayAmmoCost = 5;
	private static readonly int explosionAmmoCost = 0;

	#region Editor fields
	public Bullet bulletPrefab;
	public Transform bulletSpawnPoint;
	public GridCreator.collisionLayer playerColor {get; set;}
	public ParticleSystem explosionParticle;
	public GameObject[] objectsToHide;
	#endregion

	#region Properties
	public int PlayerNumber { get; set; }

	private string PlayerIdentifier {
		get {
			return "Player"+PlayerNumber+"_";
		}
	}

	public float HP { 
		get {
			return hp;
		} 
		set {
			hp = Mathf.Max(0, value);
			hp = Mathf.Min (1, hp);

			animator.SetFloat("HP", HP);

			if(HP == 0 && PlayerDead != null)
				PlayerDead(this);

			this.GetComponent<Collider>().enabled = (HP != 0);
		}
	}
	
	public int Ammo {
		get;
		set;
	}

	public bool Invisible {
		get{
			return _invisible;
		}
		set{
			_invisible = value;

			Renderer[] renderers = GetComponentsInChildren<Renderer>();
			foreach(Renderer r in renderers)
				r.enabled = !value;

			if( _invisible )
			{
				Invoke( "stopInvisible", pickupTimer );
			}
		}
	}

	public bool Invincible {
		get{
			return _invincible;
		}

		set{
			_invincible = value;

			if( _invincible )
			{
				Invoke ( "stopInvincible", pickupTimer );
			}
		}
	}
	#endregion

	#region Fields
	private Transform cachedTransform;
	private Vector3 oldForward;

	public Animator animator { get { return GetComponent<Animator>(); }}

	float lastTimeFired;
	float hp;
	bool _invincible = false;
	bool _invisible = false;
	bool fastFire = false;
	bool fastSpeed = false;
	bool hasBigBullets = false;
	bool hasShotgun = false;

	Vector3 velocity { 
		get {
			return (cachedTransform.position - lastKnownPosition)/Time.fixedDeltaTime;
		}
	}
	#endregion 

	#region Events
	public delegate void PlayerDeadHandler(PlayerController player);
	public event PlayerDeadHandler PlayerDead;
	#endregion

	#region Initialization
	void Awake() {
		cachedTransform = this.transform;
		lastKnownPosition = cachedTransform.position;

		PlayerDead += HandlePlayerDead;
	}

	IEnumerator Vibrate ()
	{
		GamePad.SetVibration((PlayerIndex)( PlayerNumber-1 ), 1.0f, 1.0f );
		yield return new WaitForSeconds( 0.2f );
		GamePad.SetVibration((PlayerIndex)( PlayerNumber-1 ), 0.0f, 0.0f );
	}

	void HandlePlayerDead (PlayerController player)
	{
		StartCoroutine(Vibrate() );
	}

	void Start()
	{
		HP = 1.0f;

		gameObject.layer = (int)playerColor;

		explosionParticle.gameObject.SetActive( false );
		SprayParticle sprayParticle = explosionParticle.GetComponent<SprayParticle>();
		if( sprayParticle != null )
		{
			sprayParticle.setParticleColor( playerColor );
		}


		//TODO change accent color of player
//		if(playerColor == GridCreator.collisionLayer.BLUE)
//			gameObject.renderer.material.color = Color.blue;
//		else
//			gameObject.renderer.material.color = Color.red;

		lastTimeFired = Time.time;
		Invincible = false;
		Invisible = false;
		fastFire = false;
		fastSpeed = false;
		hasBigBullets = false;
		hasShotgun = false;
	}
	#endregion

	public AudioClip[] shotSounds;
	public AudioClip[] emptySounds;
	public AudioClip[] suicideSounds;
	
	#region Methods
	// Update is called once per frame
	void Update () {
		//UpdateMovement ();

		float realFireRate = fastFire ? fastFireRate : fireRate;
		if(Input.GetAxis(PlayerIdentifier+"Fire1") == 1 && Time.time-lastTimeFired >= realFireRate)
		{
			if(Ammo >= bulletAmmoCost)
			{
				if(animator.GetFloat("Speed") == 0)
					animator.SetTrigger("Shoot");

			Bullet b = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity) as Bullet;
			if( hasBigBullets )
			{
				b.transform.localScale = new Vector3( 1.3f, 1.3f, 1.3f );
			}

			if( hasShotgun )
			{
				addShotgunBullets();
			}

				//Vector3 fireDirection = cachedTransform.forward;

				foreach(PlayerController opponent in Game.game.players.Where(p => p.playerColor != this.playerColor)) {
					if(Vector3.Angle(opponent.cachedTransform.position-cachedTransform.position, cachedTransform.forward) < 15)
						transform.LookAt(opponent.cachedTransform.position);
				}

				b.GetComponent<Rigidbody>().AddForce(cachedTransform.forward * fireForce);
				b.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
				b.bulletColor = playerColor;
				b.SingleShot = true;
				Ammo -= bulletAmmoCost;

				lastTimeFired = Time.time;
				GetComponent<AudioSource>().PlayOneShot(shotSounds[Random.Range(0, shotSounds.Length)]);
			} else {
				GetComponent<AudioSource>().PlayOneShot(emptySounds[Random.Range(0, emptySounds.Length)]);
				lastTimeFired = Time.time;
			}
		}

		if(Input.GetAxis(PlayerIdentifier+"Fire2") == 1 && Time.time - lastTimeFired >= paintRate )
		{
			// Fire the particles.
	
			if( Ammo >= sprayAmmoCost )
			{
				if( cachedTransform.forward != oldForward )
				{
					for( int i = 0; i < 500.0f * Time.deltaTime; i++ )
					{
						Bullet b = Instantiate( bulletPrefab, bulletSpawnPoint.position, Quaternion.identity ) as Bullet;
						b.GetComponent<Rigidbody>().AddForce(Vector3.Lerp( oldForward, cachedTransform.forward, Random.value ) * paintForce);
						b.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
						b.bulletColor = playerColor;
					}
				}
				else
				{
					for( int i = 0; i < 500.0f * Time.deltaTime; i++ )
					{
						Bullet b = Instantiate( bulletPrefab, bulletSpawnPoint.position, Quaternion.identity ) as Bullet;

						float directionLerpValue = Random.value;
						Vector3 forceDirection = Vector3.Lerp( -cachedTransform.right + cachedTransform.forward, cachedTransform.forward + cachedTransform.right,  directionLerpValue);

						b.GetComponent<Rigidbody>().AddForce(forceDirection  * (paintForce + (Mathf.Lerp(0, 1, 1f-Mathf.Abs((directionLerpValue-0.5f) *2f)) * Mathf.Lerp(-1,1, Random.value) * paintForce)));
						b.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
						b.bulletColor = playerColor;
					}
				}

				Ammo -= sprayAmmoCost;

				if(!GetComponent<AudioSource>().isPlaying)
					GetComponent<AudioSource>().Play();

				lastTimeFired = Time.time;
			} else {
				GetComponent<AudioSource>().PlayOneShot(emptySounds[Random.Range(0, emptySounds.Length)]);
				lastTimeFired = Time.time;
			}
		}

		if(Input.GetAxis(PlayerIdentifier+"Fire2") == 0 && GetComponent<AudioSource>().isPlaying) {
			GetComponent<AudioSource>().Stop();
		}

		if(Input.GetButtonDown(PlayerIdentifier+"Suicide")) {
			explode();
		}
	}

	void addShotgunBullets()
	{
		Bullet bShotgun1 = Instantiate( bulletPrefab, bulletSpawnPoint.position + cachedTransform.right/2, Quaternion.identity ) as Bullet;
		Bullet bShotgun2 = Instantiate( bulletPrefab, bulletSpawnPoint.position - cachedTransform.right/2, Quaternion.identity ) as Bullet;
		
		if( hasBigBullets )
		{
			bShotgun1.transform.localScale = new Vector3( 1.3f, 1.3f, 1.3f );
			bShotgun2.transform.localScale = new Vector3( 1.3f, 1.3f, 1.3f );
		}
		
		bShotgun1.GetComponent<Rigidbody>().AddForce( (cachedTransform.forward - cachedTransform.right/2) * fireForce );
		bShotgun2.GetComponent<Rigidbody>().AddForce( (cachedTransform.forward + cachedTransform.right/2) * fireForce );
		bShotgun1.GetComponent<Rigidbody>().AddForce( velocity, ForceMode.VelocityChange );
		bShotgun2.GetComponent<Rigidbody>().AddForce( velocity, ForceMode.VelocityChange );

		bShotgun1.bulletColor = playerColor;
		bShotgun2.bulletColor = playerColor;
	}
	
	void explode()
	{
		if( Time.time - lastTimeFired > explodeRate && Ammo >= explosionAmmoCost )
		{
			explosionParticle.gameObject.SetActive( true );
			explosionParticle.Emit( 500 );

			GetComponent<AudioSource>().PlayOneShot(suicideSounds[Random.Range(0, suicideSounds.Length)]);

			lastTimeFired = Time.time;

			HP = 0;
		}
	}

	void playerExploded()
	{
		PlayerDead( this );
		explosionParticle.gameObject.SetActive( false );
	}

	Vector3 lastKnownPosition;

	void FixedUpdate()
	{
		UpdateMovement ();
		
		Vector3 newPos = transform.position;
		
		if( lastKnownPosition != newPos )
		{
			LayerMask mask = 1 << ( playerColor == GridCreator.collisionLayer.BLUE ? (int)GridCreator.collisionLayer.RED : (int)GridCreator.collisionLayer.BLUE );
			
			RaycastHit hit;
			if(Physics.Raycast(lastKnownPosition, newPos - lastKnownPosition, out hit, Vector3.Distance(newPos, lastKnownPosition), 
			                   mask ) )
			{
				Vector3 movedPos = GetComponent<Collider>().bounds.extents;
				movedPos.x *= hit.normal.x;
				movedPos.y *= hit.normal.y;
				movedPos.z *= hit.normal.z;
				GetComponent<Rigidbody>().MovePosition(hit.point);
			}
		}

		animator.SetFloat("Speed", Mathf.Abs((newPos.x - lastKnownPosition.x) / Time.deltaTime) + Mathf.Abs((newPos.z - lastKnownPosition.z) / Time.deltaTime));

		lastKnownPosition = cachedTransform.position;
		//velocity = rigidbody.velocity;
	}

	void OnParticleCollision( GameObject other )
	{
		SprayParticle sprayParticle = other.GetComponent<SprayParticle>();
		if( sprayParticle != null && sprayParticle.getParticleColor() != playerColor )
		{
			PlayerDead( this ); // Too many bullets, they will kill you automatically.
		}
	}

	void UpdateMovement ()
	{
		float realSpeed = fastSpeed ? speed * 2 : speed;

		Vector3 newPosition = cachedTransform.position + realSpeed * ((Vector3.forward * Input.GetAxis (PlayerIdentifier + "LeftThumb_Y_Axis")) + (Vector3.right * Input.GetAxis (PlayerIdentifier + "LeftThumb_X_Axis"))) * Time.deltaTime;

		int groundMask = 1 << LayerMask.NameToLayer("Ground");
		
		RaycastHit groundHit;
		Ray groundRay = new Ray(GetComponent<Collider>().bounds.center, Vector3.down);
		if(Physics.Raycast(groundRay, out groundHit, GetComponent<Collider>().bounds.extents.y * 2, groundMask))
		{
			newPosition.y = groundHit.point.y;
		}

		GetComponent<Rigidbody>().MovePosition (newPosition);

		Vector3 lookDirection = (Vector3.forward * Input.GetAxis (PlayerIdentifier + "RightThumb_Y_Axis")) + (Vector3.right * Input.GetAxis (PlayerIdentifier + "RightThumb_X_Axis"));

		oldForward = cachedTransform.forward;

		if (lookDirection != Vector3.zero)
			GetComponent<Rigidbody>().MoveRotation (Quaternion.LookRotation (lookDirection));

		if(newPosition != lastKnownPosition && Vector3.Angle(cachedTransform.forward, newPosition - lastKnownPosition) >= 100)
		{
			animator.SetBool("MovingBackwards", true);
		}
		else if(newPosition != lastKnownPosition && Vector3.Angle(cachedTransform.forward, newPosition - lastKnownPosition) < 95)
		{
			animator.SetBool("MovingBackwards", false);
		}
	}

	public void BulletHit ()
	{
		if( _invincible == false )
		{
			HP -= bulletHitPower;
		}
	}

	public void stopInvincible()
	{
		_invincible = false;
	}

	public void stopInvisible()
	{
		Invisible = false;
	}

	public void changeFireRate()
	{
		fastFire = true;
	}

	public void changeSpeed()
	{
		fastSpeed = true;
		Invoke ( "resetSpeed", pickupTimer );
	}

	void resetSpeed()
	{
		fastSpeed = false;
	}

	public void setBigBullets()
	{
		hasBigBullets = true;
		Invoke ( "resetBulletSize", pickupTimer );
	}

	void resetBulletSize()
	{
		hasBigBullets = false;
	}

	public List<AudioClip> dirtFootsteps;
	public List<AudioClip> wetFootsteps;

	void FootStep()
	{
		bool wet = false;

		RaycastHit groundHit;
		Ray groundRay = new Ray(GetComponent<Collider>().bounds.center, Vector3.down);
		if(Physics.Raycast(groundRay, out groundHit, GetComponent<Collider>().bounds.extents.y * 2))
		{
			GridObject gridBox = groundHit.collider.GetComponent<GridObject>();

			if(gridBox != null) {
				wet = (gridBox.decal != null);
			}
		}

		AudioClip toPlay;

		if(wet) {
			toPlay = wetFootsteps[Random.Range(0, wetFootsteps.Count)];
		} else {
			toPlay = dirtFootsteps[Random.Range(0, dirtFootsteps.Count)];
		}

		if(GetComponent<AudioSource>() == null)
			this.gameObject.AddComponent<AudioSource>();

		GetComponent<AudioSource>().PlayOneShot(toPlay);
	}

	public void addShotgun()
	{
		Invoke ( "resetShotgun", pickupTimer );
		hasShotgun = true;
	}
	
	void resetShotgun()
	{
		hasShotgun = false;
	}
	
	public void reset()
	{
		foreach( GameObject obj in objectsToHide )
		{
			obj.GetComponent<Renderer>().enabled = true;
		}
	}
	#endregion
}
