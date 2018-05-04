using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	public AudioClip soundtrack1;
	public AudioClip soundtrack2;
	public AudioClip soundtrack3;
	public AudioClip transitionSoundtrack;

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

	public void switchByStage(BossController.Stage stage){
		if (stage == BossController.Stage.Medium) {
			source.Stop ();
			source.clip = soundtrack2;
			source.Play ();
		}
		if (stage == BossController.Stage.Hard) {
			source.Stop ();
			source.clip = soundtrack3;
			source.Play ();
		}
		if (stage == BossController.Stage.MediumTransition || stage == BossController.Stage.HardTransition) {
			source.Stop ();
			source.clip = transitionSoundtrack;
			source.Play ();
		}
	}
}
