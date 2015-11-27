using UnityEngine;
using System.Collections;

public class EvasiveManeuver : MonoBehaviour {

	//randomizable range
	public Vector2 startWait, maneuverTime, maneuverWait;
	public float dodge, smoothing, tilt;
	public Boundary boundary;

	private float targetManeuver, currentSpeed;
	private Rigidbody rb;
	private Transform playerTransform;

	void Start(){
		rb = GetComponent<Rigidbody> ();
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		if (player != null){
				playerTransform = player.transform;
		}
		currentSpeed = rb.velocity.z;
		StartCoroutine (Evade ());
	}

	IEnumerator Evade(){
		// wait a bit before evading
		yield return new WaitForSeconds(Random.Range (startWait.x, startWait.y));
		while (true) {
			// go towards some random point,
			// use -Sign to make it swerve towards the center rather than the edges
			float finalManeuver = Random.Range (1, dodge) * -Mathf.Sign(transform.position.x); 
			if ((Random.value > 0.5f) && playerTransform != null){
				targetManeuver = finalManeuver;
			} else {
				// sometimes, go towards the player
				try{
					targetManeuver = playerTransform.position.x;
				}catch{
					targetManeuver = finalManeuver;
				}
			}
			yield return new WaitForSeconds (Random.Range (maneuverTime.x, maneuverTime.y));
			// go towards origin
			targetManeuver = 0.0f;
			yield return new WaitForSeconds (Random.Range (maneuverWait.x, maneuverWait.y));
		}
	}

	void FixedUpdate (){
		// slowly move from our current x towards the target maneuver
		float newManeuver = Mathf.MoveTowards (rb.velocity.x, targetManeuver, Time.deltaTime * smoothing);
		// move in x, but keep the z movement the same
		rb.velocity = new Vector3 (newManeuver, 0.0f, currentSpeed);
		// Clamp just in case, since we always maneuver back towards the center it's unlikely
		rb.position = new Vector3 (
			// keep any given value between a min/max
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp (rb.position.z, boundary.zMin, boundary.zMax)
		);
		// make the enemy tilt as well
		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}
}
