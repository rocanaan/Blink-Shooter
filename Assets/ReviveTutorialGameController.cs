using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReviveTutorialGameController : MonoBehaviour {

    public Canvas canvas;
    public Text gameOverText;
    public GameObject alivePlayer;
	public GameObject deadPlayer;

	private HealthController p1Health;
    private Animator anim;
    private bool gameOver;

	bool firstTime;

    // Use this for initialization
    void Start () {
        gameOver = false;
        Time.timeScale = 0f;

		firstTime = true;

//		p1Health = alivePlayer.GetComponentInChildren<HealthController> ();
//
//		p1Health.TakeDamage (2);
//		deadPlayer.GetComponentInChildren<HealthController> ().TakeDamage (10);
//		deadPlayer.GetComponentInChildren<PlayerController> ().PlayerDeath ();
        anim = canvas.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		if (firstTime) {
			p1Health = alivePlayer.GetComponentInChildren<HealthController> ();
	
			p1Health.TakeDamage (2);
			deadPlayer.GetComponentInChildren<HealthController> ().TakeDamage (10);
			deadPlayer.GetComponentInChildren<PlayerController> ().PlayerDeath ();

			firstTime = false;
		}

        if (Time.timeScale == 0f && Input.GetButton("Submit"))
        {
            print("LET ME START ALREADY");
            Time.timeScale = 1.0f;
            anim.SetTrigger("Start");
        }

        int health = p1Health.GetHealth ();
		if (health == p1Health.maxHealth) {
			GameOver ();
		}

        if (gameOver && Input.GetButton("Submit"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        }

    }

    private void GameOver()
    {
        gameOver = true;
        gameOverText.text = "Congratulations! You Cleared The Second Tutorial!";
        anim.SetTrigger("GameOver");
    }
}
