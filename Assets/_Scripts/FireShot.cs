using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShot : MonoBehaviour {

	public GameObject shot;
	public int teamID;
	public int playerID;
	public float offset;
	public Material mat;

	public float shotSpeed;


	public void fireShot (){
		Vector3 direction = getFireDirection ();
		float angle = transform.rotation.eulerAngles.z;
		GameObject newShot = Instantiate(shot,  transform.position + getFireDirection()*offset, Quaternion.Euler( new Vector3(0.0f, 0.0f, angle)));
		ShotAttributes shotAtt = newShot.GetComponent<ShotAttributes> ();
		shotAtt.setTeamID (teamID);
		shotAtt.setPlayerID (playerID);
		newShot.GetComponent<TrailRenderer>().material = mat; // better approach than having 4 different prefabs for same object with different colours
        newShot.GetComponent<ExplosionSpawner>().explosionMaterial = mat;
		Rigidbody2D shotRigidbody2D = newShot.GetComponent<Rigidbody2D> ();
		shotRigidbody2D.velocity = direction * shotSpeed;
	}

	private Vector3 getFireDirection(){
		float angle = Mathf.Deg2Rad * transform.rotation.eulerAngles.z;
		Vector3 direction = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0.0f);
		return direction;
	}
		
}
