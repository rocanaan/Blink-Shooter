using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGunController : MonoBehaviour {

	public float fireInterval;
	public float firstShotDelay;
	private float nextFire;
	public bool randomFirstDelay;

	private SoundEffectsController globalSFXController;

	private FireShot fs;

	// Use this for initialization
	void Start () {
		if (randomFirstDelay) {
			nextFire = Time.time + Random.Range (0, firstShotDelay);
		} else {
			nextFire = Time.time + firstShotDelay;
		}
		fs = GetComponent<FireShot> ();

		globalSFXController = GameObject.FindGameObjectWithTag ("GlobalAudioSource").GetComponent<SoundEffectsController> ();
	}

	// Update is called once per frame
	void Update () {
		if (Time.time >= nextFire) {
			fs.fireShot ();
			globalSFXController.PlayClip ("WallGunFire");
			nextFire += fireInterval;
		}
	}
		
}
