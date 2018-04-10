using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkAnimation : MonoBehaviour {

    public float blinkTime;

    private bool isBlinking;
    private float timeStopBlinking;
    private SpriteRenderer body;

    // Use this for initialization
    void Start()
    {
        isBlinking = false;
        timeStopBlinking = 0.0f;

        body = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBlinking && Time.time > timeStopBlinking)
        {
            body.enabled = true;
            isBlinking = false;
        }
    }

    public void startAnimation()
    {
        isBlinking = true;
        body.enabled = false;
        timeStopBlinking = Time.time + blinkTime;
    }
}
