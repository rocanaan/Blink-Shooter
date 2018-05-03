using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByShot : MonoBehaviour {

	private GameController gc;

	// Use this for initialization
	void Start () {

		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		
	}
	
	void OnTriggerEnter2D (Collider2D col){
		if (col.tag == "Shot") {
			gc.NotifyTargetDestroyed ();
			Destroy (gameObject);
		}
	}
}
