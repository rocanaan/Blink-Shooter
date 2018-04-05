using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpawner : MonoBehaviour {

	public float offsetAsRadiusRatio;
	public float angularSpeed;
	public GameObject shieldObject;

	private List<GameObject> shieldList;

	public int numberShields;


	private bool statusActive;

	// Use this for initialization
	void Start () {
		statusActive = false;

		shieldList = new List<GameObject>() ;
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
			for (int i = 0; i < numberShields; i++) {
				float angle = 2*Mathf.PI * i / numberShields;
				Vector3 direction = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0);
				print (direction);
				Vector3 shieldPosition = transform.TransformPoint (direction * offsetAsRadiusRatio / 2.0f);
				GameObject shield = Instantiate<GameObject> (shieldObject, shieldPosition, Quaternion.Euler(0,0, Mathf.Rad2Deg*angle));
				shield.transform.localScale *= 3;//transform.lossyScale.x;
				shield.transform.parent = transform;
				shieldList.Add (shield);
				shield.GetComponent<Renderer> ().material = GetComponentInParent<BossController> ().getCurrentMaterial();
			}
		}
		if (!statusActive) {
			foreach (GameObject shield in shieldList){
				Destroy (shield);
			}
			transform.rotation = Quaternion.identity;
		}
	}


}
