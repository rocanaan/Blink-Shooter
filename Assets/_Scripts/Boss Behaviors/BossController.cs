﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

	private SoundEffectsController sfxController;

	private int currentHealth;

	//private static int difficulty;

	public int teamID;

	BlinkAnimation coreBlinkAnimation;

	public float defaultBehaviorDuration;
	private float currentBehaviorDuration;
	public float behaviorDelay;

	private float nextBehaviorStartTime;
	private float nextBehaviorEndTime;

	private BehaviorController behaviorController;


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
    private ParticleSystem healthIndicatorPS;
	private MinionSpawner minionSpawner;
	private ExpandingCircleSpawner expandingCircleSpawner;
	private LaserSplit laserSplittingAttack; // Note: have to fix nomeclature. For circle, the prefab that just instantiates the attack is attack, and the controller is spawner. Here, attack is the controller and controller is attack
	private PlayerCircleTrapSpawner playerCircleTrapSpawner;
    private BossBattleGameController gameController;

	private GameObject audioSource;
    private int targetedDamage;

    private AudioController soundtrackController;


	public float mediumStageDelay;
	private string mediumStatus;
	private int lastMediumBehavior;

	public float transitionDelay;
	private float transitionEndTime;

	public enum Stage {Easy, MediumTransition, Medium, HardTransition, Hard};
	public Stage currentStage;

	public float easyToMediumHealthThreshold;
	public float mediumToHardHealthThreshold;
	public float lastpushThreshold;

    private GameObject myCamera;
    private HealerBeaconSpawner healerBeaconSpawner;
    private bool gunsSpawnedMedium;
	private bool gunsSpawnedHard;
	private bool lastPushFired;

    private bool onFirstFrame;

	// Use this for initialization
	void Start () {
		coreBlinkAnimation = GetComponentInChildren<BlinkAnimation> ();
        bossHealth = transform.GetComponent<HealthController>();
        healthIndicatorPS = bossHealth.healthBar.GetComponentInParent<ParticleSystem>();
        myCamera = GameObject.FindGameObjectWithTag("MainCamera");
		currentHealth = bossHealth.GetHealth();
		//difficulty = 0;
		currentStage = Stage.Easy;

        wanderBehavior = GetComponent<WanderBehavior> ();
		followBehavior = GetComponent<FollowBehavior> ();
		ToggleMovement ("Wander");
//
		shieldSpawner = GetComponentInChildren<ShieldSpawner> ();
		directionalShieldSpawner = GetComponentInChildren<DirectionalShieldSpawner> ();
		gunSpawner = GetComponentInChildren<GunSpawner> ();
		wallGunSpawner = GetComponentInChildren<WallGunSpawner> ();
		targetFire = GetComponent<TargettedFireBehavior> ();
		trapSpawner = GetComponentInChildren<TrapSpawner> ();
		minionSpawner = GetComponentInChildren<MinionSpawner> ();
		expandingCircleSpawner = GetComponentInChildren<ExpandingCircleSpawner>();
		laserSplittingAttack = GetComponentInChildren<LaserSplit> ();
		playerCircleTrapSpawner = GetComponentInChildren<PlayerCircleTrapSpawner> ();
        healerBeaconSpawner = GetComponentInChildren<HealerBeaconSpawner>();



        //wallGunSpawner.setStatus (true);

        nextBehaviorStartTime = Time.time + behaviorDelay;
		nextBehaviorEndTime = nextBehaviorStartTime + defaultBehaviorDuration;
//		targetFire.setStatus (true);
//		wallGunSpawner.setStatus (true);

		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<BossBattleGameController> ();

		audioSource = GameObject.FindGameObjectWithTag ("GlobalAudioSource");
		sfxController = audioSource.GetComponent<SoundEffectsController> ();
        soundtrackController = audioSource.GetComponent<AudioController>();

        behaviorController = GetComponent<BehaviorController> ();

        targetedDamage = 1;

		currentBehaviorDuration = defaultBehaviorDuration;

		mediumStatus = "initial";
		lastMediumBehavior = -1;
        gunsSpawnedMedium = false;
		gunsSpawnedHard = false;
		lastPushFired = false;

        onFirstFrame = true;

		
    }
	
	// Update is called once per frame
	void Update () {

        if (onFirstFrame)
        {
            directionalShieldSpawner.setStatus(true);
            onFirstFrame = false;
        }

		if (Time.time >= nextBehaviorStartTime) {
			if (currentStage != Stage.MediumTransition && currentStage != Stage.HardTransition) {
				selectBehaviors ();
			}
		}
		else if (Time.time >= nextBehaviorEndTime) {
			deactivateBehaviors ();
			print ("deactivating behaviors");
		}

		if (currentStage == Stage.MediumTransition && Time.time > transitionEndTime) {
			currentStage = Stage.Medium;
			ToggleMovement ("Wander");
			deactivateBehaviors ();
			//minionSpawner.SpawnMinions (6,3);



			//			shieldSpawner.setStatus (true);
			//			wallGunSpawner.setStatus (false);

			soundtrackController.switchByStage (Stage.Medium);
			nextBehaviorStartTime = Time.time + behaviorDelay;
			directionalShieldSpawner.setStatus (true);

		}
		if (currentStage == Stage.HardTransition && Time.time > transitionEndTime) {
			currentStage = Stage.Hard;
			ToggleMovement ("Follow");
			soundtrackController.switchByStage (Stage.Hard);
			nextBehaviorStartTime = Time.time + behaviorDelay;
			directionalShieldSpawner.setStatus (true);
		}
        if(currentStage == Stage.Medium && !gunsSpawnedMedium)
        {
            if(healerBeaconSpawner.GetAliveBeaconCount() == 0)
            {
                print("Count of alive beacons is ZERO");
                healthIndicatorPS.Pause(); // once all the beacons in the scene are destroyed, particle system should stop playing
                healthIndicatorPS.Clear();
                gunsSpawnedMedium = true;
                wallGunSpawner.setStatus(true);
            }
        }
		if(currentStage == Stage.Hard && !gunsSpawnedHard)
		{
			if(healerBeaconSpawner.GetAliveBeaconCount() == 0)
			{
				print("Count of alive beacons is ZERO");
                healthIndicatorPS.Pause(); // once all the beacons in the scene are destroyed, particle system should stop playing
                healthIndicatorPS.Clear();
                gunsSpawnedHard = true;
				minionSpawner.SpawnMinions (6, 3);
				wallGunSpawner.setStatus(true);
			}
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

	public float GetHealthRatio(){
		//print (bossHealth.GetHealth());
		//print (bossHealth.maxHealth);
		return (float) bossHealth.GetHealth() / (float)bossHealth.maxHealth;	
	}

	void TakeDamage (int damage){

        bool isAlive = bossHealth.TakeDamage(damage);
		currentHealth = bossHealth.GetHealth ();
		print ("Boss Health is" + currentHealth);
		coreBlinkAnimation.startAnimation ();


		float healthRatio = (float)currentHealth / (float)bossHealth.maxHealth;

		print ("Health Ratio is " + healthRatio);

		if (currentStage == Stage.Easy && healthRatio < easyToMediumHealthThreshold) {
			currentStage = Stage.MediumTransition;
            myCamera.GetComponent<CameraController>().ChangeBackGround();
            healerBeaconSpawner.SpawnHealers(myCamera.GetComponent<CameraController>().timeInterval*2); // this will spawn healer beacons in sync with the blinking background
            //healthIndicatorPS.Play(); // we particle system will play as long as there are beacons alive
            ToggleMovement("Stop");
			transitionEndTime = Time.time + transitionDelay;
			soundtrackController.switchByStage(Stage.MediumTransition);

			deactivateBehaviors ();
			directionalShieldSpawner.setStatus (false);
			//wallGunSpawner.setStatus (true);
			//minionSpawner.SpawnMinions (6,3);



//			shieldSpawner.setStatus (true);
//			wallGunSpawner.setStatus (false);
//
//			wallGunSpawner.setStatus (true);
//			nextBehaviorStartTime = Time.time + mediumStageDelay;


		}

		if (currentStage == Stage.Medium && healthRatio < mediumToHardHealthThreshold) {
			currentStage = Stage.HardTransition;
            myCamera.GetComponent<CameraController>().ChangeBackGround();
            healerBeaconSpawner.SpawnHealers(myCamera.GetComponent<CameraController>().timeInterval * 2); // this will spawn healer beacons in sync with the blinking background
            //healthIndicatorPS.Play(); // we particle system will play as long as there are beacons alive
            ToggleMovement ("Stop");
            targetedDamage = 2;
			transitionEndTime = Time.time + transitionDelay;
            followBehavior.speed = followBehavior.speed + 1.0f;
			soundtrackController.switchByStage(Stage.HardTransition);

			shieldSpawner.numberShields *= 2;
			gunSpawner.numberGuns *= 2;
			targetFire.fireInterval /= 4;
			expandingCircleSpawner.numRepetitions *= 2;
			expandingCircleSpawner.delay /= 2;

			//ToggleMovement ("Follow");

			//trapSpawner.FireTrap ();
			deactivateBehaviors ();
			directionalShieldSpawner.setStatus (false);

//			shieldSpawner.setStatus (false);
//			gunSpawner.setStatus (true);
//			wallGunSpawner.setStatus (true);
			playerCircleTrapSpawner.repetitions *= 2;

			//Debug.Break ();
//			wallGunSpawner.setStatus (true);
//			minionSpawner.SpawnMinions (6, 3);
			//Debug.Break ();
			//Debug.Break ();
			//minionSpawner.SpawnMinions (12, 3);





			//nextBehaviorStartTime = Time.time + mediumStageDelay;




			// Right now, seems too difficult if the boss respawns minions and guns again
//				minionSpawner.SpawnMinions (10,3);
//				wallGunSpawner.setStatus (true);
		
		}

		if (currentStage == Stage.Hard && !lastPushFired && healthRatio < lastpushThreshold) {
			wallGunSpawner.setStatus (true);
			minionSpawner.SpawnMinions (6, 3);
			lastPushFired = true;
		}
			
		if (!isAlive) {
			//playerDeath ();
			print("Boss died!");
            healthIndicatorPS.Stop(); // particle system should be stopped when boss is dead in case beacons are still alive
			gameController.BossDied();
			Destroy (gameObject);
		}
	}

	//TODO this should be refactored to place the potential behaviors in a list and select from them.
	void selectBehaviors(){
		//directionalShieldSpawner.setStatus (true);
		currentBehaviorDuration = defaultBehaviorDuration;



		int firstBehavior = 1;
		if (currentStage == Stage.Easy) {
			firstBehavior = Random.Range (0, 3);
			switch (firstBehavior) {
			case 0:
				targetFire.setStatus (true, targetedDamage);
				break;
			case 1:
				expandingCircleSpawner.setStatus (true);
				currentBehaviorDuration = expandingCircleSpawner.getDuration ();
				break;
			case 2:
				laserSplittingAttack.SetStatus (true);
				break;
			}
		} 

		else if (currentStage == Stage.Medium && mediumStatus != "final") {
			if (mediumStatus == "initial") {
				firstBehavior = Random.Range (3, 5);
				mediumStatus = "playedOnce";
				lastMediumBehavior = firstBehavior;
			} else if (mediumStatus == "playedOnce") {
				if (lastMediumBehavior == 3) {
					firstBehavior = 4;
				}
				else{
					firstBehavior = 3;
				}
				mediumStatus = "playedTwice";
			} else if (mediumStatus == "playedTwice") {
				minionSpawner.SpawnMinions (6, 3);
				mediumStatus = "final";
				nextBehaviorEndTime = Time.time + currentBehaviorDuration;
				nextBehaviorStartTime = nextBehaviorEndTime + behaviorDelay;
				return;
			} 

		}
		else {
			firstBehavior = Random.Range (0, 5);
		}
		print ("Selected behavior is " + firstBehavior);
		switch (firstBehavior) {
		case 0:
			targetFire.setStatus (true, targetedDamage);
			break;
		case 1:
			expandingCircleSpawner.setStatus (true);
			currentBehaviorDuration = expandingCircleSpawner.getDuration ();
			break;
		case 2:
			laserSplittingAttack.SetStatus (true);
			break;
		case 3:
			playerCircleTrapSpawner.setStatus (true);
			break;
		case 4: 
			gunSpawner.setStatus (true);
			break;

		}
		if (currentStage == Stage.Hard) {
//			int secondBehavior = Random.Range (0, 5);
//			while (secondBehavior == firstBehavior){
//				secondBehavior = Random.Range (0, 5);
//			}
//			switch (secondBehavior) {
//			case 0:
//				targetFire.setStatus (true, targetedDamage);
//				break;
//			case 1:
//				expandingCircleSpawner.setStatus (true);
//				break;
//			case 2:
//				laserSplittingAttack.SetStatus (true);
//				break;
//			case 3:
//				playerCircleTrapSpawner.setStatus (true);
//				break;
//			case 4: 
//				gunSpawner.setStatus (true);
//				break;
//			}

		}

		//behaviorController.selectBehaviors (difficulty);

		nextBehaviorEndTime = Time.time + currentBehaviorDuration;
		nextBehaviorStartTime = nextBehaviorEndTime + behaviorDelay;
	}

	void deactivateBehaviors(){
		shieldSpawner.setStatus (false);
		gunSpawner.setStatus (false);
		//wallGunSpawner.setStatus (false);
		targetFire.setStatus (false, targetedDamage);
		//directionalShieldSpawner.setStatus (false);
		expandingCircleSpawner.setStatus (false);
		playerCircleTrapSpawner.setStatus (false);

		//behaviorController.inactivateBehaviors ();

		ToggleMovement ("Wander");
		nextBehaviorStartTime = Time.time + behaviorDelay;
		nextBehaviorEndTime = nextBehaviorStartTime + defaultBehaviorDuration;
	}

	void spawnTrap(){
	}

	public void ToggleMovement(string s){
		if (currentStage == Stage.Easy || currentStage == Stage.Medium) {
			if (s == "Wander") {
				followBehavior.setStatus (false);
				wanderBehavior.setStatus (true);
			}
			if (s == "Follow") {
				followBehavior.setStatus (true);
				wanderBehavior.setStatus (false);
			}
		} 
		else if (currentStage == Stage.MediumTransition || currentStage == Stage.HardTransition || s == "Stop") {
			wanderBehavior.setStatus (false);
			followBehavior.setStatus (false);
			GetComponent<Rigidbody2D> ().velocity = Vector2.zero;

		}
		else {
			followBehavior.setStatus (true);
			wanderBehavior.setStatus (false);
		}
	}
}
