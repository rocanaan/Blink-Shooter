using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FirstTutorialGameController : MonoBehaviour {

	public GameObject[] guns;
    public Text gameOverText;
    public Canvas canvas;

    private Animator anim;
    private bool gameOver;
    private PauseGame pauseGame;

    // Use this for initialization
    void Start () {
        Time.timeScale = 0f;
        anim = canvas.GetComponent<Animator>();
        gameOver = false;
        pauseGame = GetComponent<PauseGame>();
        
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

        if (Time.timeScale == 0f && Input.GetButton("Submit"))
        {
            print("trying to unpause");
            Time.timeScale = 1f;
            anim.SetTrigger("Start");
            //anim.SetTrigger("GameOver");
        }

		if (gameOver) {
			GameOver ();
		}

        if(gameOver && Input.GetButton("Submit"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        }

	}

	private void GameOver(){
        gameOverText.text = "Congratulations! You Cleared The First Tutorial!";
        anim.SetTrigger("GameOver");
        
	}
}
