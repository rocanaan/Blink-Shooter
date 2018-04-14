using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseCharacterBehavior : CharacterBehavior {

	public GameObject bodyObject;
	public GameObject ghostObject;
	private GhostController ghostController;

	public float moveSpeed;
	public float speedWhileFiring;
	public float shotInterval;
	private float nextShot;



	// Use this for initialization
	void Start () {
		ghostController = ghostObject.GetComponent<GhostController> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
