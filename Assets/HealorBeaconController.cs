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
	private LineRenderer lr;

	public Material linkMaterial;
	public float initialDelay;
	public float activeDuration;
	public float inactiveDuration;
	private float nextTransitionTime;

    // Use this for initialization
    void Start () {
        explosionSpawner = transform.GetComponent<ExplosionSpawner>();
        cam = GameObject.FindWithTag("MainCamera");
        explosionSpawner.SpawnExplosion();
        cam.GetComponent<CameraController>().CamShake(0.2f, 0.15f);

        boss = GameObject.FindGameObjectWithTag("Boss");
        bossHealthController = boss.GetComponent<HealthController>();

		lr = GetComponent<LineRenderer> ();
		lr.material = linkMaterial;
		lr.enabled = false;
		nextTransitionTime = Time.time + Random.Range(0,initialDelay);

		GetComponent<SoundEffectsController> ().PlayClip ("BeaconSpawn");

        //StartCoroutine(HealBoss());
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > nextTransitionTime) {
			if (lr.enabled == false) {
				lr.enabled = true;
				nextTransitionTime = Time.time + activeDuration;
				HealBoss ();
			} else {
				lr.enabled = false;
				nextTransitionTime = Time.time + inactiveDuration;
			}

		}
		lr.SetPosition(0, transform.position);
		lr.SetPosition(1, boss.transform.position);

	}

    private void HealBoss()
    {
//        while (true)
//        {
//            yield return new WaitForSeconds(regenTime);
//            bossHealthController.Heal(regenRate);
//        }        
		bossHealthController.Heal(regenRate);
    }

    private void OnDestroy()
    {
        boss.GetComponentInChildren<HealerBeaconSpawner>().DecrementAliveCount(); // decrements the count of alive beacons as it gets destroyed
    }
}
