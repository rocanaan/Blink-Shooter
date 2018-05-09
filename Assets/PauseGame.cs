using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Pause"))
        {
            if(Time.timeScale == 1.0F)
            {
                Time.timeScale = 0.0F;
            }else { Time.timeScale = 1.0F; }
        }
	}

}
