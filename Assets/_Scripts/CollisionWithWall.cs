using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWithWall : MonoBehaviour {

	public bool ghostTrespassable;
	public int teamID;
	
	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Shot" && col.GetComponent<ShotAttributes>().getTeamID() != teamID) {
            // TO DO: really figure out what is the best way to structure the rigidbody and collider of the shot, wither in the parent shot or in the child bullet,
            // especially if I decide to work with reflections
            //ShotAttributes shot = col.GetComponent<ShotAttributes> ();
            //if (shot.getTeamID() != teamID && !shot.energy) {
            //	print ("Shot with ID " + shot.getTeamID () + " collided with wall with team ID " + teamID);
            //	Destroy (col.gameObject);
            //}
            Destroy(col.gameObject);
        }
		if (col.tag == "Ghost" && !ghostTrespassable) {
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            Vector2 incomingDir = rb.velocity;
            rb.velocity = Vector2.zero;
            rb.transform.Translate(incomingDir.normalized * (-0.5f)); // move ghost in the opposite direction slightly so that it doesn't collide with the wall anymore
			GhostController ghostController = col.GetComponent<GhostController> ();
            //ghostController.collidesWithBoundary ();
            ghostController.ghostAction("Up"); // since the ghost is now clear from the wall, it can finish its job
		}
	}
}
