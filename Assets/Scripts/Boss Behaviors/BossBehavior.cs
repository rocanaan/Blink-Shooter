using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour {

	protected bool statusActive;

	// Use this for initialization
	void Start () {
		statusActive = false;
	}

	public void setActive(bool x){
		statusActive = x;
	}

	public bool isActive(){
		return statusActive;
	}

}
