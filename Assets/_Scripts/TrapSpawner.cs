using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawner : MonoBehaviour {


	public GameObject trapObject;

	private GameObject[] players;
	private GameController gameController;


	private int nextTarget;



	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		players = gameController.getAllPlayers ();

		nextTarget = Random.Range (0, players.Length);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void FireTrap (){
		GameObject trap = Instantiate (trapObject, players [nextTarget].transform.position, Quaternion.identity);
		trap.GetComponent<PlayerTrapController> ().player = players [nextTarget];
		nextTarget = (nextTarget + 1) % players.Length;
	}
}
