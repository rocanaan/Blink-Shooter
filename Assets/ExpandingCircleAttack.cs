using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingCircleAttack : MonoBehaviour {


	public int numberOfVertices;
	public float startingRadius;
	public float finalRadius;
	public float radiusStep;
	public float collisionWidth;
	private float currentRadius;

	public float preparationDuration;
	public bool followDuringPreparation;
	public bool followDuringExpansion;
	public bool damageDuringPreparation;

	private float startOfExpansionTime;

	private Vector3 center;

	public int damage;

	private LineRenderer l_Renderer;

	private GameObject[] players;
	private BossBattleGameController gameController;

	public Material activationMaterial;



	void Start()
	{
		l_Renderer = GetComponent<LineRenderer>();
		l_Renderer.SetVertexCount(numberOfVertices+1);
		currentRadius = startingRadius;

		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<BossBattleGameController> ();
		players = gameController.GetAllPlayers ();

		center = transform.position;
		startOfExpansionTime = Time.time + preparationDuration;
	}

	void CreatePoints()
	{
		float x ;
		float y ;
		float z = 0;
		float currentAngle = 0;
		float degreesPerSegment = 360 / numberOfVertices;
		Vector3 firstPoint = Vector3.zero;
		for (int i  = 0;i < numberOfVertices; i++)
		{
			x=Mathf.Sin(Mathf.Deg2Rad*currentAngle);
			y=Mathf.Cos(Mathf.Deg2Rad*currentAngle);
			//l_Renderer.SetPosition(i,Vector3(x,y,z)*radialScale);
			Vector3 newPoint = center + (new Vector3(x,y,z)*currentRadius);
			if (i == 0) {
				firstPoint = newPoint;
			}
			l_Renderer.SetPosition(i, newPoint);
			currentAngle += degreesPerSegment;
		}
		l_Renderer.SetPosition(numberOfVertices, firstPoint);

	}

	void Update()
	{
		// If on follow behavior, update the center before creating the circle
		if ( (Time.time >= startOfExpansionTime && followDuringExpansion) ||
			(Time.time < startOfExpansionTime && followDuringPreparation) )
		{
			center = transform.position;
		}

		// Create the circle 
		CreatePoints();


		// Damage all players in range, if on damate mode)
		if (Time.time >= startOfExpansionTime || damageDuringPreparation) {
			foreach (GameObject player in players) {
				float dist = Vector3.Distance (player.transform.position, center);
				if (dist > (currentRadius - collisionWidth) && dist < (currentRadius + collisionWidth)) {
					PlayerController pc = player.GetComponent<PlayerController> ();
					pc.TakeDamage (damage);
//					print ("Current position " + center);
//					print ("Player position " + player.transform.position);
//					print ("Distance to player " + pc.playerID + " is " + dist);
//					print ("Current radius is " + currentRadius);
//					Debug.Break ();
				}
			}
		}

		// Expand or contract the circle after preparation
		if (Time.time >= startOfExpansionTime) {
			l_Renderer.material = activationMaterial;
			currentRadius += radiusStep;
		}

		// If absolute difference between current and starting raduis is bigger
		// than different betwen starting and final radius, destroy the script
		if (Mathf.Abs(currentRadius-startingRadius) > Mathf.Abs(startingRadius-finalRadius)){
			Destroy (this.gameObject);
		}

	}


}
