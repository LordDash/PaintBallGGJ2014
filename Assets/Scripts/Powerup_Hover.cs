using UnityEngine;
using System.Collections;

public class Powerup_Hover : MonoBehaviour {

	Vector3 startpos;
	public float delta = 0.5f, speed = 2f;

	void Start()
	{
		startpos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 curPos = startpos + Vector3.up * (delta * Mathf.Sin (speed * Time.time));
		transform.localPosition = curPos;
	}
}
