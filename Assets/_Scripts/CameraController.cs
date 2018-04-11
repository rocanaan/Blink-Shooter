using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Vector3 velocity;
    private Vector3 originalPosition;

	// Use this for initialization
	void Start () {
        originalPosition = transform.position;
        velocity = Vector3.zero;
	}

    public void CamShake(float shakeTimer, float shakeIntensity)
    {
        StartCoroutine(Shake(shakeTimer, shakeIntensity));
    }

    private IEnumerator Shake(float shakeTimer, float shakeIntensity)
    {
        while (shakeTimer > 0.0f)
        {
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeIntensity;

            shakeTimer -= Time.deltaTime;
            yield return null;
        }
        transform.position = Vector3.SmoothDamp(transform.position, originalPosition, ref velocity, 0.1f);
    }
}
