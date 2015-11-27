using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {
	public GameObject explosion;
	public GameObject playerExplosion;

	void OnTriggerEnter(Collider other){
		//on a first iteration, this was called with the Boundary and
		//thus both the asteroid and boundary were promptly destroyed
		//used Debug to figure that out, can also be seen when focusing
		//the camera on the boundary and seeing that the asteroid crosses it.
		//Debug.Log (other.name);

		if (other.CompareTag ("Boundary")) {
			return;
		}
		//destroy the asteroid
		Instantiate (explosion, transform.position, transform.rotation);

		if (other.CompareTag ("Player")) {
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
		}

		// destroy the laser bolt or player
		Destroy (other.gameObject);
		// destroy the asteroid itself
		Destroy (gameObject);
		//NOTE: these are just *marked* for destruction
		// and are destroyed at the end of the frame,
		// so the order doesn't really matter.
	}
}
