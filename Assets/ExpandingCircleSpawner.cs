using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingCircleSpawner : MonoBehaviour {

	public GameObject expandingAttackPrefab;
	public int numRepetitions;
	public float delay;

	private int currentInstance;
	private float nextFireTime;


	private SoundEffectsController sfx;

	public void Update(){
		if (Time.time > nextFireTime && currentInstance < numRepetitions) {
			FireCircle ();
		}
	}

	public void Start(){
		currentInstance = 999;
		nextFireTime = Time.time + delay;


		sfx = GameObject.FindGameObjectWithTag ("GlobalAudioSource").GetComponent<SoundEffectsController> ();
	}

	public void setStatus(bool active){
		if (active) {
			currentInstance = 0;
			FireCircle ();
		}
	}

	private void FireCircle(){
		currentInstance++;
		GameObject attack = Instantiate (expandingAttackPrefab, transform);
		if (currentInstance < numRepetitions) {
			nextFireTime = Time.time + delay;
		}
		sfx.PlayClip ("ExpandingCircle");
	}

	public float getDuration(){
		return (numRepetitions - 1) * delay;
	}
		
}
