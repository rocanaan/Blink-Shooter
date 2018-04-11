using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private SoundEffectsController sfxController;

	// Controls the speed of the character
	public float speed;

	public GameObject ghostObject;
	private GhostController ghost;

	public GameObject shot;
	public float shotSpeed;
	public float shotInterval;
	private float nextShot;

	private Rigidbody2D rb;
	private Camera myCamera;

	public int playerID;
	public int teamID;
	private string controllerName;

	public int maxHealth;
	private int currentHealth;

	public float timeStunned;
	private float timeRecoverStun;
	public float speedStunned;

	private GameController gameController;

	// Script for doing the on damage blinking animation
	private BlinkAnimation blinkAnimation;

	// Script for starting and updating the healthTracker
	private HealthTracker healthTracker;

	public bool isDead;
	private bool isStunned;
	private bool respawnAllowed;
	private float nextRespawnTime;
	//private Vector3 nextRespawnLocation;

	public float gracePeriod;
	private float timeLastDamage;

	public Vector3 respawn;
	private FireShot fs;

	private Material bodyMaterial;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		fs = transform.GetComponent<FireShot> ();
		GameObject cameraObject = GameObject.FindGameObjectWithTag ("MainCamera");
		myCamera = cameraObject.GetComponent<Camera> ();

		bodyMaterial = transform.GetComponent<Renderer> ().material;

		ghost = ghostObject.GetComponent<GhostController> ();

		controllerName = "";
		getControllerName ();

		nextShot = 0.0f;

		currentHealth = maxHealth;

		blinkAnimation = GetComponent<BlinkAnimation> ();

		healthTracker = GetComponent<HealthTracker> ();
		healthTracker.startHealth (maxHealth);

		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();

		isDead = false;
		isStunned = false;
		timeRecoverStun = 0.0f;

		timeLastDamage = -10;

		sfxController = GetComponentInParent<SoundEffectsController> ();


	}

	// Update is called once per frame
	void Update () {
		if (!GameController.isGameOver ()) {
			if (isDead && Time.time >= nextRespawnTime && respawnAllowed) {
				//transform.parent.gameObject.SetActive (true);
				if (gameController.gameMode == GameController.GameMode.Boss) {
					print ("attempting to respawn");
					transform.position = respawn;
					rb.velocity = Vector3.zero;
					ghost.resetPosition ();
				} else {
					//TODO: Respawn code for other game modes is buggy
					Vector2 randomVector = gameController.getRespawnPosition ();
					transform.parent.transform.position = new Vector3 (randomVector.x, randomVector.y, transform.parent.transform.position.z);
					transform.position = transform.parent.transform.position;
				}
				currentHealth = maxHealth;
				healthTracker.setHealth (currentHealth);
				isDead = false;
				// problem: this never gets called because the script is innactive
				// right way to do it would probably be to leave the game controller responsible for that, passing the player's game object
				// probably a good idea to go ahead and refactor the player scripts
			}

			if (isStunned) {
				if (timeRecoverStun < Time.time) {
					isStunned = false;
					rb.velocity = Vector3.zero;
				}
			}
			if (!isDead) {
				if (controllerName != "Keyboard" || gameController.getCurrentKeyboardInput () == playerID) {
					getInputs ();
				}
			}
		}



	}

	public Vector3 getFireDirection(){
		float angle = Mathf.Deg2Rad * transform.rotation.eulerAngles.z;
		Vector3 direction = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0.0f);
		return direction;
	}

	// TODO: refactor this method into a new script so that boss can also shoot
	//	private void fireShot (){
	//		Vector3 direction = getFireDirection ();
	//		float angle = transform.rotation.eulerAngles.z;
	//		GameObject newShot = Instantiate(shot, transform.Find("Weapon").transform.position, Quaternion.Euler( new Vector3(0.0f, 0.0f, angle)));
	//		ShotAttributes shotAtt = newShot.GetComponent<ShotAttributes> ();
	//		shotAtt.setTeamID (teamID);
	//		shotAtt.setPlayerID (playerID);
	//		newShot.GetComponent<TrailRenderer>().material = bodyMaterial; // better approach then having 4 different prefabs for same object with different colours
	//		Rigidbody2D shotRigidbody2D = newShot.GetComponent<Rigidbody2D> ();
	//		shotRigidbody2D.velocity = direction * shotSpeed;
	//		pac.PlaySound ("Fire");
	//	}

	public void getControllerName (){
		int numControllers = Input.GetJoystickNames().Length;
		print ("Checking controllers for player " + playerID + ". Number of controllers is " + numControllers);

		foreach (string name in Input.GetJoystickNames()) {
			print (name);
		}

		if (numControllers >= playerID) {
			controllerName = "Joystick" + playerID;
		}
		else if (numControllers < playerID) {
			controllerName = "Keyboard";
		} else {
			print("ERROR: No input found for player " + playerID);
		}

		print ("Controller name for player " + playerID + " is " + controllerName);

	}

	public void getInputs(){
		if (controllerName == "") {
			print ("ERROR: No input found for player " + playerID);
		} else {

			if (!isStunned) {
				// Get movement inputs. Prone to moving faster in diagonal direction
				float horizontalMovement = Input.GetAxis ("Horizontal" + controllerName);
				float verticallMovement = Input.GetAxis ("Vertical" + controllerName);
				rb.velocity = new Vector2 (speed * horizontalMovement, speed * verticallMovement);

			}

			//			// Get rotation input
			float angle;
			if (controllerName == "Keyboard") {
				// For keyboard, rotate to face mouse position
				Vector3 mousePosition = myCamera.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0.0f));
				angle = Mathf.Atan2 ((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x)) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler (0.0f, 0.0f, angle);
			} else {
				// Controls for aiming with the right directional wheel in the PS4 Controller. 
				float aimHorizontal = Input.GetAxis ("AimHorizontal" + controllerName);
				float aimVertical = Input.GetAxis ("AimVertical" + controllerName); // * -1 

				angle = Mathf.Atan2 (aimVertical, aimHorizontal) * Mathf.Rad2Deg;

				if (aimHorizontal != 0 || aimVertical != 0) {
					transform.rotation = Quaternion.Euler (0.0f, 0.0f, angle);
				}
			}


			// Get ghost action
			if(Input.GetButtonDown("Ghost" + controllerName)){
				//print (" Firing ghost for player " + playerID + " using controller " + controllerName);
				ghost.ghostAction ();
			}

			// Get fire action
			if(Input.GetButton("Fire" + controllerName) && Time.time >= nextShot){
				//print (" Firing shot for player " + playerID + " using controller " + controllerName);
				//fireShot ();
				fs.fireShot();
				sfxController.PlayClip ("Fire");
				nextShot = Time.time + shotInterval;
			}


		}
	}

	//TODO: Implement grace period. Righ now, using OnTriggerEnter, boss can park on top of a cornered player, and using OnTriggerStay, boss insta-kills him
	void OnTriggerStay2D(Collider2D col){
		if (col.tag == "Shot") {
			ShotAttributes shot = col.GetComponent<ShotAttributes> ();
			if (shot.getTeamID() != teamID ) {
				if (!onGracePeriod ()) {
					takeDamage (shot.damage);
					if (!isDead) {
						timeLastDamage = Time.time;
					}
				}
				Destroy (col.gameObject);
			}

		}

		if (col.tag == "Pickup") {
			HealthPickup healthPickup = col.GetComponent<HealthPickup> ();
			currentHealth += healthPickup.healthRecovered;
			if (currentHealth > maxHealth) {
				currentHealth = maxHealth;
			}
			healthTracker.setHealth (currentHealth);
			Destroy (col.gameObject);
		}

		if (col.tag == "Boss"  && !onGracePeriod()) {
			takeDamage (1);
			if (!isDead) {
				isStunned = true;
				timeRecoverStun = Time.time + timeStunned;

				Vector3 offset = transform.position - col.transform.position;
				Vector3 direction = offset.normalized;
				rb.velocity = direction * speedStunned;

				timeLastDamage = Time.time;
			}




		}
	}

	void takeDamage (int damage){
		currentHealth -= damage;
		print (" Player " + playerID + " current Health is " + currentHealth);
		blinkAnimation.startAnimation ();
		healthTracker.setHealth (currentHealth);

		if (currentHealth <= 0) {
			playerDeath ();
		} else {
			sfxController.PlayClip ("Hit");
			blinkAnimation.startAnimation ();
		}
	}

	void playerDeath (){
		gameController.notifyDeath (playerID, teamID);
		//Destroy (transform.parent.gameObject);
		//transform.parent.gameObject.SetActive(false);
		transform.position = new Vector3(30,30,0);
		ghost.resetPosition ();
		rb.velocity = Vector3.zero;
		isDead = true;
		setRespawn();
		sfxController.PlayClip ("Death");
	}

	void setRespawn(){
		int respawnDelay = gameController.getRespawnTime ();
		if (respawnDelay >= 0) {
			nextRespawnTime = Time.time + 3.0f;
			respawnAllowed = true;
		} else {
			respawnAllowed = false;
		}

		//transform.parent.transform.position = new Vector3 (Random.Range (-9, 9), Random.Range (-5, 5), transform.position.z);
	}

	bool onGracePeriod(){
		return (Time.time <= (timeLastDamage + gracePeriod));
	}

}