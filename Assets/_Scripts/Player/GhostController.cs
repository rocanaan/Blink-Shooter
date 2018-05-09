using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GhostController : MonoBehaviour {


	public float angularSpeed;
	public float moveSpeed;
	public GameObject body;

	public float cooldown;
	public float earlyActivationDuration;
	public float blinkOnActivationDuration;
	public Material blinkMaterial;
	private float nextActivation;

	private PlayerController pc;
	private CircleCollider2D col;
	private SpriteRenderer spriteRenderer;

	public bool teleportOnRepeatPress;
	public bool shortDashOnEarlyRelease;

	public float earlyReleaseInterval;
	private float lastLaunchTime;

	private SoundEffectsController sfx;

	private Material defaultMaterial;

	private Vector3 defaultLocalScale;

	public float blinkScaleMultiplier;



	// TO DO: See if a better implementation is to use an enum or #define
	private int state;

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		//rb.angularVelocity = angularSpeed;
		col = GetComponent<CircleCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		defaultLocalScale = transform.localScale;

		pc = body.GetComponent<PlayerController> ();




		state = 0;
		col.enabled = false;

		nextActivation = 0.0f;

		sfx = GetComponent<SoundEffectsController> ();
	}

	// Update is called once per frame
	void Update () {
		if (state == 0 || state == 2) {
			transform.position = new Vector3 (body.transform.position.x, body.transform.position.y, transform.position.z);
		}
		if (state == 2 && Time.time >= nextActivation) {
			stateTransition (0);
			transform.localScale = defaultLocalScale;
		} else if (state == 2 && !pc.isDead && Time.time >= nextActivation - blinkOnActivationDuration) {
			spriteRenderer.enabled = true;
			transform.localScale = defaultLocalScale*blinkScaleMultiplier;
		}
		else if (state == 2 && !pc.isDead && Time.time >= nextActivation - earlyActivationDuration) {
			spriteRenderer.enabled = true;
		}
	}


	// This function will launch the ghost on a button press down and attempt to teleport on one of two conditions"
	// a) A second  "down" input
	// b) A release ("Up" command) of the button after timeIgnoreRelease has passed (to avoid accidentally teleporting on first release of the button)
	public void ghostAction (string command)
	{
		if (command == "Down") {
			if (state == 0) {
				Vector3 direction = pc.getFireDirection ();
				rb.velocity = direction * moveSpeed;
				stateTransition (1);
				pc.SetPhasing(true);
				lastLaunchTime = Time.time;
			} else if (state == 1 && teleportOnRepeatPress) {
				StartCoroutine (TryPhasing (Time.time));
			}
		}

		else if (command == "Up") {
			if (state == 1) {
				if (Time.time > lastLaunchTime + earlyReleaseInterval) {
					StartCoroutine (TryPhasing (Time.time));
				} else if (shortDashOnEarlyRelease) {
					StartCoroutine (TryPhasing (lastLaunchTime + earlyReleaseInterval));
				}
			}
        }
	}

	private IEnumerator TryPhasing(float timeStartPhase)
    {
        while(state == 1)
        {
			if (Time.time > timeStartPhase) {
				Vector3 velo = rb.velocity;
				rb.velocity = Vector3.zero;
				Vector2 pos = new Vector2 (transform.position.x, transform.position.y);
				Collider2D[] colliders = Physics2D.OverlapCircleAll (pos, 0.25f);
				bool collidesWithObjects = false;
				foreach (Collider2D col in colliders) {
					if (!col.gameObject.CompareTag ("Shot") && !col.gameObject.CompareTag ("Ghost") && !col.gameObject.CompareTag ("Laser") && transform.IsChildOf(col.transform) /*!col.gameObject.CompareTag("Player")*/) {
						collidesWithObjects = true;
					}
				}

				if (!collidesWithObjects) {
					rb.velocity = Vector3.zero;
					body.transform.position = new Vector3 (transform.position.x, transform.position.y, body.transform.position.z);
					stateTransition (2);
					pc.SetPhasing(false);
					yield return null;
				} else {
					rb.velocity = velo;
					yield return null;
				}
			} else {
				yield return null;
			}
        }
    }

	public void collidesWithBoundary(){
		if (state == 1) {
			rb.velocity = Vector3.zero;
			stateTransition (2);
			pc.SetPhasing(false);
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
			sfx.PlayClip ("Travel");
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
			sfx.StopClip ();
			sfx.PlayClip ("Teleport");
		}
	}
	public void resetPosition(){
		transform.position = new Vector3 (body.transform.position.x, body.transform.position.y, transform.position.z);
	}

}