using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHealthBarController : MonoBehaviour {

    public GameObject playerBody;
    public Image fill;
    public Material highHealthMaterial;
    public Material lowHealthMaterial;

    private HealthController playerHealthController;
    private Slider healthBar;
    private bool healthLow;

	// Use this for initialization
	void Start () {
        playerHealthController = playerBody.GetComponent<HealthController>();
        healthBar = transform.GetComponent<Slider>();

        healthBar.maxValue = playerHealthController.maxHealth;
        healthBar.value = playerHealthController.healthBar.value;
        healthLow = false;
        fill.color = highHealthMaterial.color;
    }
	
	// Update is called once per frame
	void Update () {
        healthBar.value = playerHealthController.healthBar.value;
        //fill.color = playerHealthController.fill.color;
        if (healthBar.value <= Mathf.RoundToInt(healthBar.maxValue / 3)) // if regenerating above the "danger" threshhold, health bar goes green
        {
            //start blinking
            healthLow = true;
            StartCoroutine(Blink());
        }
        else
        {
            healthLow = false;
            StopCoroutine(Blink());
            fill.color = highHealthMaterial.color;
        }
    }

    private IEnumerator Blink()
    {
        float timer = 0f;
        while (healthLow)
        {
            fill.color = lowHealthMaterial.color;
            yield return null;

            while(timer < 0.2f)
            {
                timer = timer + Time.deltaTime;
                yield return null;
            }
            timer = 0f;

            fill.color = highHealthMaterial.color;
            yield return null;

            while (timer < 0.2f)
            {
                timer = timer + Time.deltaTime;
                yield return null;
            }
            timer = 0f;
        }
    }
}
