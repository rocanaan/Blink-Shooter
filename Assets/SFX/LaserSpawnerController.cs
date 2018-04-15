using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawnerController : MonoBehaviour {

	public float angularSpeed;
	public float angleRotated;
	public float preparationTime;


	private Vector3 currentDirection;
	private float arcTraveled;

	private Vector3 currentOrigin;
	private Vector3 currentTarget;

	private LineRenderer lr;

	// Use this for initialization
	void Start () {
		arcTraveled = 0.0f;
		lr = GetComponent<LineRenderer> ();
		Initialize(transform.position, transform.position + Vector3.right);

	}

	
	// Update is called once per frame
	void Update () {
		print (transform.forward);
		RaycastHit2D[] allHits = Physics2D.RaycastAll (currentOrigin, transform.right, 20.0f);
		RaycastHit2D firstWall = new RaycastHit2D ();
		float distanceFirstWall = 9999;

		print ("Number of objects hit is" + allHits.Length);

		foreach (RaycastHit2D hit in allHits) {
			Collider2D col = hit.collider;
			if (col.tag == "Wall" && (hit.distance < distanceFirstWall) ) {
				firstWall = hit;
				distanceFirstWall = hit.distance;
			}
		}
		foreach (RaycastHit2D hit in allHits) {
			Collider2D col = hit.collider;
			if (col.tag == "Player" && hit.distance < distanceFirstWall) {
				print ("player should be taking damage");
				PlayerController pc = col.GetComponent<PlayerController> ();
				pc.TakeDamage (1);
			}
		}
		lr.enabled = true;
		lr.SetPosition (0, currentOrigin);
		lr.SetPosition (1, firstWall.point);

		transform.Rotate (0, 0, angularSpeed);
	}

	public void Initialize(Vector3 origin, Vector3 target){
		currentOrigin = origin;
		currentTarget = target;
	}



}
