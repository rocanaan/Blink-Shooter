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
	private MinionSpawner minionSpawner;
	private ExpandingCircleSpawner expandingCircleSpawner;
	private LaserSplittingAttack laserSplittingAttack; // Note: have to fix nomeclature. For circle, the prefab that just instantiates the attack is attack, and the controller is spawner. Here, attack is the controller and controller is attack
	private PlayerCircleTrapSpawner playerCircleTrapSpawner;
    private GameController gameController;

	private GameObject audioSource;


	// Use this for initialization
	void Start () {
		coreBlinkAnimation = GetComponentInChildren<BlinkAnimation> ();
        bossHealth = transform.GetComponent<HealthController>();
		currentHealth = bossHealth.GetHealth();
		difficulty = 0;
        bossHealth.SetMaterial(startingDifficultyMaterial);

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
		laserSplittingAttack = GetComponentInChildren<LaserSplittingAttack> ();
		playerCircleTrapSpawner = GetComponentInChildren<PlayerCircleTrapSpawner> ();



		wallGunSpawner.setStatus (true);

		nextBehaviorStartTime = Time.time + behaviorDelay;
		nextBehaviorEndTime = nextBehaviorStartTime + behaviorDuration;
//		targetFire.setStatus (true);
//		wallGunSpawner.setStatus (true);

		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();

		audioSource = GameObject.FindGameObjectWithTag ("GlobalAudioSource");
		sfxController = audioSource.GetComponent<SoundEffectsController> ();

		behaviorController = GetComponent<BehaviorController> ();
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
		currentHealth = bossHealth.GetHealth ();
		print ("Boss Health is" + currentHealth);
		coreBlinkAnimation.startAnimation ();


		float healthRatio = (float)currentHealth / (float)bossHealth.maxHealth;

		print ("Health Ratio is " + healthRatio);

		if (difficulty == 0 && healthRatio < 0.6) {
			difficulty = 1;
            bossHealth.SetMaterial(difficulty1Material);
			GetComponentsInChildren<Renderer> () [1].material = difficulty1Material;

			deactivateBehaviors ();
			minionSpawner.SpawnMinions (6,3);



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

			ToggleMovement ("Follow");

			//trapSpawner.FireTrap ();
			deactivateBehaviors ();

//			shieldSpawner.setStatus (false);
//			gunSpawner.setStatus (true);
			wallGunSpawner.gunsPerWall = 2;
//			wallGunSpawner.setStatus (true);




			// Right now, seems too difficult if the boss respawns minions and guns again
//				minionSpawner.SpawnMinions (10,3);
//				wallGunSpawner.setStatus (true);
		
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
		directionalShieldSpawner.setStatus (true);

		int firstBehavior = 1;
		if (difficulty == 0) {
			firstBehavior = Random.Range (0, 3);
		}
		else {
			firstBehavior = Random.Range (0, 5);
		}
		print ("Selected behavior is " + firstBehavior);
		switch (firstBehavior) {
		case 0:
			targetFire.setStatus (true);
			break;
		case 1:
			expandingCircleSpawner.setStatus (true);
			break;
		case 2:
			laserSplittingAttack.setStatus (true);
			break;
		case 3:
			playerCircleTrapSpawner.setStatus (true);
			break;
		case 4: 
			gunSpawner.setStatus (true);
			break;

		}
		if (difficulty > 0) {
			int secondBehavior = Random.Range (0, 5);
			while (secondBehavior == firstBehavior){
				secondBehavior = Random.Range (0, 5);
			}
			switch (secondBehavior) {
			case 0:
				targetFire.setStatus (true);
				break;
			case 1:
				expandingCircleSpawner.setStatus (true);
				break;
			case 2:
				laserSplittingAttack.setStatus (true);
				break;
			case 3:
				playerCircleTrapSpawner.setStatus (true);
				break;
			case 4: 
				gunSpawner.setStatus (true);
				break;
			}

		}

		//behaviorController.selectBehaviors (difficulty);

		nextBehaviorEndTime = Time.time + behaviorDuration;
		nextBehaviorStartTime = nextBehaviorEndTime + behaviorDelay;
	}

	void deactivateBehaviors(){
		shieldSpawner.setStatus (false);
		gunSpawner.setStatus (false);
		//wallGunSpawner.setStatus (false);
		targetFire.setStatus (false);
		directionalShieldSpawner.setStatus (false);
		expandingCircleSpawner.setStatus (false);
		playerCircleTrapSpawner.setStatus (false);

		//behaviorController.inactivateBehaviors ();

		ToggleMovement ("Wander");
		nextBehaviorStartTime = Time.time + behaviorDelay;
		nextBehaviorEndTime = nextBehaviorStartTime + behaviorDuration;
	}

	void spawnTrap(){
	}

	public void ToggleMovement(string s){
		if (difficulty != 2) {
			if (s == "Wander") {
				followBehavior.setStatus (false);
				wanderBehavior.setStatus (true);
			}
			if (s == "Follow") {
				followBehavior.setStatus (true);
				wanderBehavior.setStatus (false);
			}
		} else {
			followBehavior.setStatus (true);
			wanderBehavior.setStatus (false);
		}
	}
}
