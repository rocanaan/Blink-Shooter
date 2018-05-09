using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {


	private bool paused;
    private float lastTimeScale;
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
            Pause();
        }
	}

    public void Pause()
    {
        if (!transform.GetComponent<BossBattleGameController>().IsGameOver())
        {
            if (!paused)
            {
                lastTimeScale = Time.timeScale;
                Time.timeScale = 0f;
                paused = true;
                soundtrackController.GetComponent<AudioSource>().Pause();
            }
            else
            {
                Time.timeScale = lastTimeScale;
                paused = false;
                soundtrackController.GetComponent<AudioSource>().UnPause();
            }
        }        
    }

	public bool IsPaused(){
		return paused;
	}

}
