using UnityEngine;
using System.Collections;

public class CapturePoint : MonoBehaviour {
	#region Editor fields
	public GameObject redPylon;
	public GameObject bluePylon;
	#endregion

	#region Fields
	private static readonly float decaySpeed = 0.2f;

	float redCapturePercentage;
	float blueCapturePercentage;

	float timeLastCapture;
	#endregion
	
	#region Properties
	private bool Captured { get { return RedCapturePercentage == 1 || BlueCapturePercentage == 1; } }

	public bool Capturing { get { return Time.time-timeLastCapture < 1f; } }

	public float RedCapturePercentage {
		get { return redCapturePercentage; }
		set { 
			redCapturePercentage = Mathf.Max(0, value);
			redCapturePercentage = Mathf.Min(1, redCapturePercentage);

			if(redCapturePercentage == 1)
				Owner = GridCreator.collisionLayer.RED;
			else if(redCapturePercentage == 0)
				Owner = GridCreator.collisionLayer.NEUTRAL;
		}
	}

	public float BlueCapturePercentage {
		get { return blueCapturePercentage; }
		set { 
			blueCapturePercentage = Mathf.Max(0, value);
			blueCapturePercentage = Mathf.Min(1, blueCapturePercentage);

			if(blueCapturePercentage == 1)
				Owner = GridCreator.collisionLayer.BLUE;
			else if(blueCapturePercentage == 0)
				Owner = GridCreator.collisionLayer.NEUTRAL;
		}
	}

	public GridCreator.collisionLayer Owner { 
		get; private set; 
	}
	#endregion


	#region Initilization
	// Use this for initialization
	void Start () {
		Owner = GridCreator.collisionLayer.NEUTRAL;
	}
	#endregion
	
	// Update is called once per frame
	void Update () {
		if(!Captured && !Capturing)
		{
			Uncapture();
		}

		redPylon.transform.localPosition = Vector3.Slerp(Vector3.zero, Vector3.up * 1.4f, RedCapturePercentage);
		bluePylon.transform.localPosition = Vector3.Slerp(Vector3.zero, Vector3.up * 1.4f, BlueCapturePercentage);
	}

	public void Capture(float captureAmount, GridCreator.collisionLayer team)
	{
		if(CapturePercentage(OtherTeam(team)) > 0) {
			CaptureThePoint((-1f)*captureAmount, OtherTeam(team));
		} else {
			CaptureThePoint(captureAmount, team);
		}
	}

	public float CapturePercentage(GridCreator.collisionLayer team) {
		switch(team)
		{
		case GridCreator.collisionLayer.BLUE:
			return BlueCapturePercentage;
		case GridCreator.collisionLayer.RED:
			return RedCapturePercentage;
		default:
			return (RedCapturePercentage != 0)?1f-RedCapturePercentage:1f-BlueCapturePercentage;
		}
	}

	void CaptureThePoint(float captureAmount, GridCreator.collisionLayer team) {
		switch(team)
		{
		case GridCreator.collisionLayer.BLUE:
			BlueCapturePercentage += captureAmount;
			timeLastCapture = Time.time;
			break;
		case GridCreator.collisionLayer.RED:
			RedCapturePercentage += captureAmount;
			timeLastCapture = Time.time;
			break;
		}
	}

	void Uncapture ()
	{
		if(Owner == GridCreator.collisionLayer.NEUTRAL)
		{
			if(BlueCapturePercentage > 0) 
			{
				BlueCapturePercentage -= decaySpeed * Time.deltaTime;
			} else if(RedCapturePercentage > 0)
			{
				RedCapturePercentage -= decaySpeed * Time.deltaTime;
			}
		}
		else if(Owner == GridCreator.collisionLayer.RED)
		{
			if(BlueCapturePercentage > 0) 
			{
				BlueCapturePercentage -= decaySpeed * Time.deltaTime;
			} else if(RedCapturePercentage > 0)
			{
				RedCapturePercentage += decaySpeed * Time.deltaTime;
			}
		}
		else if(Owner == GridCreator.collisionLayer.BLUE)
		{
			if(BlueCapturePercentage > 0) 
			{
				BlueCapturePercentage += decaySpeed * Time.deltaTime;
			} else if(RedCapturePercentage > 0)
			{
				RedCapturePercentage -= decaySpeed * Time.deltaTime;
			}
		}
	}

	GridCreator.collisionLayer OtherTeam(GridCreator.collisionLayer team) {
		switch(team)
		{
		case GridCreator.collisionLayer.BLUE:
			return  GridCreator.collisionLayer.RED;
		case GridCreator.collisionLayer.RED:
			return  GridCreator.collisionLayer.BLUE;
		default:
			return  GridCreator.collisionLayer.NEUTRAL;
		}
	}

	void OnParticleCollision( GameObject other )
	{
		SprayParticle sprayParticle = other.GetComponent<SprayParticle>();
		if( sprayParticle != null )
		{
			// Instantly capture the points
			Capture( 1.0f, sprayParticle.getParticleColor() );
		}
	}
}
