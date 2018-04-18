using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangesColor : MonoBehaviour {

	public GameObject bossObject;
	private BossController bossControler;
	private Renderer renderer;

	// Use this for initialization
	void Start () {
		bossControler = bossObject.GetComponent<BossController> ();
		renderer = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		renderer.material = bossControler.getCurrentMaterial ();
	}
}
