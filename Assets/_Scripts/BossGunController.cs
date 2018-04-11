using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGunController : MonoBehaviour {

	public float fireInterval;
	public float firstShotDelay;
	private float nextFire;

	private FireShot fs;

	// Use this for initialization
	void Start () {
		nextFire = Time.time + firstShotDelay;
		fs = GetComponent<FireShot> ();
	}

	// Update is called once per frame
	void Update () {
		if (Time.time >= nextFire) {
			fs.fireShot ();
			nextFire += fireInterval;
		}
	}
		
}
