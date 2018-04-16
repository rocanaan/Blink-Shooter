using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderBehavior : BossGenericBehavior {

	private Vector3 target;
	private Rigidbody2D rb;

	public float x_range;
	public float y_range;
	public float speed;

	public enum Dimension{both,x,y};
	public Dimension dim;

	public float offsetTolerance;
	//public float retargetTolerance;


	void Start(){
		statusActive = true;
		rb = GetComponent<Rigidbody2D> ();

		getNewTarget ();
	}
	
	// Update is called once per frame
	void Update () {
		if (statusActive) {
			Vector3 offset = (target - transform.position);
			while (offset.magnitude < offsetTolerance) {
					getNewTarget ();
					offset = (target - transform.position);
			}
			Vector3 direction = offset.normalized;
			GetComponent<Rigidbody2D> ().velocity = direction * speed;
		}

	}

	private void getNewTarget(){
		if (dim == Dimension.both) {
			target = new Vector3 (Random.Range (-x_range, x_range), Random.Range (-y_range, y_range), transform.position.z);
		} else if (dim == Dimension.x) {
			target = new Vector3 (Random.Range (-x_range, x_range), transform.position.y, transform.position.z);
		} else if (dim == Dimension.y) {
			target = new Vector3 (transform.position.x, Random.Range (-y_range, y_range), transform.position.z);
		}

	}
		
}
