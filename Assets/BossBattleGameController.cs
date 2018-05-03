using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossBattleGameController : MonoBehaviour {

    public enum GameMode{Survival, KOTH, Tutorial, Boss};
    public GameObject[] players;
    public GameObject[] playerIndicators;
    public int respawnTime;
    public int respawnLives;
    public Text gameOverText;
    public TextMesh respawnText;
    [Range(1.0f, 10.0f)]
    public float healthRegenRange;
    public float max_x;
    public float max_y;
    public float respawnDistanceFromCenter;

    private int currentKeyboardInput;
    private static bool gameOver;
    private GameMode gameMode = GameMode.Boss;
    private float lastRespawnTime;


    // Use this for initialization
    void Start () {
        gameOver = false;

        //respawns = new Queue<RespawnEvent> ();
        SetRespawnText();
    }
	
	// Update is called once per frame
	void Update () {
        if (gameOver && Input.GetButton("Submit"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetButton("Cancel"))
        {
            Application.Quit();
        }

        // If 1, 2, 3 or 4 is pressed, set currentKeyboardInput to that player ID
        SetCurrentKeyboardInput();

        CheckPlayerRegen();
        //		if (respawns.Count != 0) {
        //			RespawnEvent nextRespawn = respawns.Peek ();
        //			if (Time.time >= nextRespawn.time) {
        //			}
        //		}
    }

    private void SetRespawnText()
    {
        respawnText.text = "Lives: " + respawnLives;
    }

    public static bool IsGameOver()
    {
        return gameOver;
    }

    void SetCurrentKeyboardInput()
    {
        for (int i = 1; i < players.Length + 1; i++)
        {
            if (Input.GetButton("Toggle" + i))
            {
                currentKeyboardInput = i;
            }
        }
    }

    public int GetCurrentKeyboardInput()
    {
        return currentKeyboardInput;
    }

    void CheckPlayerRegen()
    {
        if (players.Length == 2)
        {
            float playerDistance = Vector3.Distance(players[0].transform.position, players[1].transform.position);

            bool bothAlive = false;
            if (players[0].GetComponent<HealthController>().IsAlive() && players[1].GetComponent<HealthController>().IsAlive())
            {
                bothAlive = true;
            }

            if (bothAlive && playerDistance <= healthRegenRange)
            {
                for (int i = 0; i < 2; i++)
                {
                    players[i].GetComponent<HealthController>().StartRegeneration();
                    players[i].GetComponent<ParticleSystem>().Play();
                    playerIndicators[i].GetComponent<ParticleSystem>().Play();
                }
                return;
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    players[i].GetComponent<HealthController>().StopRegeneration();
                    players[i].GetComponent<ParticleSystem>().Pause();
                    players[i].GetComponent<ParticleSystem>().Clear();
                    playerIndicators[i].GetComponent<ParticleSystem>().Pause();
                    playerIndicators[i].GetComponent<ParticleSystem>().Clear();
                }
            }
        }
    }

    public void NotifyDeath(int playerID, int teamID)
    {
        
        
        bool oneAlive = false;
        if (players[0].GetComponent<HealthController>().IsAlive() || players[1].GetComponent<HealthController>().IsAlive())
        {
            oneAlive = true;
        }

        if (respawnLives <= 0 && !oneAlive && Time.time > lastRespawnTime)
        {
            SetGameOver(2);
        }
    }

    public void BossDied()
    {
        SetGameOver(1);
    }

    void SetGameOver(int winner)
    {
        gameOver = true;
        if (winner == 1)
        {
            gameOverText.text = "Congratulations!!! Your team defeated the boss!!!  Press SQUARE or ENTER to restart.";
        }
        else if (winner == 2)
        {
            gameOverText.text = "Too bad!!! You were defeated by the boss!!!  Press SQUARE or ENTER to restart.";
        }
    }

    public int GetRespawnTime()
    {
        if (respawnLives > 0)
        {
            respawnLives = respawnLives - 1;
            SetRespawnText();
            lastRespawnTime = Time.time + respawnTime;
            return respawnTime;
        }
        else
        {
            return -1;
        }
    }

    public Vector2 GetRespawnPosition()
    {
        while (true)
        {
            Vector2 randomVector = new Vector2(Random.Range(-max_x, max_x), Random.Range(-max_y, max_y));
            Vector2 center = new Vector2(0f, 0f);
            if (Vector2.Distance(randomVector, center) >= respawnDistanceFromCenter)
            {
                return randomVector;
            }
        }
    }

    public GameObject[] GetAllPlayers()
    {
        return players;
    }

    public GameMode GetGameMode()
    {
        return gameMode;
    }

}
