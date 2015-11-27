using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameObject hazard;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	void Start(){
		StartCoroutine (SpawnWaves ());
	}

	IEnumerator SpawnWaves(){
		yield return new WaitForSeconds (startWait);
		// keep creating waves until the player dies
		while(true){
			for (int i = 0; i < hazardCount; i++) {
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				//don't create them all at the same time, needs the function to be a coroutine to not block the loop
				yield return new WaitForSeconds(spawnWait);
			}
			yield return new WaitForSeconds(waveWait);
		}
	}
}
