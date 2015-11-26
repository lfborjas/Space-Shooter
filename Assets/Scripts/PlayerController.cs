using UnityEngine;
using System.Collections;


/*
	If the class isn't serialized, it won't be visible in the inspector
 */

[System.Serializable]
public class Boundary{
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {
	private Rigidbody _rigidbody;

	// public vars can be set from the properties in Unity itself
	public float speed, tilt;
	public Boundary boundary;

	void Start(){
		// gotta use GetComponent here because unity 5 no longer has the "shorthand"
		// getters
		_rigidbody = GetComponent<Rigidbody> ();
	}

	// called just before each fixed physics step
	void FixedUpdate (){
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		// remember: x -> left/right, y -> up/down, z -> forward/back
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		// by default the movement has a speed of 1
		_rigidbody.velocity = movement * speed;

		// limit the movement in each physics action, keep it within
		// the boundaries in X and Z
		_rigidbody.position = new Vector3 (
			// keep any given value between a min/max
			Mathf.Clamp(_rigidbody.position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp (_rigidbody.position.z, boundary.zMin, boundary.zMax)
		);

		// set the tilt of the ship every time it moves
		_rigidbody.rotation = Quaternion.Euler (0.0f, 0.0f, _rigidbody.velocity.x * -tilt);
	}
}
