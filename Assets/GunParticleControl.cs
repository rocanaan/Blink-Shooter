using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunParticleControl : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ps.Play();
        StartCoroutine(AutoDestruct(ps.main.duration));
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator AutoDestruct(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
