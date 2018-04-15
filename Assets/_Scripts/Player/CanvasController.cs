using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour {

    public GameObject playerBody;

    private Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = transform.position - playerBody.transform.position;

	}
	
	// Update is called once per frame
	void Update () {
		transform.position = playerBody.transform.position + offset;
    }
}
