using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollidesWithHill : MonoBehaviour {

	private PlayerController pc;
	private int playerID;
	private int teamID;
	private int frameCount; 


	// Use this for initialization


	void Start () {
		pc = GetComponent<PlayerController> ();
		print ("Player ID on this script is " + pc.playerID);
		playerID = pc.playerID;
		teamID = pc.teamID;

			}

	void OnTriggerExit2D(Collider2D col){
		if (col.tag == "Hill") {
			HillController hc = col.GetComponent<HillController> ();
			hc.PlayerExit (playerID, teamID);
			
			print (" (Trigger Leave) Player / Group " + playerID + teamID + " left the Hill at frame " + frameCount);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Hill") {
			HillController hc = col.GetComponent<HillController> ();
			hc.PlayerEnter (playerID, teamID);

			print ("(Trigger enter) Player / Group " + playerID + teamID + " is on the Hill at frame " + frameCount);
		}
	}
	
	// Update is called once per frame
	void Update () {
		frameCount++;
		
	}
}
