using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {
	static public Main S;

	public GameObject[]			prefabEnemies;
	public float				enemySpawnPerSecond = 0.5f; //#enemies/second
	public float				enemySpawnPadding = 1.5f; //padding for position
	public WeaponDefinition []	weaponDefinitions;

	public bool					_______;

	public WeaponType[]			activeWeaponTypes;
	public float				enemySpawnRate; //Delay between Enemy spawns

	void Awake() {
		S = this;
		// Set Utils.camBounds
		Utils.SetCameraBounds(this.GetComponent<Camera>());
		// 0.5 enemies/s = enemySpawnRate of 2
		enemySpawnRate = 1f / enemySpawnPerSecond;
		// Invoke call SpawnEnemy() once after a 2 second delay
		Invoke ("SpawnEnemy", enemySpawnRate);
	}

	void Start(){
		activeWeaponTypes = new WeaponType[weaponDefinitions.Length];
		for (int i=0; i<weaponDefinitions.Length; i++) {
			activeWeaponTypes [i] = weaponDefinitions [i].type;
		}
	}

	public void Spawnenemy(){
		// Pick a random Enemy prefab to instantiate
		int ndx = Random.Range (0, prefabEnemies.Length);
		GameObject go = Instantiate (prefabEnemies [ndx]) as GameObject;
		// Position the Enemy above the screen with a random x position
		Vector3 pos = Vector3.zero;
		float xMin = Utils.camBounds.min.x + enemySpawnPadding;
		float xMax = Utils.camBounds.max.x + enemySpawnPadding;
		pos.x = Random.Range (xMin, xMax);
		pos.y = Utils.camBounds.max.y + enemySpawnPadding;
		go.transform.position = pos;
		// Call SpawnEnemy() again in a couple of seconds
		Invoke ("SpawnEnemy", enemySpawnRate);
	}

	public void DelayedRestart (float delay){
		//Invoke the Restart () method in delay seconds
		Invoke ("Restart", delay);
	}
	public void Restart() {
		// Reload_Scene_0 to restart the game
		Application.LoadLevel ("_Scene_0");
	}
}

