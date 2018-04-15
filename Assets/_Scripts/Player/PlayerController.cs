using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject ghostObject;
    public int playerID;
    public int teamID;
    public float speed;// Controls the speed of the character
    public float shotInterval;    
    public float timeStunned;
    public float speedStunned;
    public float gracePeriod;
    public Vector3 respawn;
    public bool isDead;

    private SoundEffectsController sfxController;
    private GameController gameController;
    private GhostController ghost;
	private float nextShot; // time when the next shot can be fired
	private Rigidbody2D rb;
	private Camera myCamera;
	private string controllerName;
	private float timeRecoverStun;	
	private BlinkAnimation blinkAnimation;// Script for doing the on damage blinking animation
    private PlayerHealth playerHealth;// Script for starting and updating the healthTracker
	private bool isStunned;
	private bool respawnAllowed;
	private float nextRespawnTime;
	//private Vector3 nextRespawnLocation;
	private float timeLastDamage; // time when last damage was taken
	private FireShot fs; // script for shooting
	private Material bodyMaterial;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		fs = transform.GetComponent<FireShot> ();
        blinkAnimation = GetComponent<BlinkAnimation>();
        playerHealth = GetComponent<PlayerHealth>();
        bodyMaterial = transform.GetComponent<Renderer>().material;
        ghost = ghostObject.GetComponent<GhostController>();
        GameObject cameraObject = GameObject.FindGameObjectWithTag ("MainCamera");
		myCamera = cameraObject.GetComponent<Camera> ();        

		controllerName = "";
		getControllerName ();
		nextShot = 0.0f;
		isDead = false;
		isStunned = false;
		timeRecoverStun = 0.0f;
		timeLastDamage = -10;

		sfxController = GetComponentInParent<SoundEffectsController> ();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

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

			// Get rotation input
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
                TakeDamage(shot.damage); // takeDamage handles whether player is on grace period now
				Destroy (col.gameObject);
			}

		}

		if (col.tag == "Pickup") {
			HealthPickup healthPickup = col.GetComponent<HealthPickup> ();
            playerHealth.Heal(healthPickup.healthRecovered); // increases health by the given amount, doesn't go over max health
            Destroy(col.gameObject);
		}

		if (col.tag == "Boss"  && !OnGracePeriod()) {
			TakeDamage (1);
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

	public void TakeDamage (int damage)
    {
        if (!OnGracePeriod())
        {
            bool isAlive = playerHealth.TakeDamage(damage); // returns true if alive (health > 0)

            if (isAlive)
            {
                sfxController.PlayClip("Hit");
                blinkAnimation.startAnimation();
                timeLastDamage = Time.time;
                
            }
            else
            {
                PlayerDeath();
            }
        }

	}

	void PlayerDeath (){
		gameController.notifyDeath (playerID, teamID);
		//Destroy (transform.parent.gameObject);
		//transform.parent.gameObject.SetActive(false);
		transform.position = new Vector3(30,30,0);
		ghost.resetPosition ();
		rb.velocity = Vector3.zero;
		isDead = true;
		SetRespawn();
		sfxController.PlayClip ("Death");
	}

	void SetRespawn(){
		int respawnDelay = gameController.getRespawnTime ();
		if (respawnDelay >= 0) {
			nextRespawnTime = Time.time + 3.0f;
			respawnAllowed = true;
		} else {
			respawnAllowed = false;
		}

		//transform.parent.transform.position = new Vector3 (Random.Range (-9, 9), Random.Range (-5, 5), transform.position.z);
	}

	bool OnGracePeriod(){
		return (Time.time <= (timeLastDamage + gracePeriod));
	}

}