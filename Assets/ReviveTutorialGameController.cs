using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReviveTutorialGameController : MonoBehaviour {

	public GameObject alivePlayer;
	public GameObject deadPlayer;

	private HealthController p1Health;

	// Use this for initialization
	void Start () {
		p1Health = alivePlayer.GetComponentInChildren<HealthController> ();

		p1Health.TakeDamage (2);
		deadPlayer.GetComponentInChildren<HealthController> ().TakeDamage (10);
		deadPlayer.GetComponentInChildren<PlayerController> ().PlayerDeath ();
	}
	
	// Update is called once per frame
	void Update () {
		int health = p1Health.GetHealth ();
		if (health == p1Health.maxHealth) {
			GameOver ();
		}
		
	}

	void GameOver()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex+1,LoadSceneMode.Single);
	}
}
