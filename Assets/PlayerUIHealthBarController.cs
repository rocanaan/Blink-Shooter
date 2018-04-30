using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHealthBarController : MonoBehaviour {

    public GameObject playerBody;
    public Image fill;

    private HealthController playerHealthController;
    private Slider healthBar;

	// Use this for initialization
	void Start () {
        playerHealthController = playerBody.GetComponent<HealthController>();
        healthBar = transform.GetComponent<Slider>();

        healthBar.maxValue = playerHealthController.maxHealth;
        healthBar.value = playerHealthController.healthBar.value;
    }
	
	// Update is called once per frame
	void Update () {
        healthBar.value = playerHealthController.healthBar.value;
        fill.color = playerHealthController.fill.color;
    }
}
