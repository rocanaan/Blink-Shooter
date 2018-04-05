using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerEntersTarget : MonoBehaviour {


	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			gameObject.SendMessageUpwards("targetEntered");
		}
	
			

	}
}
