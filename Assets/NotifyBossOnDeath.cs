using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyBossOnDeath : MonoBehaviour {

	private DirectionalShieldSpawner shieldSpawner;

	// Use this for initialization
	void Start () {
		shieldSpawner = GetComponentInParent<DirectionalShieldSpawner>();
	}
	
	// Update is called once per frame
	public void OnDestroy(){
		shieldSpawner.NotifyShieldDestroyed ();
	}
}
