using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleGunBehaviour : MonoBehaviour {
    public GameObject gunParticle;
    public int health = 3;

    private FireShot fs;
    private BlinkAnimation blinkAnimation;
    private GameObject cam;

	private SoundEffectsController globalSFXController;

    // Use this for initialization
    void Start () {
        fs = GetComponent<FireShot>();
        blinkAnimation = GetComponent<BlinkAnimation>();

        cam = GameObject.FindWithTag("MainCamera");

		globalSFXController = GameObject.FindGameObjectWithTag ("GlobalAudioSource").GetComponent<SoundEffectsController> ();
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
                Instantiate(gunParticle, transform.position, transform.rotation);
                Destroy(gameObject);
				globalSFXController.PlayClip ("WallGunExplosion");
            }
        }
    }
}
