using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingCircleSpawner : MonoBehaviour {

	public GameObject expandingAttackPrefab;



	public void setStatus(bool active){
		if (active) {
			GameObject attack = Instantiate (expandingAttackPrefab, transform);
		}
	}
		
}
