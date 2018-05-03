using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehavior : MonoBehaviour {

	private GameObject[] players;
	private BossBattleGameController gameController;
	public float speed;
	public float tolerance;

	private bool statusActive;
	private Vector3 target;

	// Use this for initialization
	void Start () {
		statusActive = false;

		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<BossBattleGameController> ();
		players = gameController.GetAllPlayers ();
	}
	
	// Update is called once per frame
	void Update () {
		if (statusActive) {
			target = acquireTarget ();
			Vector3 offset = (target - transform.position);
			if (offset.magnitude > tolerance) {
				Vector3 direction = offset.normalized;
				GetComponent<Rigidbody2D> ().velocity = direction * speed;
			} else {
				GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
			}
		}
		
	}

	public void setStatus(bool status){
		statusActive = status;
	}

	private Vector3 acquireTarget(){
		float minDistance= 99999.0f;
		Vector3 target = new Vector3 (0, 0, 0);
		foreach (GameObject playerObj in players){
			if (!playerObj.GetComponent<PlayerController> ().isDead) {
				float distance = Vector3.Distance (transform.position, playerObj.transform.position);
				if (distance < minDistance) {
					minDistance = distance;
					target = playerObj.transform.position;
				}
			}
		}

		return target;
	}
}
