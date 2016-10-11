using UnityEngine;
using System.Collections;

public class SoundFXSpawner : MonoBehaviour {

	void Start () {
		if(GetComponent<AudioSource>() == null)
			this.gameObject.AddComponent<AudioSource>();
	}

	void SpawnSoundFX(AudioClip clip) {
		GetComponent<AudioSource>().PlayOneShot(clip);
	}
}
