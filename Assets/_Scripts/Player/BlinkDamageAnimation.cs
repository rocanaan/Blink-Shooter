using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkDamageAnimation : MonoBehaviour {

	public float blinkTime;
	private bool isBlinking;

	private float timeStopBlinking;

	private MeshRenderer body;
	private MeshRenderer weapon;

	// Use this for initialization
	void Start () {
		isBlinking = false;
		timeStopBlinking = 0.0f;

		body = GetComponent<MeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isBlinking && Time.time > timeStopBlinking) {
			body.enabled = true;
			isBlinking = false;
		}
	}

	public void startAnimation(){
		isBlinking = true;
		body.enabled = false;
		timeStopBlinking = Time.time + blinkTime;
	}

}
