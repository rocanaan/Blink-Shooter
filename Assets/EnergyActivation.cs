using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyActivation : MonoBehaviour {

	public Material activationMaterial;
	private Vector3 target;
	public float waitDelay;
	public float lifeDuration;
	private float launchTime;
	private float destroyTime;
	public float speed;

	private bool prepared;
	private bool fired;

	void Start(){
		prepared = false;
		fired = false;
	}

	void Update(){
		if (prepared && !fired && Time.time > launchTime) {
			FireEnergy ();
		}
		if (fired && Time.time > destroyTime) {
			Destroy (this);
		}
	}

	// Use this for initialization
	public void Prepare(Vector3 t){
		transform.parent = null;
		target = t;
		GetComponent<Renderer> ().material = activationMaterial;
		GetComponent<CircleCollider2D> ().enabled = true;
		launchTime = Time.time + waitDelay;
		destroyTime = Time.time + lifeDuration;
		prepared = true;
	}

	private void FireEnergy(){
		fired = true;
		Vector3 direction = (target - transform.position);
		direction.Normalize();
		Vector3 velocity = direction*speed;
		GetComponent<Rigidbody2D>().velocity = velocity;
	}
		
}
