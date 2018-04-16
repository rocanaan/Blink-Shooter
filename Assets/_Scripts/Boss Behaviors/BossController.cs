using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

	private SoundEffectsController sfxController;

	private int currentHealth;

	private static int difficulty;

	public int teamID;

	public Material startingDifficultyMaterial;
	public Material difficulty1Material;
	public Material difficulty2Material;

	BlinkAnimation coreBlinkAnimation;

	public float behaviorDuration;
	public float behaviorDelay;

	private float nextBehaviorStartTime;
	private float nextBehaviorEndTime;


	// Boss behaviors
	private WanderBehavior wanderBehavior;
	private FollowBehavior followBehavior;
	private ShieldSpawner shieldSpawner;
	private DirectionalShieldSpawner directionalShieldSpawner;
	private GunSpawner gunSpawner;
	private WallGunSpawner wallGunSpawner;
	private TargettedFireBehavior targetFire;
	private TrapSpawner trapSpawner;
    private HealthController bossHealth;

    private GameController gameController;

	private GameObject audioSource;

	// Use this for initialization
	void Start () {
		coreBlinkAnimation = GetComponentInChildren<BlinkAnimation> ();
        bossHealth = transform.GetComponent<HealthController>();
		currentHealth = bossHealth.GetHealth();
		difficulty = 0;

		wanderBehavior = GetComponent<WanderBehavior> ();
		wanderBehavior.setActive (true);
		followBehavior = GetComponent<FollowBehavior> ();
		followBehavior.setStatus (false);

		shieldSpawner = GetComponentInChildren<ShieldSpawner> ();
		directionalShieldSpawner = GetComponentInChildren<DirectionalShieldSpawner> ();
		gunSpawner = GetComponentInChildren<GunSpawner> ();
		wallGunSpawner = GetComponentInChildren<WallGunSpawner> ();
		targetFire = GetComponent<TargettedFireBehavior> ();
		trapSpawner = GetComponentInChildren<TrapSpawner> ();

		nextBehaviorStartTime = Time.time + behaviorDelay;
		nextBehaviorEndTime = nextBehaviorStartTime + behaviorDuration;
//		targetFire.setStatus (true);
//		wallGunSpawner.setStatus (true);

		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();

		audioSource = GameObject.FindGameObjectWithTag ("GlobalAudioSource");
		sfxController = audioSource.GetComponent<SoundEffectsController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= nextBehaviorStartTime) {
			selectBehaviors ();
		}
		else if (Time.time >= nextBehaviorEndTime) {
			deactivateBehaviors ();
			print ("deactivating behaviors");
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Shot") {
			ShotAttributes shot = col.GetComponent<ShotAttributes> ();
			if (shot.getTeamID() != teamID) {
				TakeDamage (shot.damage);
				Destroy (col.gameObject);
			}
		}
	}

	void TakeDamage (int damage){

        bool isAlive = bossHealth.TakeDamage(damage);
		print ("Boss Health is" + currentHealth);
		coreBlinkAnimation.startAnimation ();


		float healthRatio = (float)currentHealth / (float)bossHealth.maxHealth;

		if (difficulty == 0 && healthRatio < 0.6) {
			difficulty = 1;
            bossHealth.SetMaterial(difficulty1Material);
			GetComponentsInChildren<Renderer> () [1].material = difficulty1Material;

			deactivateBehaviors ();

//			shieldSpawner.setStatus (true);
//			wallGunSpawner.setStatus (false);

		}

		if (difficulty == 1 && healthRatio < 0.2) {
			difficulty = 2;
            bossHealth.SetMaterial(difficulty2Material);
            GetComponentsInChildren<Renderer> () [1].material = difficulty2Material;

			shieldSpawner.numberShields *= 2;
			gunSpawner.numberGuns *= 2;
			wallGunSpawner.gunsPerWall *= 2;
			targetFire.fireInterval /= 2;

			followBehavior.setStatus(true);
			wanderBehavior.setActive (false);

			trapSpawner.FireTrap ();
			deactivateBehaviors ();

//			shieldSpawner.setStatus (false);
//			gunSpawner.setStatus (true);
//			wallGunSpawner.gunsPerWall = 2;
//			wallGunSpawner.setStatus (true);
		
		}
			
		if (!isAlive) {
			//playerDeath ();
			print("Boss died!");
			gameController.bossDied();
			Destroy (gameObject);
		}
	}

	//TODO: refactor this into an array of materials
	public Material getCurrentMaterial(){
		if (difficulty == 0) {
			return startingDifficultyMaterial;
		}
		if (difficulty == 1) {
			return difficulty1Material;
		}
		if (difficulty == 2) {
			return difficulty2Material;
		}
		return null;
	}

	//TODO this should be refactored to place the potential behaviors in a list and select from them.
	void selectBehaviors(){
		if (difficulty == 2) {
			int trapChance = Random.Range (0, 10);
			if (trapChance == 0) {
				trapSpawner.FireTrap ();
				nextBehaviorEndTime = Time.time + behaviorDuration;
				nextBehaviorStartTime = nextBehaviorEndTime + behaviorDelay;
				return;
			}
		}
		int firstBehavior = Random.Range (0, 4);
		//int firstBehavior =0;
		switch (firstBehavior) {
		case 0:
			directionalShieldSpawner.setStatus (true);
			followBehavior.setStatus (true);
			wanderBehavior.setActive (false);
			break;
		case 1:
			gunSpawner.setStatus (true);
			break;
		case 2:
			wallGunSpawner.setStatus (true);

			break;
		case 3:
			targetFire.setStatus (true);
			break;
		}
		if (difficulty > 0) {
			int secondBehavior = Random.Range (0, 4);
			while (secondBehavior == firstBehavior){
				secondBehavior = Random.Range (0, 4);
			}
			switch (secondBehavior) {
			case 0:
				directionalShieldSpawner.setStatus (true);
				followBehavior.setStatus (true);
				wanderBehavior.setActive (false);
				break;
			case 1:
				gunSpawner.setStatus (true);
				break;
			case 2:
				wallGunSpawner.setStatus (true);

				break;
			case 3:
				targetFire.setStatus (true);
				break;
			}

		}

		nextBehaviorEndTime = Time.time + behaviorDuration;
		nextBehaviorStartTime = nextBehaviorEndTime + behaviorDelay;
	}

	void deactivateBehaviors(){
		shieldSpawner.setStatus (false);
		gunSpawner.setStatus (false);
		wallGunSpawner.setStatus (false);
		targetFire.setStatus (false);
		directionalShieldSpawner.setStatus (false);

		if (difficulty != 2) {
			followBehavior.setStatus (false);
			wanderBehavior.setActive (true);
		} else {
			followBehavior.setStatus (true);
			wanderBehavior.setActive (false);
		}
		nextBehaviorStartTime = Time.time + behaviorDelay;
		nextBehaviorEndTime = nextBehaviorStartTime + behaviorDuration;
	}

	void spawnTrap(){
	}
}
