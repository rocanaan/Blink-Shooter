using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTracker : MonoBehaviour {

	// Game object and material representing the health of the player
	public GameObject healthMarker;
	public Material material;

	// Variables to control the placement of each health marker
	public Vector2 startingPosition;
	public Vector2 offset;

	private List<GameObject> healthBar;

	// Use this for initialization
	void Awake () {
		healthBar = new List<GameObject> ();
	}

	// Sets current health. Can fail if passed negative health or health over max health
	public void setHealth(int health){
		if (health >= 0){
			for (int i = 0; i < health; i++) {
				healthBar [i].SetActive (true);
			}
			for (int j = health; j < healthBar.Count; j++) {
				healthBar [j].SetActive (false);
			}
		}
	}

	// Starts the health tracker by instantiating a number of markers equal to the max health
	public void startHealth(int health){
		for (int i = 0; i < health; i++) {
			print ("Creating health marker number " + i);
			GameObject marker = Instantiate (healthMarker, new Vector3 (startingPosition.x + i * offset.x, startingPosition.y + i * offset.y, 0), Quaternion.identity);
			marker.GetComponent<Renderer> ().material = material;
			healthBar.Add (marker);
			//healthBar.Add(marker);
		}

	}

}
