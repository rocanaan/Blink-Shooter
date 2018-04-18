using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCircleTrapSpawner : MonoBehaviour {

	public GameObject CircleTrapPrefab;

	private GameObject[] players;
	private GameController gameController;

	void Start(){
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		players = gameController.getAllPlayers ();
	}

	public void setStatus(bool active){
		if (active) {
			
			int randomInt = Random.Range (0, players.Length);
			if (!players [randomInt].GetComponent<PlayerController> ().isDead) {
				GameObject attack = Instantiate (CircleTrapPrefab, players[randomInt].transform);
			}

		}
	}

}
