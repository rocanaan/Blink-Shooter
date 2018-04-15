using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public Slider healthBar;
    public int maxHealth;
    public float singleRegenTime;

    private bool regenerate;
    private float timer;
    private bool isAlive;

	// Use this for initialization
	void Start () {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
        regenerate = false;
        isAlive = true;
	}

    public void StartRegeneration()
    {
        if (!regenerate)
        {
            regenerate = true;
            StartCoroutine(Regenerate());
        }        
    }

    public void StopRegeneration()
    {
        //if (regenerate)
        //{
        //    regenerate = false;
        //    StopCoroutine(Regenerate());
        //}
        regenerate = false;
    }

    public bool TakeDamage(int damage)
    {
        // reduces health if player is still alive
        if (isAlive)
        {
            healthBar.value = healthBar.value - damage;
        }

        if(healthBar.value <= 0)
        {
            isAlive = false;
        }
        // returns true if still alive, returns false
        return isAlive;
    }

    public void Heal(int healthRecovered)
    {
        healthBar.value = healthBar.value + healthRecovered;
    }

    private IEnumerator Regenerate()
    {
        while (regenerate)
        {
            while (timer < singleRegenTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            if(healthBar.value < maxHealth)
            {
                healthBar.value = healthBar.value + 1;
            }            
            timer = 0f;
        }
        
    }




}
