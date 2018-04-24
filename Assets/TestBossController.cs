using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossController : MonoBehaviour {
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
    private TargettedFireBehavior targetFire;
    private HealthController bossHealth;
    private ExpandingCircleSpawner expandingCircleSpawner;
    private LaserSplit laserSplittingAttack; // Note: have to fix nomeclature. For circle, the prefab that just instantiates the attack is attack, and the controller is spawner. Here, attack is the controller and controller is attack
    private GameController gameController;

    private GameObject audioSource;


    // Use this for initialization
    void Start()
    {
        coreBlinkAnimation = GetComponentInChildren<BlinkAnimation>();
        bossHealth = transform.GetComponent<HealthController>();
        currentHealth = bossHealth.GetHealth();
        difficulty = 0;
        bossHealth.SetMaterial(startingDifficultyMaterial);

        wanderBehavior = GetComponent<WanderBehavior>();
        followBehavior = GetComponent<FollowBehavior>();
        ToggleMovement("Wander");
        targetFire = GetComponent<TargettedFireBehavior>();
        expandingCircleSpawner = GetComponentInChildren<ExpandingCircleSpawner>();
        laserSplittingAttack = GetComponentInChildren<LaserSplit>();



        nextBehaviorStartTime = Time.time + behaviorDelay;
        nextBehaviorEndTime = nextBehaviorStartTime + behaviorDuration;

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        audioSource = GameObject.FindGameObjectWithTag("GlobalAudioSource");
        sfxController = audioSource.GetComponent<SoundEffectsController>();

        behaviorController = GetComponent<BehaviorController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextBehaviorStartTime)
        {
            selectBehaviors();
        }
        else if (Time.time >= nextBehaviorEndTime)
        {
            deactivateBehaviors();
            print("deactivating behaviors");
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Shot")
        {
            ShotAttributes shot = col.GetComponent<ShotAttributes>();
            if (shot.getTeamID() != teamID)
            {
                TakeDamage(shot.damage);
                Destroy(col.gameObject);
            }
        }
    }

    void TakeDamage(int damage)
    {

        bool isAlive = bossHealth.TakeDamage(damage);
        currentHealth = bossHealth.GetHealth();
        print("Boss Health is" + currentHealth);
        coreBlinkAnimation.startAnimation();


        float healthRatio = (float)currentHealth / (float)bossHealth.maxHealth;

        print("Health Ratio is " + healthRatio);

        if (difficulty == 0 && healthRatio < 0.6)
        {
            difficulty = 1;
            bossHealth.SetMaterial(difficulty1Material);
            GetComponentsInChildren<Renderer>()[1].material = difficulty1Material;

            deactivateBehaviors();
        }

        if (difficulty == 1 && healthRatio < 0.2)
        {
            difficulty = 2;
            bossHealth.SetMaterial(difficulty2Material);
            GetComponentsInChildren<Renderer>()[1].material = difficulty2Material;
            targetFire.fireInterval /= 2;

            ToggleMovement("Follow");
            deactivateBehaviors();

        }

        if (!isAlive)
        {
            //playerDeath ();
            print("Boss died!");
            gameController.bossDied();
            Destroy(gameObject);
        }
    }

    //TODO: refactor this into an array of materials
    public Material getCurrentMaterial()
    {
        if (difficulty == 0)
        {
            return startingDifficultyMaterial;
        }
        if (difficulty == 1)
        {
            return difficulty1Material;
        }
        if (difficulty == 2)
        {
            return difficulty2Material;
        }
        return null;
    }

    //TODO this should be refactored to place the potential behaviors in a list and select from them.
    void selectBehaviors()
    {
        laserSplittingAttack.SetStatus(true);

        //behaviorController.selectBehaviors (difficulty);

        nextBehaviorEndTime = Time.time + behaviorDuration;
        nextBehaviorStartTime = nextBehaviorEndTime + behaviorDelay;
    }

    void deactivateBehaviors()
    {
        //wallGunSpawner.setStatus (false);
        targetFire.setStatus(false);
        expandingCircleSpawner.setStatus(false);

        //behaviorController.inactivateBehaviors ();

        ToggleMovement("Wander");
        nextBehaviorStartTime = Time.time + behaviorDelay;
        nextBehaviorEndTime = nextBehaviorStartTime + behaviorDuration;
    }

    public void ToggleMovement(string s)
    {
        if (difficulty != 2)
        {
            if (s == "Wander")
            {
                followBehavior.setStatus(false);
                wanderBehavior.setStatus(true);
            }
            if (s == "Follow")
            {
                followBehavior.setStatus(true);
                wanderBehavior.setStatus(false);
            }
        }
        else
        {
            followBehavior.setStatus(true);
            wanderBehavior.setStatus(false);
        }
    }
}
