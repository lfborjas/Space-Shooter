using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	//public GameObject hazard;
	public GameObject[] hazards;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public GUIText scoreText, restartText, gameOverText;
	private int score;
	private bool gameOver, restart;

	void Start(){
		score = 0;
		gameOver = false;
		restart = false;
		restartText.text = "";
		gameOverText.text = "";

		UpdateScore ();
		StartCoroutine (SpawnWaves ());
	}

	void Update(){
		if (restart) {
			if (Input.GetKeyDown(KeyCode.R)){
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}

	IEnumerator SpawnWaves(){
		yield return new WaitForSeconds (startWait);
		// keep creating waves until the player dies
		while(true){
			for (int i = 0; i < hazardCount; i++) {

				// all kinds of things could be here: powerups, enemies, etc.
				// in Unity, we just dragged the asteroids from the prefabs to a locked "hazards" property for
				// the GameController
				GameObject hazard = hazards[Random.Range(0,hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				//don't create them all at the same time, needs the function to be a coroutine to not block the loop
				yield return new WaitForSeconds(spawnWait);
			}
			yield return new WaitForSeconds(waveWait);

			if (gameOver){
				restartText.text = "Press 'R' to restart";
				restart = true;
				break;
			}
		}
	}

	public void addScore(int newScoreValue){
		score += newScoreValue;
		UpdateScore ();
	}

	void UpdateScore(){
		scoreText.text = "Score: " + score; 
	}

	public void GameOver(){
		gameOverText.text = "Game Over";
		gameOver = true;
	}
}
