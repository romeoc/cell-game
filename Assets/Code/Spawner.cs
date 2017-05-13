using UnityEngine;
using System.Collections;

[System.Serializable]
public class Wave
{
	public float spawnDeplay;
	public int enemyCount;

	public bool spawnAtTop = true;
	public bool spawnAtBottom;
	public bool spawnLeft;
	public bool spawnRight;

	public GameObject[] enemyTypes;
	
}

public class Spawner : MonoBehaviour 
{
	public float startDelay;
	public float waveDelay;

	public GameObject boundary;
	public Wave[] waves;

	private Vector2 spawnPoints;

	void Start()
	{
		//Save collider boundaries for enemy spawn point reference
		spawnPoints = boundary.GetComponent<BoxCollider2D>().size;
		StartCoroutine (spawnEnemies ());
	}

	IEnumerator spawnEnemies()
	{
		//initial waiting time (at the end of the level)
		yield return new WaitForSeconds (startDelay);

		//loop through every wave
		foreach (Wave wave in waves) {
			int i = 0;

			//create "enemyCount " number of enemies for each wave (create an infinite number of enemies if set to 0)
			while (wave.enemyCount  > i++ || wave.enemyCount == 0) {
				//get a random index from the "enemyTypes" array
				System.Random random = new System.Random();
				int randomEnemyIndex = random.Next(0, wave.enemyTypes.Length);

				Instantiate (wave.enemyTypes[randomEnemyIndex], getRandomPosition(wave), Quaternion.identity);
				//waiting time after an enemy is spawned
				yield return new WaitForSeconds(wave.spawnDeplay);
			}

			//waiting time after a wave
			yield return new WaitForSeconds(waveDelay);
		}
	}

	//Generate a random position for the enemy
	Vector3 getRandomPosition(Wave wave)
	{
		//array of all position starting from top and going clockwise (top, right, bottom, left)
		bool[] positions = {wave.spawnAtTop, wave.spawnRight, wave.spawnAtBottom, wave.spawnLeft};
		System.Random random = new System.Random();
		int? randomPosition = null;

		//loop while we get an enabled position
		while (randomPosition == null) {
			//get a new random position
			int newRandomPosition = random.Next (0, positions.Length);
			//check if the position is enabled
			if (positions[newRandomPosition]) {
				randomPosition = newRandomPosition;
			}
		}

		Vector3 generatedPosition = new Vector3();

		//Generate vecctor for the resulted position
		switch (randomPosition) {
			//Top
			case 0:
				generatedPosition = new Vector3 (Random.Range (-spawnPoints.x/2, spawnPoints.x/2), spawnPoints.y/2, 0);
				break;
			//Right
			case 1:
				generatedPosition = new Vector3 (spawnPoints.x/2, Random.Range (-spawnPoints.y/2, spawnPoints.y/2), 0);
				break;
			//Bottom
			case 2:
				generatedPosition = new Vector3 (Random.Range (-spawnPoints.x/2, spawnPoints.x/2), -spawnPoints.y/2, 0);
				break;
			//Left
			case 3:
				generatedPosition = new Vector3 (-spawnPoints.x/2, Random.Range (-spawnPoints.y/2, spawnPoints.y/2), 0);
				break;
		}

		return generatedPosition;
	}
}
