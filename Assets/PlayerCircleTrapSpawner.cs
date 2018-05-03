using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCircleTrapSpawner : MonoBehaviour {

	public GameObject CircleTrapPrefab;

	private GameObject[] players;
	private BossBattleGameController gameController;
	public int repetitions;
	private int currentTrapCount;
	private int lastPlayerID;
	public float interval;
	private float nextTrapTime;
	private bool isActive;

	void Start(){
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<BossBattleGameController> ();
		players = gameController.GetAllPlayers ();
		lastPlayerID = -1;
		isActive = false;
		currentTrapCount = 0;
		nextTrapTime = 999999;
	}

	void Update(){
		if (isActive && Time.time >= nextTrapTime && currentTrapCount < repetitions) {
			int nextTargetID = (lastPlayerID + 1) % players.Length;
			if (!players [nextTargetID].GetComponent<PlayerController> ().isDead) {
				GameObject attack = Instantiate (CircleTrapPrefab, players[nextTargetID].transform);
			}
			lastPlayerID = nextTargetID;
			currentTrapCount++;
			nextTrapTime = Time.time + interval;
			
		}
		
	}

	public void setStatus(bool active){
		if (active) {
			isActive = true;
			int randomInt = Random.Range (0, players.Length);
			if (!players [randomInt].GetComponent<PlayerController> ().isDead) {
				GameObject attack = Instantiate (CircleTrapPrefab, players[randomInt].transform);
			}
			lastPlayerID = randomInt;
			currentTrapCount = 1;
			nextTrapTime = Time.time + interval;

		}
		if (!active) {
			isActive = false;
		}
	}

}
