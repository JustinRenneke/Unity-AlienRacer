using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
	public static EnemyManager enemyManager;
	// public PlayerHealth playerHealth;       // Reference to the player's heatlh.
	//public GameObject[] enemies;                // The enemy prefab to be spawned.
	public float spawnTime = 3f;            // How long between each spawn.
	public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
	public int maxEnemyPerSpawnPoint = 5;

	// Object Pooling
	public int numEnemiesInEachPool = 30;
	List<GameObject> lowPolyAliensCyan;
	List<GameObject> lowPolyAliensYellow;
	List<GameObject> lowPolyAliensWhite;
	List<GameObject> mrGreys;
	List<GameObject> spawnSource;
	public GameObject cyanAlien;
	public GameObject yellowAlien;
	public GameObject whiteAlien;
	public GameObject mrGrey;

	void Awake()
	{
		enemyManager = this;
	}

	void Start()
	{
		// Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
		InvokeRepeating("Spawn", spawnTime, spawnTime);

		lowPolyAliensCyan = new List<GameObject>();
		lowPolyAliensYellow = new List<GameObject>();
		lowPolyAliensWhite = new List<GameObject>();
		mrGreys = new List<GameObject>();

		for (int i = 0; i < numEnemiesInEachPool; i++)
		{
			GameObject cyanAlienInstance = (GameObject)Instantiate(cyanAlien);
			cyanAlienInstance.SetActive(false);
			lowPolyAliensCyan.Add(cyanAlienInstance);
			GameObject yellowAlienInstance = (GameObject)Instantiate(yellowAlien);
			yellowAlienInstance.SetActive(false);
			lowPolyAliensYellow.Add(yellowAlienInstance);
			GameObject whiteAlienInstance = (GameObject)Instantiate(whiteAlien);
			whiteAlienInstance.SetActive(false);
			lowPolyAliensWhite.Add(whiteAlienInstance);
			GameObject mrGreyInstance = (GameObject)Instantiate(mrGrey);
			mrGreyInstance.SetActive(false);
			mrGreys.Add(mrGreyInstance);
		}
	}


	void Spawn()
	{
		/*     // If the player has no health left...
			if(playerHealth.currentHealth <= 0f)
			{
				// ... exit the function.
				return;
			}
		*/
		// Find a random index between zero and one less than the number of spawn points.
		int spawnPointIndex = Random.Range(0, spawnPoints.Length);
		//int enemyIndex = Random.Range(0, enemies.Length);
		// Random number between 0 and 4 to determine which pool to pull from
		int enemyIndex = Random.Range(0, 4);
		switch (enemyIndex)
		{
			case 0:
				spawnSource = lowPolyAliensCyan;
				break;
			case 1:
				spawnSource = lowPolyAliensYellow;
				break;
			case 2:
				spawnSource = lowPolyAliensWhite;
				break;
			case 3:
				spawnSource = mrGreys;
				break;
			default:
				break;
		}

		// Don't spawn too many per spawnPoint
		if (spawnPoints[spawnPointIndex].transform.childCount < maxEnemyPerSpawnPoint)
		{
			// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
			// old way before pooling
			//var enemy = Instantiate(enemies[enemyIndex], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);

			// Add the newly spawned enemy as a child of the spawner
			//enemy.transform.parent = spawnPoints[spawnPointIndex].transform;

			// New way with pooling
			for (int i = 0; i < spawnSource.Count; i++)
			{
				if (!spawnSource[i].activeInHierarchy)
				{
					spawnSource[i].transform.position = spawnPoints[spawnPointIndex].position;
					spawnSource[i].transform.rotation = spawnPoints[spawnPointIndex].rotation;
					spawnSource[i].transform.parent = spawnPoints[spawnPointIndex].transform;
					spawnSource[i].SetActive(true);
					break;
				}
			}


		}

	}

	public void cleanUp()
	{
		Debug.Log("Cleaning up object pools...");
		for (int i = 0; i < numEnemiesInEachPool; i++)
		{
			if (lowPolyAliensCyan[i])
			{
				Destroy(lowPolyAliensCyan[i]);
			}
			if (lowPolyAliensYellow[i])
			{
				Destroy(lowPolyAliensYellow[i]);
			}
			if (lowPolyAliensWhite[i])
			{
				Destroy(lowPolyAliensWhite[i]);
			}
			if (mrGreys[i])
			{
				Destroy(mrGreys[i]);
			}
		}
	}
}