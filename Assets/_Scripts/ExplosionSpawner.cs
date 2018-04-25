using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSpawner : MonoBehaviour {

    public GameObject explosion;
    public float explosionSize;
    public float explosionDuration;
    public float explosionRadius;
    public Color explosionColor;
    public Material explosionMaterial;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        ParticleSystem newExplosion = Instantiate(explosion, transform.position, transform.rotation).GetComponent<ParticleSystem>();
        var main = newExplosion.main;
        main.startSize = explosionSize;
        main.duration = explosionDuration;
        main.startLifetime = explosionDuration;
        newExplosion.GetComponent<Renderer>().material = explosionMaterial;
        //main.startColor = explosionColor;
        var shape = newExplosion.shape;
        shape.radius = explosionRadius;
    }
}
