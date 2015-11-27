using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

	public GameObject shot;
	// like with the player, this allows us to get the transform
	// directly from the game object
	public Transform shotSpawn;
	public float fireRate;
	// don't fire right away
	public float delay;

	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		InvokeRepeating ("Fire", delay, fireRate); 
	}

	void Fire(){
		Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
		audioSource.Play ();
	}
}
