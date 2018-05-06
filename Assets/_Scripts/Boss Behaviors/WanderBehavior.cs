using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderBehavior : BossGenericBehavior {

	private Vector3 target;
	private Rigidbody2D rb;

	public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
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
			if (offset.magnitude < offsetTolerance) {
					getNewTarget ();
					offset = (target - transform.position);
			}

            if(offset.magnitude > 0)
            {
                Vector3 direction = offset.normalized;
                GetComponent<Rigidbody2D>().velocity = direction * speed;
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
			
		}

	}

	public void getNewTarget(){
		if (dim == Dimension.both) {
			target = new Vector3 (Random.Range (xMin, xMax), Random.Range (yMin, yMax), transform.position.z);
		} else if (dim == Dimension.x) {
			target = new Vector3 (Random.Range (xMin, xMax), transform.position.y, transform.position.z);
		} else if (dim == Dimension.y) {
			target = new Vector3 (transform.position.x, Random.Range (yMin, yMax), transform.position.z);
		}

	}


		
}
