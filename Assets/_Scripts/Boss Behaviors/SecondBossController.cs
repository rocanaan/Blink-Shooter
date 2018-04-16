using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBossController : MonoBehaviour {
    // Phases to be implemented:
    // Phase 1: Boss is idle, wall cannons spawn
    // TRANSITION 1: After cannons are all dead, if the boss is at 2/3(?) health within 5(?) seconds, boss wakes up, exposes weak spot
    // Phase 2: Boss speed is 5, wandersfor 5 seconds, then follows closer player, targets farther player for 10 seconds
    // TRANSITION 2: Once boss is at 1/3 health, returns to origin and starts very slowly regenerating
    // Phase 3: While boss is slowly regenerating, it spawns minions
    // TRANSITION 3: Once minions are dead, boss wakes up, exposes weak spot
    // Phase 4: Boss speed is 6, wanders for 4, follows farther player, targets closer player for 10 seconds
    // Final: Once boss is at 1/10 health, returns to origin, rotates and shoots


    private BlinkAnimation blinkAnimation;
    private int maxHealth;
    private PlayerHealth bossHealth;    
    private enum Difficulty { Phase_1, Transition_1, Phase_2, Transition_2, Phase_3, Transition_3, Phase_4};
    private Difficulty currentDifficulty;

    // Use this for initialization
    void Start()
    {
        blinkAnimation = transform.GetComponent<BlinkAnimation>();
        bossHealth = transform.GetComponent<PlayerHealth>();
        maxHealth = bossHealth.maxHealth;

        currentDifficulty = Difficulty.Phase_1;
    }

    // Update is called once per frame
    void Update()
    {

    }



    void TakeDamage(int damage)
    {
        blinkAnimation.startAnimation();
        bossHealth.TakeDamage(damage);

        int currentHealth = bossHealth.GetHealth();

        // TODO: implement difficulty transitions that are based on health level

        if (currentHealth <= 0)
        {
            //playerDeath ();
            print("Boss died!");
            //gameController.bossDied();
            Destroy(gameObject);
        }
    }



}
