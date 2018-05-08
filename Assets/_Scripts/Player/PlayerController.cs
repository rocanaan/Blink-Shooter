using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject ghostObject;
    public int playerID;
    public int teamID;
    public float shotInterval;    
    public float timeStunned;
    public float speedStunned;
    public float gracePeriod;
    public Vector3 respawn;
    public bool isDead;
    //controls private variable, speed
    [Range(1.0f, 10.0f)]
    public float normalSpeed;
    [Range(1.0f, 10.0f)]
    public float slowedSpeed;
    public Material healthMaterial;
    public Material lowHealthMaterial;
    public bool stopWhenPhasing;

    private float speed;// Controls the speed of the character
    private SoundEffectsController sfxController;
    private BossBattleGameController gameController;
    private GhostController ghost;
    private ExplosionSpawner explosionSpawner;
	private float nextShot; // time when the next shot can be fired
	private Rigidbody2D rb;
	private Camera myCamera;
	private string controllerName;
	private float timeRecoverStun;	
	private BlinkAnimation blinkAnimation;// Script for doing the on damage blinking animation
    private HealthController playerHealth;// Script for starting and updating the healthTracker
	private bool isStunned;
	private bool respawnAllowed;
	private float nextRespawnTime;
	//private Vector3 nextRespawnLocation;
	private float timeLastDamage; // time when last damage was taken
	private FireShot fs; // script for shooting
	private Material bodyMaterial;
    private float lastFireTime;
    private bool isPhasing;

    

    // Use this for initialization
    void Start () {
		rb = GetComponent<Rigidbody2D> ();
		fs = transform.GetComponent<FireShot> ();
        blinkAnimation = GetComponent<BlinkAnimation>();
        explosionSpawner = GetComponent<ExplosionSpawner>();
        playerHealth = GetComponent<HealthController>();
        playerHealth.SetMaterial(healthMaterial);
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
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<BossBattleGameController>();

		isPhasing = false;

    }

	// Update is called once per frame
	void Update () {
		if (!GameController.IsGameOver ()) {
			if (isDead && Time.time >= nextRespawnTime && respawnAllowed) {
				//transform.parent.gameObject.SetActive (true);
				if (gameController.GetGameMode () == BossBattleGameController.GameMode.Boss) {
					print ("attempting to respawn");
					transform.position = respawn;
					rb.velocity = Vector3.zero;
					ghost.resetPosition ();
				} else {
					//TODO: Respawn code for other game modes is buggy
					Vector2 randomVector = gameController.GetRespawnPosition ();
					transform.parent.transform.position = new Vector3 (randomVector.x, randomVector.y, transform.parent.transform.position.z);
					transform.position = transform.parent.transform.position;
				}

				// player has now respawned
				isDead = false;
				playerHealth.Respawn ();
                gameController.SetRespawnText(); // the text on the screen will be updated once the player has respawned
                // still doesn't act perfectly. we should make the player press a button to respawn after some time, then this will also act perfectly accurately
                // (the reason its behavior is janky is the wait time between death and respawn)
                //(if player1 is dead and player2 dies before player1 has respawned, it will jump from 2 to 0)


			}

			if (isPhasing && stopWhenPhasing) {
				rb.velocity = Vector3.zero;
			}

			if (!isDead) {
				if (controllerName != "Keyboard" || gameController.GetCurrentKeyboardInput () == playerID) {
					getInputs ();
				}
				statUpdates ();
			}
		} else {
			rb.velocity = Vector3.zero;
		}
	}
    void statUpdates()
    {
        //slow when firing
        bool isSlowed = (Time.time - lastFireTime) < shotInterval;
        if (isSlowed)
        { speed = slowedSpeed;}
        else
        { speed = normalSpeed; }

        //account for stunned
        if (isStunned)
        {
            if (timeRecoverStun < Time.time)
            {
                isStunned = false;
                rb.velocity = Vector3.zero;
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

			if (!isPhasing || !stopWhenPhasing) {
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

					if (aimHorizontal != 0 || aimVertical != 0) { // TODO: refactor so that the code does not repeat here and on the button "Fire"
						if (Time.time >= nextShot) {
							fs.fireShot ();
							sfxController.PlayClip ("Fire");
							nextShot = Time.time + shotInterval;
							lastFireTime = Time.time;
						}
						transform.rotation = Quaternion.Euler (0.0f, 0.0f, angle);
					}
				}

				// Get fire action
				if (Input.GetButton ("Fire" + controllerName) && Time.time >= nextShot) {
					//print (" Firing shot for player " + playerID + " using controller " + controllerName);
					fs.fireShot ();
					sfxController.PlayClip ("Fire");
					nextShot = Time.time + shotInterval;
					lastFireTime = Time.time;
				}

			}

			// Get ghost action
			if(Input.GetButtonDown("Ghost" + controllerName)){
				//print (" Firing ghost for player " + playerID + " using controller " + controllerName);
				ghost.ghostAction ("Down");
			}
			if (Input.GetButtonUp ("Ghost" + controllerName)) {
				ghost.ghostAction ("Up");
			}




		}
	}

	//TODO: Implement grace period. Righ now, using OnTriggerEnter, boss can park on top of a cornered player, and using OnTriggerStay, boss insta-kills him
	void OnTriggerStay2D(Collider2D col){
        if (!isPhasing || !stopWhenPhasing)
        {
            if (col.tag == "Shot")
            {
                ShotAttributes shot = col.GetComponent<ShotAttributes>();
                if (shot.getTeamID() != teamID)
                {
                    TakeDamage(shot.damage); // takeDamage handles whether player is on grace period now
                    Destroy(col.gameObject);
                }
            }

            if (col.tag == "Pickup")
            {
                HealthPickup healthPickup = col.GetComponent<HealthPickup>();
                playerHealth.Heal(healthPickup.healthRecovered); // increases health by the given amount, doesn't go over max health
                Destroy(col.gameObject);
            }

			if (col.tag == "Boss" || col.tag == "Beacon" || col.tag == "BossShield")
            {
                if (!OnGracePeriod())
                {
                    TakeDamage(1);
                    timeLastDamage = Time.time;
                }
                if (!isDead)
                {
                    isStunned = true;
                    timeRecoverStun = Time.time + timeStunned;

                    Vector3 offset = transform.position - col.transform.position;
                    Vector3 direction = offset.normalized;
                    rb.velocity = direction * speedStunned;
                }
            }
        }        
    }

    public void TakeDamage (int damage)
    {
        if (!isDead && !OnGracePeriod() && !(isPhasing && stopWhenPhasing))
        {
            bool isAlive = playerHealth.TakeDamage(damage); // returns true if alive (health > 0)
            myCamera.GetComponent<CameraController>().CamShake(0.2f * damage, 0.15f * damage);
            explosionSpawner.SpawnExplosion(damage);

            if (playerHealth.GetHealth() <= Mathf.RoundToInt(playerHealth.maxHealth / 3)) // if low on health, change the colour of the healtbar
            {
                playerHealth.SetMaterial(lowHealthMaterial);
            }

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

        Debug.Log("PlayerDeath() called for player " + playerID);
		transform.position = new Vector3(30,30,0);
		ghost.resetPosition ();
		rb.velocity = Vector3.zero;
		isDead = true;
		SetRespawn();
		sfxController.PlayClip ("Death");
        gameController.NotifyDeath(playerID, teamID);
    }

	void SetRespawn(){
		int respawnDelay = gameController.GetRespawnTime ();
		if (respawnDelay >= 0) {
			nextRespawnTime = Time.time + 3.0f;
			respawnAllowed = true;
		} else {
			respawnAllowed = false;
		}
        
	}

	bool OnGracePeriod(){
		return (Time.time <= (timeLastDamage + gracePeriod));
	}

    public bool IsPhasing()
    {
        return isPhasing;
    }

    public void SetPhasing(bool phasing)
    {
        isPhasing = phasing;
    }

}