using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawnerAudioController : MonoBehaviour {

	public AudioClip preparationClip;
	public AudioClip releaseClip;

	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	public void playPreparation (){
		audioSource.clip = preparationClip;
		audioSource.Play();
	}

	public void stopPreparation (){
		audioSource.Stop();
	}

	public void playRelease(){
		audioSource.PlayOneShot (releaseClip);
	}

}
