﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GhostController : MonoBehaviour {


	public float angularSpeed;
	public float moveSpeed;
	public GameObject body;

	public float cooldown;
	private float nextActivation;

	private PlayerController pc;
	private CircleCollider2D col;
	private SpriteRenderer spriteRenderer;

	public float timeIgnoreRelease;
	private float lastLaunchTime;



	// TO DO: See if a better implementation is to use an enum or #define
	private int state;

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		//rb.angularVelocity = angularSpeed;
		col = GetComponent<CircleCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer> ();

		pc = body.GetComponent<PlayerController> ();


		state = 0;
		col.enabled = false;

		nextActivation = 0.0f;
	}

	// Update is called once per frame
	void Update () {
		if (state == 0 || state == 2) {
			transform.position = new Vector3 (body.transform.position.x, body.transform.position.y, transform.position.z);
		}
		if (state == 2 && Time.time >= nextActivation) {
			stateTransition (0);
		}
	}


	// This function will launch the ghost on a button press down and attempt to teleport on one of two conditions"
	// a) A second  "down" input
	// b) A release ("Up" command) of the button after timeIgnoreRelease has passed (to avoid accidentally teleporting on first release of the button)
	public void ghostAction (string command)
	{
		if (state == 0 && command == "Down"){
			Vector3 direction = pc.getFireDirection ();
			rb.velocity = direction * moveSpeed;
			stateTransition (1);
			lastLaunchTime = Time.time;
		}
		else if (state == 1) {
			if (command == "Down" || Time.time > lastLaunchTime + timeIgnoreRelease) {
				StartCoroutine (TryPhasing ());
			}
        }
	}

    private IEnumerator TryPhasing()
    {
        while(state == 1)
        {
            Vector3 velo = rb.velocity;
            rb.velocity = Vector3.zero;
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, 0.5f);
            bool collidesWithObjects = false;
            foreach (Collider2D col in colliders)
            {
                if (!col.gameObject.CompareTag("Shot") && !col.gameObject.CompareTag("Ghost") && !col.gameObject.CompareTag("Laser"))
                {
                    collidesWithObjects = true;
                }
            }

            if (!collidesWithObjects)
            {
                rb.velocity = Vector3.zero;
                body.transform.position = new Vector3(transform.position.x, transform.position.y, body.transform.position.z);
                stateTransition(2);
                yield return null;
            }
            else
            {
                rb.velocity = velo;
                yield return null;
            }
        }
    }

	public void collidesWithBoundary(){
		if (state == 1) {
			rb.velocity = Vector3.zero;
			stateTransition (2);
		}


	}

	void stateTransition(int nextState){
		if (nextState == 0) {
			state = 0;
			col.enabled = false;
			spriteRenderer.enabled = true;
		}
		if (nextState == 1) {
			state = 1;
			col.enabled = true;
			spriteRenderer.enabled = true;
		}
		if (nextState == 2) {
			if (cooldown > 0.0f) {
				state = 2;
				col.enabled = false;
				spriteRenderer.enabled = false;
				nextActivation = Time.time + cooldown;
			} else {
				stateTransition (0);
			}
		}
	}
	public void resetPosition(){
		transform.position = new Vector3 (body.transform.position.x, body.transform.position.y, transform.position.z);
	}

}