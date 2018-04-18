using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorController : MonoBehaviour {

	public GameObject[] easyBehaviors;
	public GameObject[] mediumBehaviors;
	public GameObject[] hardBehaviors;

	public int[] numBehaviorsPerDifficulty;


	private List<BossGenericBehavior> activeBehaviorList;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void selectBehaviors(int difficulty){
		List<GameObject> currentBehaviorCandidates = new List<GameObject> ();
		if (difficulty == 0) {
			foreach (GameObject obj  in easyBehaviors) {
				currentBehaviorCandidates.Add (obj);
			}

		} else if (difficulty == 1) {
			foreach (GameObject obj  in mediumBehaviors) {
				currentBehaviorCandidates.Add (obj);
			}
		} else if (difficulty == 2) {
			foreach (GameObject obj  in hardBehaviors) {
				currentBehaviorCandidates.Add (obj);
			}
		}

		randomizeBehaviors (currentBehaviorCandidates.ToArray(), difficulty);
	}

	public void randomizeBehaviors (GameObject[] candidates, int difficulty){
		//activeBehaviorList = new List<BossGenericBehavior> ();
		print ("Entering randomize behaviors");
		print (candidates);
		print (candidates.Length);
		print (candidates[0]);
		List<int> randomList = new List<int> ();

		for (int i = 0; i<numBehaviorsPerDifficulty[difficulty];i++){
			int numToAdd = Random.Range(0,candidates.Length);
			while(randomList.Contains(numToAdd)){
					numToAdd = Random.Range(0,candidates.Length);
			}
			randomList.Add(numToAdd);
			print ("Selecting behavior number " + numToAdd);
			Debug.Break ();
			candidates [numToAdd].GetComponent<BossGenericBehavior> ().setStatus (true);
			activeBehaviorList.Add (candidates[numToAdd].GetComponent<BossGenericBehavior>());
			print ("Size of list is" + activeBehaviorList.Count);
		}
		
	}

	public void inactivateBehaviors(){
		foreach (BossGenericBehavior behavior in activeBehaviorList){
			behavior.setStatus (false);
		}
		activeBehaviorList = new List<BossGenericBehavior> ();
	}


}
