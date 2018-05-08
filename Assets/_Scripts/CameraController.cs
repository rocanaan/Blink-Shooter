using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Material regularMaterial;
    public Material secondaryMaterial;
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
        cam.backgroundColor = regularMaterial.color;
    }

    public void CamShake(float shakeTimer, float shakeIntensity)
    {
        StartCoroutine(Shake(shakeTimer, shakeIntensity));
    }

    public void ChangeBackGround()
    {
        StartCoroutine(BlinkingBackground(regularMaterial, secondaryMaterial));
    }

    private IEnumerator Shake(float shakeTimer, float shakeIntensity)
    {
        while (shakeTimer > 0.0f)
        {
			transform.localPosition = originalPosition + Time.timeScale * Random.insideUnitSphere * shakeIntensity;

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
    }
}
