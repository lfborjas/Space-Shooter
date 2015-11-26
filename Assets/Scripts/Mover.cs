using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	public float speed;

	private Rigidbody rb;
	// Use this for initialization: executed on the first frame
	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.velocity = transform.forward * speed;
	}
}
