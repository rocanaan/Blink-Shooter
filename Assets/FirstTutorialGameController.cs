using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstTutorialGameController : MonoBehaviour {

	public GameObject[] guns;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		bool gameOver = true;
		foreach (GameObject gun in guns){
			if (gun != null) {
				gameOver = false;
				break;
			}
		}
		if (gameOver) {
			GameOver ();
		}
	}

	private void GameOver(){
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex+1,LoadSceneMode.Single);
	}
}
