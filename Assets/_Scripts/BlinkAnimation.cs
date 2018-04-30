using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkAnimation : MonoBehaviour {

    public float blinkTime;
	public float waitTime;

    private bool isBlinking;
    private float timeStopBlinking;
	private float timeAbleToBlink;
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
        }
		if (isBlinking && Time.time > timeAbleToBlink)
		{
			isBlinking = false;
		}
    }

    public void startAnimation()
    {
		if (!isBlinking) {
			isBlinking = true;
			body.enabled = false;
			timeStopBlinking = Time.time + blinkTime;
			timeAbleToBlink = timeStopBlinking + waitTime;
		}
    }
}
