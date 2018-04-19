using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGunSpawner : MonoBehaviour {


	public GameObject gunObj;
	public float x_boundary;
	public float y_boundary;
	public int gunsPerWall;

	private bool statusActive;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setStatus(bool status){
		statusActive = status;
		if (status) {
			for (int i = 0; i < gunsPerWall; i++) {
				GameObject wallGun;
				//left wall
				wallGun = Instantiate (gunObj, new Vector3 (-x_boundary, Random.Range (-y_boundary, y_boundary), transform.position.z), Quaternion.identity);
				wallGun.GetComponent<WanderBehavior> ().dim = WanderBehavior.Dimension.y;
				// bottom wall
				wallGun = Instantiate (gunObj, new Vector3 (Random.Range (-x_boundary, x_boundary), -y_boundary, transform.position.z), Quaternion.Euler (new Vector3 (0, 0, 90)));
				wallGun.GetComponent<WanderBehavior> ().dim = WanderBehavior.Dimension.x;
				// right wall
				wallGun = Instantiate (gunObj, new Vector3 (x_boundary, Random.Range (-y_boundary, y_boundary), transform.position.z), Quaternion.Euler (new Vector3 (0, 0, 180)));
				wallGun.GetComponent<WanderBehavior> ().dim = WanderBehavior.Dimension.y;
				// top wall
				wallGun = Instantiate (gunObj, new Vector3 (Random.Range (-x_boundary, x_boundary), y_boundary, transform.position.z), Quaternion.Euler (new Vector3 (0, 0, 270)));
				wallGun.GetComponent<WanderBehavior> ().dim = WanderBehavior.Dimension.x;
			}
		}
	}
}
