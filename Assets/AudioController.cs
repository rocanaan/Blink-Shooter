using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	public AudioClip soundtrack1;
	public AudioClip soundtrack2;
	public AudioClip soundtrack3;

	private AudioSource source;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		source.clip = soundtrack1;
		source.Play();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void switchByDifficulty(int difficulty){
		if (difficulty == 1) {
//			source.Stop ();
//			source.clip = soundtrack2;
//			source.Play ();
		}
		if (difficulty == 2) {
			source.Stop ();
			source.clip = soundtrack3;
			source.Play ();
		}
	}
}
