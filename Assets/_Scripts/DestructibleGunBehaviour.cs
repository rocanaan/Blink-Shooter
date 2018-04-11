using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleGunBehaviour : MonoBehaviour {

    public int health = 3;

    private FireShot fs;
    private ParticleSystem ps;
    private BlinkAnimation blinkAnimation;
    private GameObject cam;

    // Use this for initialization
    void Start () {
        fs = GetComponent<FireShot>();
        ps = GetComponent<ParticleSystem>();
        blinkAnimation = GetComponent<BlinkAnimation>();

        cam = GameObject.FindWithTag("MainCamera");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Shot"))
        {
            ShotAttributes shot = col.GetComponent<ShotAttributes>();
            if (shot.getTeamID() != fs.teamID)
            {
                health = health - 1;
                Destroy(col.gameObject);
                if(health != 0)
                {
                    blinkAnimation.startAnimation();
                }                
            }

            if(health == 0)
            {
                cam.GetComponent<CameraController>().CamShake(0.2f, 0.15f);
                col.GetComponent<SpriteRenderer>().enabled = false;
                ps.Play();
                Destroy(gameObject, ps.main.duration);
                
            }
        }
    }
}
