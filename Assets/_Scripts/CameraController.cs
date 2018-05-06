using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Material phase1Material;
    public Material phase2Material;
    public Material phase3Material;
    public int repetition;
    public float timeInterval;


    private Vector3 velocity;
    private Vector3 originalPosition;
    private Camera cam;

	// Use this for initialization
	void Start () {
        originalPosition = transform.position;
        velocity = Vector3.zero;
        cam = transform.GetComponent<Camera>();
        cam.backgroundColor = phase1Material.color;
    }

    public void CamShake(float shakeTimer, float shakeIntensity)
    {
        StartCoroutine(Shake(shakeTimer, shakeIntensity));
    }

    public void ChangeBackGround(int phaseTransition)
    {
        if(phaseTransition == 1)
        {
            StartCoroutine(BlinkingBackground(phase1Material, phase2Material));
        }
        else if(phaseTransition == 2)
        {
            StartCoroutine(BlinkingBackground(phase2Material, phase3Material));
        }
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

    private IEnumerator BlinkingBackground(Material mat1, Material mat2)
    {
        //float timer = 0f;
        for (int i=0; i<repetition; i++)
        {            
            cam.backgroundColor = mat2.color;
            yield return new WaitForSeconds(timeInterval);

            cam.backgroundColor = mat1.color;
            yield return new WaitForSeconds(timeInterval);
        }

        cam.backgroundColor = mat2.color;
    }
}
