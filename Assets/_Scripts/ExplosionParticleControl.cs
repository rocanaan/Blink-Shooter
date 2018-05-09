using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticleControl : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ps.Play();
        StartCoroutine(AutoDestruct(ps.main.duration));
    }

    IEnumerator AutoDestruct(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
