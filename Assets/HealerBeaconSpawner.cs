using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerBeaconSpawner : MonoBehaviour
{

    public GameObject beaconParent;
    public GameObject healerBeaconPrefab;
    public float xRange;
    public float yRange;
    public float minimumDistance;

    private List<GameObject> beaconList;


    private IEnumerator Spawn(float timeInterval)
    {
        Debug.Log("Spawn called");
        GameObject beacon;

        beaconList = new List<GameObject>();
        yield return new WaitForSeconds(timeInterval * 2);
        Debug.Log("Spawn returned once again");

        // 1st quadrant
        beacon = SpawnBeacon(0f, xRange, 0f, yRange);
        beaconList.Add(beacon);
        yield return new WaitForSeconds(timeInterval);

        // 3rd quadrant
        SpawnBeacon(-xRange, 0f, -yRange, 0f);
        beaconList.Add(beacon);
        yield return new WaitForSeconds(timeInterval);

        // 2nd quadrant
        SpawnBeacon(-xRange, 0f, 0f, yRange);
        beaconList.Add(beacon);
        yield return new WaitForSeconds(timeInterval);

        // 4th quadrant
        SpawnBeacon(0f, xRange, -yRange, 0f);
        beaconList.Add(beacon);
        yield return null;
    }

    private GameObject SpawnBeacon(float xMin, float xMax, float yMin, float yMax)
    {
        Debug.Log("SpawnBeacon CALLED");
        GameObject beacon;
        WanderBehavior wb;
        float randX = 0f;
        float randY = 0f;
        Vector2 pos = Vector2.zero;
        do
        {
            randX = Random.Range(xMin, xMax);
            randY = Random.Range(yMin, yMax);
            pos = new Vector2(randX, randY);
        } while (!IsCoordUnoccupied(pos) || !FarEnough(pos, minimumDistance));
        beacon = Instantiate(healerBeaconPrefab, new Vector3(pos.x, pos.y, 0f), Quaternion.identity, beaconParent.transform);
        wb = beacon.GetComponent<WanderBehavior>();
        wb.xMin = xMin;
        wb.yMin = yMin;
        wb.xMax = xMax;
        wb.yMax = yMax;
        wb.offsetTolerance = 0.2f;
        wb.setStatus(true);
        wb.getNewTarget();

        return beacon;
    }

    public void SpawnHealers(float timeInterval)
    {
        StartCoroutine(Spawn(timeInterval));
    }

    private bool FarEnough(Vector2 pos, float minDistance)
    {
        Debug.Log("FarEnough CALLED");
        foreach (GameObject obs in beaconList)
        {
            if (obs != null)
            {
                Vector2 beaconPos = new Vector2(obs.transform.position.x, obs.transform.position.y);
                if (Vector2.Distance(beaconPos, pos) < minDistance)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool IsCoordUnoccupied(Vector2 pos)
    {
        if (Physics2D.OverlapCircle(pos, healerBeaconPrefab.transform.localScale.x / Mathf.Sqrt(2)))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
