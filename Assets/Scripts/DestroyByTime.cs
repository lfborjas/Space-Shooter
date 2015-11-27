using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {
	public float lifetime;

	void Start(){
		// we can specify a wait time before destroy actually takes effect
		Destroy (gameObject, lifetime);
	}
}
