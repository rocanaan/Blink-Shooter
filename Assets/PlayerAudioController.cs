using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour {

	public AudioClip hitSound;
	public AudioClip deathSound;

	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlaySound(string s){
		if (s == "Hit"){
			audioSource.clip = hitSound;
			audioSource.Play();
		}
		if (s == "Death") {
			audioSource.clip = deathSound;
			audioSource.Play();
		}
	}
}
