using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawner : MonoBehaviour {

	public float offsetAsRadiusRatio;
	public float angularSpeed;
	public GameObject gunObject;

	public float fireInterval;

	private List<GameObject> gunList;

	public int numberGuns;


	private bool statusActive;

	// Use this for initialization
	void Start () {
		statusActive = false;

		gunList = new List<GameObject>() ;
	}

	// Update is called once per frame
	void Update () {
		if (statusActive) {
			transform.Rotate (0, 0, angularSpeed);
		}
	}

	public void setStatus(bool status){
		statusActive = status;
		if (statusActive) {
			for (int i = 0; i < numberGuns; i++) {
				float angle = 2*Mathf.PI * i / numberGuns;
				Vector3 direction = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0);
				//print (direction);
				Vector3 shieldPosition = transform.TransformPoint (direction * offsetAsRadiusRatio / 2.0f);
				GameObject gun = Instantiate<GameObject> (gunObject, shieldPosition, Quaternion.Euler(0,0, Mathf.Rad2Deg*angle));
				gun.transform.localScale *= transform.lossyScale.x;
				gun.transform.parent = transform;
				gunList.Add (gun);
			}
		}
		if (!statusActive) {
			foreach (GameObject gun in gunList){
				Destroy (gun);
			}
			transform.rotation = Quaternion.identity;
		}
	}

//	public void SpawnGuns(){
//		for (int i = 0; i < numberGuns; i++) {
//			float angle = 2*Mathf.PI * i / numberGuns;
//			Vector3 direction = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0);
//			print (direction);
//			Vector3 shieldPosition = transform.TransformPoint (direction * offsetAsRadiusRatio / 2.0f);
//			GameObject gun = Instantiate<GameObject> (gunObject, shieldPosition, Quaternion.Euler(0,0, Mathf.Rad2Deg*angle));
//			gun.transform.localScale *= transform.lossyScale.x;
//			gun.transform.parent = transform;
//			gunList.Add (gun);
//		}
//	}
//
//	public void DestroyGuns(){
//		foreach (GameObject gun in gunList){
//			Destroy (gun);
//		}
//	}
}
