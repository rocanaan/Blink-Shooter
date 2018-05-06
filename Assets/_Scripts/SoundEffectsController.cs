using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This class assumes the object it is in has an AudioSource component and that the clip will be played as "one shot"
// It does not work if you need a clip to loop or to be cut off 
// (for example, a sound that should only exist while a certain object is on the screen and then terminate, such as the trap sound)
// Possible solutions to cut-off sound effects :
//     a) create a new lass to handle specific effects that need to cut-off
//     b) implement the method PlayCliffCutoff - allows whoever is calling to control how long the sound should run for
//     c) add the cutoffInSeconds  to the clipLabelPair class. This allows us to handle the cutoff from the editor (but not from the object calling)
// I think solution b is the best. For example, if we change the preparation time of the trap, we want the clip to also cutoff at the same point, so we just call playClipCutoff passing preparation time)

public class SoundEffectsController : MonoBehaviour {


	[System.Serializable]
	public class clipLabelPair
	{
		public AudioClip clip;
		public string label;
		// public int cutoffInSeconds
	}//a pair of an audio clip and a label

	public clipLabelPair[] clips;

	private AudioSource audioSource;

	// Use this for initialization
	void Awake () {
		audioSource = GetComponentInParent<AudioSource> ();
	}
	
	public void PlayClip(string label){
		foreach (clipLabelPair clp in clips) {
			if (clp.label == label) {
				audioSource.PlayOneShot(clp.clip);
				return;
			}
		}
		return;
	}

	public void StopClip(){
		audioSource.Stop ();
	}

	public void PlayClipCuttoff(string label, float time){
		//TODO: instantiate a clip and then keep track of it (using Update) to see when it should be cut-off
	}
}
