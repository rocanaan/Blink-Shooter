using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HillController : MonoBehaviour {

	private int countTeam1;
	private int countTeam2;

	public int captureScore;

	public Material team1Material;
	public Material team2Material;
	public Material emptyMaterial;
	public Material contestedMaterial;

	public Text team1Text;
	public Text team2Text;

	private int team1Score;
	private int team2Score;

	private BossBattleGameController gameController;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<BossBattleGameController> ();
	}
		
	
	// Update is called once per frame
	void Update () {
		if (!GameController.IsGameOver ()) {
			if (countTeam1 > 0 && countTeam2 == 0) {
				team1Score++;
				GetComponent<Renderer> ().material = team1Material;
			}
			if (countTeam2 > 0 && countTeam1 == 0) {
				team2Score++;
				GetComponent<Renderer> ().material = team2Material;
			}
			if (countTeam1 == 0 && countTeam2 == 0) {
				GetComponent<Renderer> ().material = emptyMaterial;
			}
			if (countTeam1 > 0 && countTeam2 > 0) {
				GetComponent<Renderer> ().material = contestedMaterial;
			}
			

			team1Text.text = "Blue Score: " + ((100 * team1Score) / captureScore) +"%"; 
			team2Text.text = "Red Score: " + ((100 * team2Score) / captureScore) +"%";

			if (team1Score >= captureScore) {
				gameController.HillCaptured (1);
			}
			if (team2Score >= captureScore) {
				gameController.HillCaptured (2);
			}
		}
		
	}


	public void PlayerEnter(int playerID, int teamID){
		if (teamID == 1) {
			countTeam1++;
		}
		if (teamID == 2) {
			countTeam2++;
		}
		print ("Hill currently has " + countTeam1 + " players from team 1 and " + countTeam2 + " players from team 2.");
	}

	public void PlayerExit(int playerID, int teamID){
		if (teamID == 1) {
			countTeam1--;
		}
		if (teamID == 2) {
			countTeam2--;
		}
		print ("Hill currently has " + countTeam1 + " players from team 1 and " + countTeam2 + " players from team 2.");
	}
//	// check if getContacts or isTouching or overlapCollider gets a collision to a specific object
//	void onTriggerStay2D(Collision2D col){
////		if (col.tag == "Player") {
////			
////		}
//	}
}
