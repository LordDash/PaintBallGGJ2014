using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	public float magnitude = 50f;
	public float time = .5f;

	float timeToShakeLeft;
	float totalTimeToShakeLeft;
	Transform cachedTransform;
	Vector3 originalPosition;
	float magnitudeToShake;
	
	// Use this for initialization
	void Start () {
		cachedTransform = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(timeToShakeLeft > 0)
		{
			cachedTransform.position = originalPosition + (Time.deltaTime * magnitudeToShake * (timeToShakeLeft/totalTimeToShakeLeft) * (Vector3.Slerp(-cachedTransform.right,cachedTransform.right, Random.value) * magnitude + Vector3.Slerp(-cachedTransform.forward,cachedTransform.forward, Random.value)));

			timeToShakeLeft -= Time.deltaTime;

			if(timeToShakeLeft <= 0)
			{
				timeToShakeLeft = 0;
				cachedTransform.position = originalPosition;
			}

		}
	}

//	void OnGUI()
//	{
//		if(GUI.Button(new Rect(0,0,200,50), "Shake")) {
//			ShakeCamera(magnitude, time);
//		}
//	}

	public void ShakeCamera (float magnitude, float time)
	{
		originalPosition = cachedTransform.position;
		timeToShakeLeft = time;
		totalTimeToShakeLeft = time;
		magnitudeToShake = magnitude;
	}
}
