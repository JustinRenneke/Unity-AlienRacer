using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelfDestroy : MonoBehaviour
{

	private GameObject grandparent;
	private DestroyPortal destroyPortal;
	private bool collided = false;

	private int deathScreamRoll;
	private string name;

	void Start()
	{
		//Debug.Log("self: " + gameObject);
		//Debug.Log("parent: " + transform.parent.gameObject);
		//Debug.Log("grandparent: " + transform.parent.gameObject.transform.parent.gameObject);
		//Debug.Log("great-grandparent: " + transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject);
		destroyPortal = transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<DestroyPortal>();

		deathScreamRoll = (int)Random.Range(0, 4);
		name = transform.parent.gameObject.name;

	}

	void OnCollisionEnter(Collision other)
	{
		//Debug.Log("Collision with: " + other.gameObject.tag);
		if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Pulse") && !collided)
		{
			collided = true;
			ExplodeEnemy();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		//Debug.Log("Collision with: " + other.gameObject.tag);
		if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Pulse") && !collided)
		{
			collided = true;
			ExplodeEnemy();
		}
	}

	void OnParticalCollision(GameObject other)
	{
		//Debug.Log("Particle Collision with: " + other.tag);
		if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Pulse") && !collided)
		{
			collided = true;
			ExplodeEnemy();
		}
	}

	// If we destroy the enemy by destroying its grandparent (i.e. spawning portal), trigger this for mesh explosion
	private void OnDestroy()
	{
		if (transform.parent.gameObject != null && transform.parent.gameObject.activeSelf)
		{
			GetComponent<MeshExploder>().Explode();
			transform.parent.gameObject.SetActive(false);
			collided = false;
			transform.parent.gameObject.transform.parent = null;
		}
		else
		{
			Destroy(transform.parent.gameObject);
		}
	}

	private void ExplodeEnemy()
	{
		if (destroyPortal != null)
		{
			if (string.Compare(name, "Alien_CyanGlow(Clone)") == 0)
				DeathScream.playDeathScreamCyan();
			if (string.Compare(name, "Alien_YellowGlow(Clone)") == 0)
				DeathScream.playDeathScreamYellow();
			if (string.Compare(name, "Alien_WhiteGlow(Clone)") == 0)
				DeathScream.playDeathScreamWhite();
			if (string.Compare(name, "Mr Grey(Clone)") == 0)
				DeathScream.playDeathScreamGrey();
			destroyPortal.incrementSpawnsKilled();
			GetComponent<MeshExploder>().Explode();
			transform.parent.gameObject.SetActive(false);
			collided = false;
			transform.parent.gameObject.transform.parent = null;
		}
		else
		{
			if (string.Compare(name, "Alien_CyanGlow(Clone)") == 0)
				DeathScream.playDeathScreamCyan();
			if (string.Compare(name, "Alien_YellowGlow(Clone)") == 0)
				DeathScream.playDeathScreamYellow();
			if (string.Compare(name, "Alien_WhiteGlow(Clone)") == 0)
				DeathScream.playDeathScreamWhite();
			if (string.Compare(name, "Mr Grey(Clone)") == 0)
				DeathScream.playDeathScreamGrey();
			DeathScream.playDeathScreamCyan();
			GetComponent<MeshExploder>().Explode();
			transform.parent.gameObject.SetActive(false);
			collided = false;
			transform.parent.gameObject.transform.parent = null;
		}
	}
}
