using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSplit : MonoBehaviour {

    private GameObject[] players;
    private GameController gameController;

    public GameObject laserSpawnerPrefab;

    public float angularSpeed;
    public float preparationTime;
    public float maxArcTraveled;

    // Use this for initialization
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        players = gameController.GetAllPlayers();
        //	setActive (true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetStatus(bool active)
    {
        if (active)
        {
            Vector3 playersMidpoint = getPlayersMidpoint();
            if (playersMidpoint == transform.position)
            {
                playersMidpoint = Vector3.right;
            }
            GameObject laserObj1 = Instantiate(laserSpawnerPrefab, transform.position, Quaternion.identity);
            LaserShooter laser1 = laserObj1.GetComponent<LaserShooter>();
            laser1.target = playersMidpoint;
            laser1.angularSpeed = angularSpeed;
            laser1.preparationTime = preparationTime;
            laser1.angleRotated = maxArcTraveled;
            laser1.transform.parent = transform;


            GameObject laserObj2 = Instantiate(laserSpawnerPrefab, transform.position, Quaternion.identity);
            LaserShooter laser2 = laserObj2.GetComponent<LaserShooter>();
            laser2.target = playersMidpoint;
            laser2.angularSpeed = -angularSpeed;
            laser2.preparationTime = preparationTime;
            laser2.angleRotated = maxArcTraveled;
            laser2.transform.parent = transform;

            laser1.Initialize();
            laser2.Initialize();
        }
    }

    private Vector3 getPlayersMidpoint()
    {
        Vector3 midpoint = Vector3.zero;
        foreach (GameObject player in players)
        {
            midpoint += player.transform.position;
        }
        midpoint = midpoint * (1.0f / players.Length);
        return midpoint;
    }
}
