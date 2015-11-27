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
	private float nextFire = 0.0f;

	// public vars can be set from the properties in Unity itself
	public float speed, tilt;
	public Boundary boundary;

	public GameObject shot;
	//if we declare this as a transform, Unity will be smart enough to reach into
	//the referenced game object and use its transform, without having to do spawn.transform
	public Transform shotSpawn;
	public float fireRate = 0.5F;


	void Start(){
		// gotta use GetComponent here because unity 5 no longer has the "shorthand"
		// getters
		_rigidbody = GetComponent<Rigidbody> ();
	}

	// firing a shot doesn't require physics, so let's not wait for
	// FixedUpdate
	void Update(){
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			// in the inspectors, rotation quaternions are simplified as Euler quaternions
			//GameObject clone = Instantiate(shot, shotSpawn.position, shotSpawn.rotation) as GameObject;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
		}
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
