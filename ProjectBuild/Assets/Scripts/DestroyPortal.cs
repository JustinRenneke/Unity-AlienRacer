using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPortal: MonoBehaviour
{
	private Renderer rend;
	private int numSpawnsKilled = 0;

	public int spawnKillsRequired = 20;

	public static int globalAliensKilled;


	// Use this for initialization
	void Start()
	{
		globalAliensKilled = 0;
	}

	void Update()
	{
		if(numSpawnsKilled >= spawnKillsRequired)
		{
			ExplodePortal();
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player")
		{
			rend = GetComponent<Renderer>();
			rend.enabled = true;
			DeathScream.playDeathScreamPortal();
			GetComponent<MeshExploder>().Explode();
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		print(gameObject.GetType());
		if (other.gameObject.tag == "Player")
		{
			rend = GetComponent<Renderer>();
			rend.enabled = true;
			DeathScream.playDeathScreamPortal();
			GetComponent<MeshExploder>().Explode();
			Destroy(gameObject);
		}
	}

	void OnParticalCollision(GameObject other)
	{
		if (other.tag == "Player")
		{
			rend = GetComponent<Renderer>();
			rend.enabled = true;
			DeathScream.playDeathScreamPortal();
			GetComponent<MeshExploder>().Explode();
			Destroy(gameObject);
		}
	}

	public void incrementSpawnsKilled()
	{
		numSpawnsKilled++;
		globalAliensKilled++;
		InvasionStageUIController.aliensKilled = globalAliensKilled;
	}

	private void ExplodePortal()
	{
		InvasionStageUIController.portalsKilled++;
		GameState.portalsKilled++;
		rend = GetComponent<Renderer>();
		rend.enabled = true;
		DeathScream.playDeathScreamPortal();
		GetComponent<MeshExploder>().Explode();
		Destroy(gameObject);
	}


}