using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRacePortal : MonoBehaviour
{
	private Renderer rend;
	private RaceStageUIController controller;
	private GameObject levelController;
	private bool collided = false;

	public static int portalsKilled = 0;





	// Use this for initialization
	void Start()
	{
		portalsKilled = 0;
	}

	void Update()
	{

	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player" && !collided)
		{
			collided = true;
			ExplodePortal();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player" && !collided)
		{
			collided = true;
			ExplodePortal();
		}
	}

	void OnParticalCollision(GameObject other)
	{
		if (other.tag == "Player" && !collided)
		{
			collided = true;
			ExplodePortal();
		}
	}

	private void ExplodePortal()
	{
		RaceStageUIController.portalsKilled++;
		rend = GetComponent<Renderer>();
		rend.enabled = true;
		DeathScream.playDeathScreamPortal();
		Debug.Log("ExplodePortal()");
		GetComponent<MeshExploder>().Explode();
		Destroy(gameObject);
	}


}