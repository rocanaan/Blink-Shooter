using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealorBeaconController : MonoBehaviour {

    public int regenRate;
    public float regenTime;

    private GameObject boss;
    private HealthController bossHealthController;
    private ExplosionSpawner explosionSpawner;
    private GameObject cam;

    // Use this for initialization
    void Start () {
        explosionSpawner = transform.GetComponent<ExplosionSpawner>();
        cam = GameObject.FindWithTag("MainCamera");
        explosionSpawner.SpawnExplosion();
        cam.GetComponent<CameraController>().CamShake(0.2f, 0.15f);

        boss = GameObject.FindGameObjectWithTag("Boss");
        bossHealthController = boss.GetComponent<HealthController>();

        StartCoroutine(HealBoss());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator HealBoss()
    {
        while (true)
        {
            yield return new WaitForSeconds(regenTime);
            bossHealthController.Heal(regenRate);
        }        
    }
}
