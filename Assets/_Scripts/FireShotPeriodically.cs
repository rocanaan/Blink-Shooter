using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShotPeriodically : MonoBehaviour {

	public GameObject shot;
	public int teamID;
	public int playerID;

	public float shotSpeed;

	public float fireInterval;
	private float nextFire;

	// Use this for initialization
	void Start () {
		nextFire = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= nextFire) {
			fireShot ();
			nextFire += fireInterval;
		}
	}


	public Vector3 getFireDirection(){
		float angle = Mathf.Deg2Rad * transform.rotation.eulerAngles.z;
		Vector3 direction = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0.0f);
		return direction;
	}

	// TODO: refactor this method into a new script so that boss can also shoot
	private void fireShot (){
		Vector3 direction = getFireDirection ();
		float angle = transform.rotation.eulerAngles.z;
		GameObject newShot = Instantiate(shot,  transform.position, Quaternion.Euler( new Vector3(0.0f, 0.0f, angle)));
		ShotAttributes shotAtt = newShot.GetComponent<ShotAttributes> ();
		shotAtt.setTeamID (teamID);
		shotAtt.setPlayerID (playerID);
		Rigidbody2D shotRigidbody2D = newShot.GetComponent<Rigidbody2D> ();
		shotRigidbody2D.velocity = direction * shotSpeed;
	}
}
