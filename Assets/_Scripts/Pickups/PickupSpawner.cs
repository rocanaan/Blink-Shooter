using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {

	public float firstSpawnTime;
	public float spawnInterval;

	public float max_x;
	public float max_y;

	public GameObject[] pickups;

	private float nextSpawn;

	private GameController gc;

	// Use this for initialization
	void Start () {
		nextSpawn = firstSpawnTime;
		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad > nextSpawn && !GameController.IsGameOver()) {
			GameObject nextPickup = getNextPickup ();
			Vector2 randomVector = gc.GetRespawnPosition ();
			Vector3 spawnPosition =  new Vector3 (randomVector.x, randomVector.y, nextPickup.transform.position.z);
			Instantiate (nextPickup, spawnPosition, Quaternion.identity);

			nextSpawn += spawnInterval;
		}
	}

	// Returns the GameObject that will be the next spawn. Right now simply returns the first (and only) object in the array
	private GameObject getNextPickup (){
		return pickups [0];
	}
}
