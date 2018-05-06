using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUpSpawner : MonoBehaviour {

    public GameObject healthPickUpPrefab;

	public void SpawnHealthPickUps()
    {
        Instantiate(healthPickUpPrefab, transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        SpawnHealthPickUps();
    }
}
