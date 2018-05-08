using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreResize : MonoBehaviour {

	public GameObject bossObject;
	private BossController bossController;

	public float startingRelativeSize;
	public float finalRelativeSize;

	// Use this for initialization
	void Start () {
		bossController = bossObject.GetComponent<BossController> ();
	}
	
	// Update is called once per frame
	void Update () {
		Resize ();
	}

	void Resize (){
		float scale = startingRelativeSize + (1-bossController.GetHealthRatio ()) * (finalRelativeSize - startingRelativeSize);
		//print ("Health ratio" + bossController.GetHealthRatio());
		transform.localScale = new Vector3 (scale, scale, 1);
	}
}
