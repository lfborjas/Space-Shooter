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
	private AudioSource _audio;
	private float nextFire = 0.0f;
	private Quaternion calibrationQuaternion;

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
		_audio = GetComponent<AudioSource> ();

		// in "reality", we'd call this from some sort of options settings
		// so we calibrate every time the player is ready to play, but not as late
		// as actually during the scene! This implementation will calibrate
		// to whatever orientation the device physically was in when starting the scene.
		CalibrateAccelerometer ();
	}

	// firing a shot doesn't require physics, so let's not wait for
	// FixedUpdate
	void Update(){

		// magically enough, "Fire1" is derived from either mouse clicks on a computer
		// OR any touch events on a device! So when testing this on the phone touches
		// actually made the ship fire, which is pretty darn neat!
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			// in the inspectors, rotation quaternions are simplified as Euler quaternions
			//GameObject clone = Instantiate(shot, shotSpawn.position, shotSpawn.rotation) as GameObject;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			_audio.Play ();
		}	
	}

	// called just before each fixed physics step
	void FixedUpdate (){

		// these are for the keyboard input, not using them for mobile
		//float moveHorizontal = Input.GetAxis ("Horizontal");
		//float moveVertical = Input.GetAxis("Vertical");
		// remember: x -> left/right, y -> up/down, z -> forward/back
		//Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		// get input from the accelerometer
		// Using this as is (default zero point) I'd need to hold the device parallel to the floor
		// otherwise the ship falls to the bottom of the screen. Need to calibrate the starting point so it tilts
		// from that instead of actuall acceleration zero.
		Vector3 accelerationRaw = Input.acceleration;
		Vector3 acceleration = FixAcceleration (accelerationRaw);
		// using you in the Z plane because that's how the orientation on devices
		// would work. 
		Vector3 movement = new Vector3 (acceleration.x, 0.0f, acceleration.y);

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

	/* Calibrate the input's raw acceleration by rotating to a plane that allows the player to hold it more naturally, vs parallel to the floor*/
	void CalibrateAccelerometer(){
		Vector3 accelerationSnapshot = Input.acceleration;
		Quaternion rotateQuaternion = Quaternion.FromToRotation (new Vector3 (0.0f, 0.0f, -1.0f), accelerationSnapshot);
		calibrationQuaternion = Quaternion.Inverse (rotateQuaternion);
	}

	/* Get the 'calibrated' value from the device input */
	Vector3 FixAcceleration(Vector3 acceleration){
		Vector3 fixedAcceleration = calibrationQuaternion * acceleration;
		return fixedAcceleration;
	}
}
