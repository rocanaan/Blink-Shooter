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


				// @ Burak the next few lines determine where the boss is hit by healing beam
				// As it is now, if you do something with the point of contact here, it will happen only once, at the start of the beam
				// Because the beam is so short lived, I thin this is okay.
				// If you do something continuously, you have to move these lines to where I point below
				RaycastHit2D[] allHits = Physics2D.RaycastAll(transform.position, boss.transform.position, 60.0f);
				foreach (RaycastHit2D hit in allHits)
				{
					Vector3 pointOfContact = Vector3.zero;
					Collider2D col = hit.collider;
					if (col.tag == "Boss" )
					{
						pointOfContact = hit.point;
					}
					// Do something with point of contact, such as spawn particles
				} // End of point of contact logic
			} else {
				lr.enabled = false;
				nextTransitionTime = Time.time + inactiveDuration;
			}

		}

		// If you want to make point of contact detection continuous:
		// if (lr.enabled){
		// 	detect point of contact
		//  do something with it


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
