using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerBeaconSpawner : MonoBehaviour {

    public GameObject beaconParent;
    public GameObject healerBeaconPrefab;
    public float xRange;
    public float yRange;
    public float minimumDistance;

    private List<GameObject> beaconList;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnHealer()
    {
        GameObject beacon;
        beaconList = new List<GameObject>();
        float randX = 0f;
        float randY = 0f;
        Vector2 pos = Vector2.zero;

        // 1st quadrant
        do
        {
            randX = Random.Range(0, xRange);
            randY = Random.Range(0, yRange);
            pos = new Vector2(randX, randY);
        } while (!IsCoordUnoccupied(pos) || !FarEnough(pos, minimumDistance));
        beacon = Instantiate(healerBeaconPrefab, new Vector3(pos.x, pos.y, 0f), Quaternion.identity, beaconParent.transform);
        beaconList.Add(beacon);

        // 3rd quadrant
        do
        {
            randX = Random.Range(-xRange, 0);
            randY = Random.Range(-yRange, 0);
            pos = new Vector2(randX, randY);
        } while (!IsCoordUnoccupied(pos) || !FarEnough(pos, minimumDistance));
        beacon = Instantiate(healerBeaconPrefab, new Vector3(pos.x, pos.y, 0f), Quaternion.identity, beaconParent.transform);
        beaconList.Add(beacon);

        // 2nd quadrant
        do
        {
            randX = Random.Range(-xRange, 0);
            randY = Random.Range(0, yRange);
            pos = new Vector2(randX, randY);
        } while (!IsCoordUnoccupied(pos) || !FarEnough(pos, minimumDistance));
        beacon = Instantiate(healerBeaconPrefab, new Vector3(pos.x, pos.y, 0f), Quaternion.identity, beaconParent.transform);
        beaconList.Add(beacon);

        // 4th quadrant
        do
        {
            randX = Random.Range(0, xRange);
            randY = Random.Range(-yRange, 0);
            pos = new Vector2(randX, randY);
        } while (!IsCoordUnoccupied(pos) || !FarEnough(pos, minimumDistance));
        beacon = Instantiate(healerBeaconPrefab, new Vector3(pos.x, pos.y, 0f), Quaternion.identity, beaconParent.transform);
        beaconList.Add(beacon);
    }

    private bool FarEnough(Vector2 pos, float minDistance)
    {
        foreach (GameObject obs in beaconList)
        {
            Vector2 beaconPos = new Vector2(obs.transform.position.x, obs.transform.position.y);
            if (Vector2.Distance(beaconPos, pos) < minDistance)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsCoordUnoccupied(Vector2 pos)
    {
        if (Physics2D.OverlapCircle(pos, healerBeaconPrefab.transform.localScale.x/Mathf.Sqrt(2)))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
