﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalShieldSpawner : MonoBehaviour {

	public float offsetAsRadiusRatio;
	public float angularSpeed;

	public GameObject shieldPrefab;
	private GameObject shield;


	public GameObject[] players;
	private GameObject targetPlayer;

	public float shieldSize;



	private bool statusActive;

	// Use this for initialization
	void Start () {
		statusActive = false;
	
	}

	// Update is called once per frame
	void Update () {
		if (statusActive) {
			getClosestTarget ();
			Vector3 targetDirection = targetPlayer.transform.position - transform.position;
			Vector3 newDirection = Vector3.RotateTowards (transform.right,targetDirection, Mathf.Deg2Rad*angularSpeed,0.0f);
			transform.right = newDirection;

		}
	}

	public void setStatus(bool status){
		statusActive = status;
		if (statusActive) {
//				float angle = 2*Mathf.PI * i / numberShields;
//				Vector3 direction = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0);
//				print (direction);
			Vector3 shieldPosition = transform.TransformPoint(Vector3.right*offsetAsRadiusRatio/2.0f); 
			//Vector3 shieldPosition = transform.TransformPoint (direction * offsetAsRadiusRatio / 2.0f);
			shield = Instantiate (shieldPrefab, shieldPosition, Quaternion.identity );
			getClosestTarget();
			shield.transform.localScale *= shieldSize;//transform.lossyScale.x;
			shield.transform.parent = transform;
//			Vector3 direction = (targetPlayer.transform.position - transform.position).normalized;
			//transform.rotation = Quaternion.LookRotation (Vector3.right, targetPlayer.transform.position - transform.position);
			transform.right = targetPlayer.transform.position - transform.position;
//				shieldList.Add (shield);
			shield.GetComponent<Renderer> ().material = GetComponentInParent<BossController> ().getCurrentMaterial();

		}
		if (!statusActive) {
			print ("Attempting to destroy directional shield");
			Destroy (shield);
			transform.rotation = Quaternion.identity;
		}
	}

	private void getClosestTarget(){
		targetPlayer = players[0];
		foreach (GameObject player in players) {
			if (Vector3.Distance(transform.position, player.transform.position) < Vector3.Distance(transform.position, targetPlayer.transform.position)){
				targetPlayer = player;
			}
		}
	}


}