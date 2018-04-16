using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSplittingAttack : BossGenericBehavior {

	private GameObject[] players;
	private GameController gameController;

	public GameObject laserSpawnerPrefab;

	public float angularSpeed;
	public float preparationTime;
	public float maxArcTraveled;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		players = gameController.getAllPlayers ();
		setActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void setActive(bool active){
		if (active) {
			Vector3 playersMidpoint = getPlayersMidpoint();
			if (playersMidpoint == transform.position) {
				playersMidpoint = Vector3.right;
			}
			GameObject laserObj1 = Instantiate (laserSpawnerPrefab, transform.position, Quaternion.identity);
			LaserSpawnerController laser1 = laserObj1.GetComponent<LaserSpawnerController> ();
			laser1.angularSpeed = angularSpeed;
			laser1.preparationTime = preparationTime;
			laser1.angleRotated = maxArcTraveled;
			laser1.target = playersMidpoint;
			laser1.transform.parent = transform;

			GameObject laserObj2 = Instantiate (laserSpawnerPrefab, transform.position, Quaternion.identity);
			LaserSpawnerController laser2 = laserObj2.GetComponent<LaserSpawnerController> ();
			laser2.angularSpeed = -angularSpeed;
			laser2.preparationTime = preparationTime;
			laser2.angleRotated = maxArcTraveled;
			laser2.target = playersMidpoint;
			laser2.transform.parent = transform;
		}
	}

	private Vector3 getPlayersMidpoint(){
		Vector3 midpoint = Vector3.zero;
		foreach (GameObject player in players) {
			midpoint += player.transform.position;
		}
		midpoint = midpoint * (1.0f / players.Length);
		return midpoint;
	}
}
