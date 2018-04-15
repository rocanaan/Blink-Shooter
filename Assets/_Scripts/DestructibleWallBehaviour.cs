﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWallBehaviour : MonoBehaviour {

    public GameObject explosion;
    public int health = 3;

    private GameObject cam;
    private BlinkAnimation blinkAnimation;
    private SoundEffectsController globalSFXController;

    // Use this for initialization
    void Start () {
        blinkAnimation = GetComponent<BlinkAnimation>();
        cam = GameObject.FindWithTag("MainCamera");
        //globalSFXController = GameObject.FindGameObjectWithTag("GlobalAudioSource").GetComponent<SoundEffectsController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shot"))
        {
            health = health - 1;
            Destroy(collision.gameObject);
            if (health != 0)
            {
                //blinkAnimation.startAnimation();
            }

            if (health == 0)
            {
                cam.GetComponent<CameraController>().CamShake(0.2f, 0.15f);
                Instantiate(explosion, transform.position, transform.rotation);
                Destroy(gameObject);
                //globalSFXController.PlayClip("WallGunExplosion");
            }
        }
    }
}