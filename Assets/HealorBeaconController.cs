using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealorBeaconController : MonoBehaviour {

    public GameObject glow;
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
    private BossBattleGameController gameController;

    // Use this for initialization
    void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<BossBattleGameController>();
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
        bool gameOver = gameController.IsGameOver();

        if (!gameOver)
        {
            if (Time.time > nextTransitionTime)
            {
                if (lr.enabled == false)
                {
                    lr.enabled = true;
                    nextTransitionTime = Time.time + activeDuration;
                    HealBoss();


                    RaycastHit2D[] allHits = Physics2D.RaycastAll(transform.position, boss.transform.position - transform.position, 60.0f);
                    foreach (RaycastHit2D hit in allHits)
                    {
                        Vector3 pointOfContact = Vector3.zero;
                        Collider2D col = hit.collider;
                        if (col.tag == "Boss")
                        {
                            pointOfContact = new Vector3(hit.point.x, hit.point.y);

                            Vector3 dir = transform.position - pointOfContact;
                            GameObject ps = Instantiate(glow, pointOfContact, Quaternion.LookRotation(dir), col.transform);
                            var main = ps.GetComponent<ParticleSystem>().main;
                            main.duration = activeDuration;
                        }                    
                    } // End of point of contact logic
                }
                else
                {
                    lr.enabled = false;
                    nextTransitionTime = Time.time + inactiveDuration;
                }

            }

            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, boss.transform.position);
        }        

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
        bool gameOver = gameController.IsGameOver();
        if (!gameOver) // we should check if game is over. if it is, boss might have been destroyed.
        {
            boss.GetComponentInChildren<HealerBeaconSpawner>().DecrementAliveCount(); // decrements the count of alive beacons as it gets destroyed
        }        
    }
}
