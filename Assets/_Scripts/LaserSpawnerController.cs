﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawnerController : MonoBehaviour {

    public int laserDamage;
	public float angularSpeed;
	public float angleRotated;
	public float preparationTime;
	private float activationTime;

	public Material activationMaterial;

	public Vector3 target;


	private Vector3 currentDirection;
	private float arcTraveled;

	private Vector3 currentOrigin;
	private Vector3 currentTarget;

	private LineRenderer lr;

    
    void Awake()
    {
    }
    void Start () {
		arcTraveled = 0.0f;
		lr = GetComponent<LineRenderer> ();
    }
		
	
	// Update is called once per frame
	void Update () {
        // If has already traveled its arc, destroy itself
        if (Mathf.Abs (arcTraveled) >= angleRotated) {
			Destroy (this.gameObject);
		} else { 
			// If it is still during prepatation time, keep aiming at original target 
			if (Time.time <= activationTime) {
				transform.right = currentTarget - transform.position;
			} 

			// This part is executed regardless of preparation time or no
			// Do a raycast from position to current target (transform.right)
			//print (transform.forward);
			RaycastHit2D[] allHits = Physics2D.RaycastAll (transform.position, transform.right, 60.0f);
			RaycastHit2D firstWall = new RaycastHit2D ();
			float distanceFirstWall = 9999;

			//print ("Number of objects hit is" + allHits.Length);
			// Locate first wall
			foreach (RaycastHit2D hit in allHits) {
				Collider2D col = hit.collider;
				if (col.tag == "Wall" && (hit.distance < distanceFirstWall)) {
					firstWall = hit;
					distanceFirstWall = hit.distance;
				}
			}
			// Draw a line from position to first wall
			lr.enabled = true;
			lr.SetPosition (0, transform.position);
			lr.SetPosition (1, firstWall.point);

			// If preparation time is over, damage players and rotate;
			if (Time.time > activationTime) {
				lr.material = activationMaterial;
				foreach (RaycastHit2D hit in allHits) {
					Collider2D col = hit.collider;
					if (col.tag == "Player" && hit.distance < distanceFirstWall) {
						print ("player should be taking damage");
						PlayerController pc = col.GetComponent<PlayerController> ();
						pc.TakeDamage (laserDamage);
					}
				}


				transform.Rotate (0, 0, angularSpeed);
				arcTraveled += angularSpeed;
			}

		}
	}

    public void Initialize(){
		//currentOrigin = origin;
		activationTime = Time.time + preparationTime;
		currentTarget = target;
		transform.right = target - transform.position;
	}



}
