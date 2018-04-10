using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPosition : MonoBehaviour {

	GameObject left;
	GameObject right;
	GameObject bottom;
	GameObject top;
	GameObject middleStarting;
	GameObject middlePermanent;
	GameObject target;

	// Use this for initialization
	void Start () {
		findChildren ();
		middlePermanent.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void findChildren(){
		left = transform.Find ("Left").gameObject;
		right = GameObject.Find ("Right").gameObject;
		bottom = transform.Find ("Bottom").gameObject;
		top = transform.Find ("Top").gameObject;
		middleStarting = transform.Find ("MiddleStarting").gameObject;
		middlePermanent = transform.Find ("MiddlePermanent").gameObject;
		target = transform.Find ("Target").gameObject;


	}

	void targetEntered(){
		middlePermanent.SetActive (true);
		Destroy (middleStarting.gameObject);
		Destroy (target.gameObject);
	}

	void endTutorial(){
		print ("Tutorial ended - starting position");
		Destroy (left.gameObject);
		Destroy (right.gameObject);
		Destroy (top.gameObject);
		Destroy (bottom.gameObject);
	}

}
