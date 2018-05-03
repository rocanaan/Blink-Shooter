using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByShot : MonoBehaviour {

	private BossBattleGameController gc;

	// Use this for initialization
	void Start () {

		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<BossBattleGameController> ();
		
	}
	
	void OnTriggerEnter2D (Collider2D col){
		if (col.tag == "Shot") {
			gc.NotifyTargetDestroyed ();
			Destroy (gameObject);
		}
	}
}
