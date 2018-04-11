﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargettedFireBehavior : MonoBehaviour {

	public GameObject[] players;
	public GameObject shot;

	public float fireInterval;
	public float firstShotDelay;
	private float nextFire;

	private bool statusActive;

	public float shotSpeed;
	private int targetPlayerIndex;

	public bool activateOnStartup;

	// Use this for initialization
	void Start () {
		setStatus (activateOnStartup); //activates the behavior on startup if that flag is set to true
		
	}
	
	// Update is called once per frame
	void Update () {
		if (statusActive && Time.time > nextFire) {
			if (!players [targetPlayerIndex].GetComponent<PlayerController> ().isDead) {
				Vector3 target = players [targetPlayerIndex].transform.position;
				Vector3 direction = target - transform.position;
				direction.Normalize ();
				GameObject newShot = Instantiate (shot, new Vector3 (transform.position.x, transform.position.y, transform.position.z - 1), Quaternion.identity);
				newShot.GetComponent<Rigidbody2D> ().velocity = direction * shotSpeed;
				newShot.GetComponent<Renderer> ().material = GetComponentInParent<BossController> ().getCurrentMaterial ();
				newShot.GetComponent<ShotAttributes> ().setTeamID (2);

				nextFire = Time.time + fireInterval;
			}
			targetPlayerIndex = (targetPlayerIndex + 1) % players.Length;
		}
	}

	public void setStatus(bool status){
		statusActive = status;
		if (status) {
			nextFire = Time.time + firstShotDelay;
			targetPlayerIndex = 0;
		}
	}
}