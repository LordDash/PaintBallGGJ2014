using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	private static readonly float captureStrength = .1f;

	#region Fields
	Transform cachedTransform;
	private GridCreator.collisionLayer _bulletColor;
	Vector3 lastKnownPosition;

	public AudioClip[] impactSounds;

	public bool SingleShot { get;set; }
	#endregion

	public GridCreator.collisionLayer bulletColor{ 
		get{
			return _bulletColor;
		} 

		set{
			_bulletColor = value;

			if( bulletColor == GridCreator.collisionLayer.BLUE )
			{
				gameObject.GetComponent<Renderer>().material.color = new Color( 0.0f, 128f, 255f );
			}
			else if( bulletColor == GridCreator.collisionLayer.RED )
			{
				gameObject.GetComponent<Renderer>().material.color = new Color( 255f, 216f, 0f );
			}
		}
	}

	void Awake ()
	{
		cachedTransform = this.transform;
		gameObject.layer = 11; // Bullet

		lastKnownPosition = cachedTransform.position;
	}

	void FixedUpdate()
	{
		RaycastHit hit;
		if(Physics.Raycast(lastKnownPosition, cachedTransform.position-lastKnownPosition, out hit, Vector3.Distance(lastKnownPosition,cachedTransform.position)))
		{
			if( hit.collider.gameObject.layer != LayerMask.NameToLayer("Bullet"))
			{
				cachedTransform.position = hit.point;
			}
		}

		lastKnownPosition = cachedTransform.position;
	}

	void OnTriggerEnter(Collider other) {
		PlayerController player = other.gameObject.GetComponent<PlayerController>();
		if(player != null && player.playerColor != bulletColor)
			player.BulletHit();

		GridObject gridObject = other.gameObject.GetComponent<GridObject>();
		if( gridObject != null )
		{
			gridObject.hitByBullet( bulletColor );
		}

		CapturePoint capturePoint = other.gameObject.GetComponent<CapturePoint>();
		if(capturePoint != null)
		{
			capturePoint.Capture(captureStrength, bulletColor);
		}

		Bullet bullet = other.gameObject.GetComponent<Bullet>();
		if( bullet == null )
		{
			Destroy(this.gameObject);
		}
	}

	static MonoBehaviour coroutineHelper;

	void OnDestroy() {
		if(coroutineHelper == null)
		{
			GameObject go = new GameObject();
			go.name = "Bullet Sound Spawner";
			coroutineHelper = go.AddComponent<MonoBehaviour>();
		}

		if(SingleShot)
			coroutineHelper.StartCoroutine(PlaySplat(impactSounds[Random.Range(0, impactSounds.Length)], this.transform.position));
	}

	private static IEnumerator PlaySplat(AudioClip clip, Vector3 position)
	{
		GameObject go = new GameObject();
		go.name = "BulletSound: " + clip.name;
		go.transform.position = position;

		go.AddComponent<AudioSource>();
		go.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
		go.GetComponent<AudioSource>().maxDistance = 50;

		go.GetComponent<AudioSource>().clip = clip;
		go.GetComponent<AudioSource>().loop = false;
		go.GetComponent<AudioSource>().Play();

		while(go.GetComponent<AudioSource>().isPlaying)
		{
			yield return null;
		}

		Destroy(go);
	}
}
