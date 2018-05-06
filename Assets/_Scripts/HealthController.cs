using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour {

    public Slider healthBar;
    public Image fill;
    public int maxHealth;
    public float singleRegenTime;
    public float regenAmount;
    private bool regenerate;
    private float timer;
    private bool isAlive;


	public Material highHealthMaterial;
    public bool checkForHealthThreshold;

	// Use this for initialization
	void Start () {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
        regenerate = false;
        isAlive = true;
	}

    public void StartRegeneration()
    {
        if (!regenerate && isAlive)
        {
            regenerate = true;
            StartCoroutine(Regenerate());
        }        
    }

    public void StopRegeneration()
    {
        if (regenerate)
        {
            regenerate = false;
            StopCoroutine(Regenerate());
        }
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
        if (checkForHealthThreshold)
        {
            if (healthBar.value > Mathf.RoundToInt(healthBar.maxValue / 3)) // if regenerating above the "danger" threshhold, health bar goes green
            {
                SetMaterial(highHealthMaterial);
            }
        }
    }

    public int GetHealth()
    {
        return (int)healthBar.value;
    }

    private IEnumerator Regenerate()
    {
        while (regenerate && isAlive)
        {
            while (regenerate && timer < singleRegenTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            if(timer >= singleRegenTime) // means regeneration was not interrupted
            {
                if (healthBar.value < maxHealth)
                {
                    healthBar.value = healthBar.value + regenAmount;
                }
                if (healthBar.value > Mathf.RoundToInt(healthBar.maxValue / 3)) // if regenerating above the "danger" threshhold, health bar goes green
                {
                    SetMaterial(highHealthMaterial);
                }                
            }

            timer = 0f;
        }        
    }

    public void Respawn()
    {
        if (!isAlive)
        {
            isAlive = true;
            healthBar.value = maxHealth;
            SetMaterial(highHealthMaterial);
        }
    }

    public void SetMaterial(Material mat)
    {
        //fill.material = mat;
        fill.color = mat.color;
    }

    public bool IsAlive()
    {
        return isAlive;
    }

}
