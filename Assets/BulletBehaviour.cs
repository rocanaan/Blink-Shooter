using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    private ParticleSystem ps;
    private Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        ps = transform.GetComponent<ParticleSystem>();
        rb = transform.GetComponent<Rigidbody2D>();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("Wall"))
        //{
        //    ps.Play();
        //    Destroy(gameObject, ps.main.duration);
        //}
        ps.Play();
        Destroy(gameObject, ps.main.duration);
    }
}
