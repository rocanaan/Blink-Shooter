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
    public GameObject trianglePrefab;
    [Range(1.0f, 10.0f)]
    public float healthRegenRange;
    public float max_x;
    public float max_y;
    public float respawnDistanceFromCenter;

    private int currentKeyboardInput;
    private static bool gameOver;
    private GameMode gameMode = GameMode.Boss;
    private float lastRespawnTime;

	private GameObject audioSource;
	private AudioController soundtrackController;


    // Use this for initialization
    void Start () {
        gameOver = false;

        //respawns = new Queue<RespawnEvent> ();
        SetRespawnText();

		audioSource = GameObject.FindGameObjectWithTag ("GlobalAudioSource");
		soundtrackController = audioSource.GetComponent<AudioController>();
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

    public void SetRespawnText()
    {
        respawnText.text = "Lives: " + respawnLives;
    }

    public bool IsGameOver()
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

    public void NotifyPlayerDeath()
    {
        print("NotifyDeath() CALLED");
        // there are 3 possible cases
        bool player1Alive = players[0].GetComponent<HealthController>().IsAlive();
        bool player2Alive = players[1].GetComponent<HealthController>().IsAlive();

        if (player1Alive || player2Alive)
        {
            // CASE 1
            if (player1Alive && !player2Alive)
            {
                print("Player2 died");
                // Player 1 is alive, Player 2 is dead                
                StartCoroutine(CheckPlayerRespawn(1, 0));
            }
            // CASE 2
            else if (!player1Alive && player2Alive)
            {
                // Player 2 is alive, Player 1 is dead
                Vector3 pos = players[0].transform.position;
                StartCoroutine(CheckPlayerRespawn(0, 1));
            }
        }
        // CASE 3
        else if (!players[0].GetComponent<HealthController>().IsAlive() && !players[1].GetComponent<HealthController>().IsAlive())
        {
            // Both players are dead
            SetGameOver(2); // Boss wins
        }
    }

    private IEnumerator CheckPlayerRespawn(int deadPlayerIndex, int alivePlayerIndex)
    {
        float timer = 0f;

        bool waitingToRespawn = true;
        Vector3 pos = players[deadPlayerIndex].transform.position;
        GameObject triangle = Instantiate(trianglePrefab, pos, Quaternion.identity);
        triangle.SetActive(false);
        while (waitingToRespawn)
        {
            if (!gameOver && respawnLives > 0)
            {
                float playerDistance = Vector3.Distance(players[0].transform.position, players[1].transform.position);

                if (playerDistance <= healthRegenRange)
                {
                    // trigger some sort of animation on the dead player here
                    if(timer > 0.1f)
                    {
                        triangle.SetActive(!triangle.activeSelf);
                        timer = 0f;
                    }
                    else
                    {
                        timer = timer + Time.deltaTime;
                    }

                    string controllerName = players[alivePlayerIndex].GetComponent<PlayerController>().GetControllerName();
                    if (Input.GetButton("Respawn" + controllerName))
                    {
                        Destroy(triangle);
                        respawnLives = respawnLives - 1;
                        SetRespawnText();
                        // call a function to respawn the player here
                        players[deadPlayerIndex].GetComponent<PlayerController>().PlayerRespawn();
                        waitingToRespawn = false;
                        print("Respawning PLAYER " + deadPlayerIndex);
                    }
                }
                else
                {
                    triangle.SetActive(false);
                }
            }
            else
            {
                waitingToRespawn = false;
            }
            yield return null;
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
			gameOverText.text = "Congratulations!!! Your team defeated the boss!!!\r\n" +
            	"Press SQUARE or ENTER to restart.";
			soundtrackController.PlayVictorySoundtrack ();

        }
        else if (winner == 2)
        {
            gameOverText.text = "Too bad!!! You were defeated by the boss!!!\r\n" +
            	"Press SQUARE or ENTER to restart.";
			soundtrackController.PlayDefeatSoundtrack ();

        }
    }

    public int GetRespawnTime()
    {
        if (respawnLives > 0)
        {
            respawnLives = respawnLives - 1;
            //SetRespawnText();
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
