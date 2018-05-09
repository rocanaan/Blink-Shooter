using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {


	private bool paused;

	private GameObject audioSource;
	private AudioController soundtrackController;

	// Use this for initialization
	void Start () {
		paused = false;

		audioSource = GameObject.FindGameObjectWithTag ("GlobalAudioSource");
		soundtrackController = audioSource.GetComponent<AudioController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Pause"))
        {
            if(!paused)
            {
                Time.timeScale = 0.0F;
				paused = true;
				soundtrackController.GetComponent<AudioSource> ().Pause ();
            }else { 
				Time.timeScale = 1.0F;
				paused = false;
				soundtrackController.GetComponent<AudioSource> ().UnPause ();
			}
        }
	}

	public bool isPaused(){
		return paused;
	}

}
