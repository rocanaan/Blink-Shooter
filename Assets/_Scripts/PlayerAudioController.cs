using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour {

	public AudioClip hitSound;
	public AudioClip deathSound;
	public AudioClip fireSound;

	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponentInParent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlaySound(string s){
		if (s == "Hit"){
//			audioSource.clip = hitSound;
//			audioSource.Play();
			audioSource.PlayOneShot(hitSound);
		}
		if (s == "Death") {
//			audioSource.clip = deathSound;
//			audioSource.Play();
			audioSource.PlayOneShot(deathSound);
		}
		if (s == "Fire") {
//			audioSource.clip = fireSound;
//			audioSource.Play ();
			audioSource.PlayOneShot(fireSound);
		} 
	}
}
