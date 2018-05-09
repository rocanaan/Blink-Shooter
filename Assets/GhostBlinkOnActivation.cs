using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBlinkOnActivation : MonoBehaviour {

	public float startDelay;
	public float blinkDelay;
	public float totalDuration;
	public Material blinkMaterial;

	private float nextStartTime;
	private float nextBlinkTime;
	private float nextEndTime;
	private Material normalMaterial;
	private SpriteRenderer sprite;

	private bool isActive;

	// Use this for initialization
	void Start () {
		isActive = false;
		sprite = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive) {
			if (Time.time > nextEndTime) {
				isActive = false;
				sprite.enabled = true;
				sprite.material = normalMaterial;
			} else if (Time.time > nextBlinkTime) {
				sprite.enabled = true;
				sprite.material = blinkMaterial;
			} else if (Time.time >nextStartTime) {
				sprite.enabled = true;
				sprite.material = normalMaterial;
			}
		}
	}

	public void ScheduleBlinkOnActivation(){
		isActive = true;
		nextStartTime = Time.time + startDelay;
		nextBlinkTime = Time.time + blinkDelay;
		nextEndTime = Time.time + totalDuration;
		normalMaterial = sprite.material;
	}
}
