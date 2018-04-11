using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawner : MonoBehaviour {


	public GameObject trapObject;
	public GameObject[] players;

	private int nextTarget;
	private SoundEffectsController sfxController;


	// Use this for initialization
	void Start () {
		nextTarget = Random.Range (0, players.Length);
		sfxController = GetComponentInParent<SoundEffectsController> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void FireTrap (){
		GameObject trap = Instantiate (trapObject, players [nextTarget].transform.position, Quaternion.identity);
		trap.GetComponent<PlayerTrapController> ().player = players [nextTarget];
		nextTarget = (nextTarget + 1) % players.Length;
		sfxController.PlayClip("Trap");
	}
}
