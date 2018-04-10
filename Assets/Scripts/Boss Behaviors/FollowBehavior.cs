using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehavior : MonoBehaviour {

	public GameObject[] players;
	public float speed;

	private bool statusActive;
	private Vector3 target;

	// Use this for initialization
	void Start () {
		statusActive = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (statusActive) {
			target = acquireTarget ();
			Vector3 offset = (target - transform.position);
			Vector3 direction = offset.normalized;
			GetComponent<Rigidbody2D> ().velocity = direction * speed;
		}
		
	}

	public void setStatus(bool status){
		statusActive = status;
	}

	private Vector3 acquireTarget(){
		float minDistance= 99999.0f;
		Vector3 target = new Vector3 (0, 0, 0);
		for (int i = 0; i < players.Length; i++){
			float distance = Vector3.Distance (transform.position, players [i].transform.position);
			if (distance < minDistance) {
				minDistance = distance;
				target = players [i].transform.position;
			}
		}

		return target;
	}
}
