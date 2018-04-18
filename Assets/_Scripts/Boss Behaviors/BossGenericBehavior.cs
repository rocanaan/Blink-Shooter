using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGenericBehavior : MonoBehaviour {

	protected bool statusActive;

	// Use this for initialization
	void Start () {
		statusActive = false;
	}

	public virtual void setStatus(bool x){
		statusActive = x;
	}

	public virtual bool isActive(){
		return statusActive;
	}

}
