using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotAttributes : MonoBehaviour {

	private int teamID;
	private int playerID; 
	public int damage;
	public bool energy;

	public int getTeamID(){
		return teamID;
	}

	public void setTeamID(int id){
		teamID = id;
	}

	public int getPlayerID(){
		return playerID;
	}

	public void setPlayerID(int id){
		playerID = id;
	}

}
