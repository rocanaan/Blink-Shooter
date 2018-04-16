using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour {

    public GameObject minions;
    public GameObject minion;
    public int numOfMinions;
    public float xRange;
    public float yRange;
    public float minDistance;

    private List<GameObject> minionList;

    // Use this for initialization
    void Start () {
        minionList = new List<GameObject>();
        SpawnMinions();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnMinions()
    {
        for(int i=0; i < numOfMinions; i++)
        {
            float xCoord;
            float yCoord;
            Vector2 pos = Vector2.zero;
            do
            {
                xCoord = Mathf.RoundToInt(Random.Range(-xRange, xRange));
                yCoord = Mathf.RoundToInt(Random.Range(-yRange, yRange));
                pos = new Vector2(xCoord, yCoord);
            } while (!IsCoordUnoccupied(pos) || !FarEnough(pos));

            Quaternion randomQua = Quaternion.Euler(0.0f, 0.0f, Random.Range(0, 360));
            GameObject newMinion = Instantiate(minion, new Vector3(pos.x, pos.y, 0f), Quaternion.identity, minions.transform);
            minionList.Add(newMinion);
        }
    }

    private bool FarEnough(Vector2 pos)
    {
        foreach (GameObject obs in minionList)
        {
            Vector2 minionPos = new Vector2(obs.transform.position.x, obs.transform.position.y);
            if (Vector2.Distance(minionPos, pos) < minDistance)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsCoordUnoccupied(Vector2 pos)
    {
        if (Physics2D.OverlapCircle(pos, 0.45f))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
