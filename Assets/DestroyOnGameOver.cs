using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnGameOver : MonoBehaviour {

    private BossBattleGameController gameController;

    // Use this for initialization
    void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<BossBattleGameController>();
    }

    private void Update()
    {
        if (gameController.IsGameOver())
        {
            Destroy(gameObject);
        }
    }
}
