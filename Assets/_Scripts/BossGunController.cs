using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGunController : MonoBehaviour {

	public float fireInterval;
	public float firstShotFixedDelay;
	public float firstShotRandomDelay;
	private float nextFire;

	private SoundEffectsController globalSFXController;

	private FireShot fs;

	// Use this for initialization
	void Start () {

		nextFire = Time.time + firstShotFixedDelay + Random.Range(0,firstShotRandomDelay);

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
