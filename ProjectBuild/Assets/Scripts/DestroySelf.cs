using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour {

	void OnCollisionEnter(Collision other)
	{
		//Debug.Log("Collision with: " + other.gameObject.tag);
		if (other.gameObject.tag == "Player")
		{
			GetComponent<MeshExploder>().Explode();
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		//Debug.Log("Collision with: " + other.gameObject.tag);
		if (other.gameObject.tag == "Player")
		{
			GetComponent<MeshExploder>().Explode();
			Destroy(gameObject);
		}
	}

	void OnParticalCollision(GameObject other)
	{
		//Debug.Log("Particle Collision with: " + other.tag);
		if (other.tag == "Player")
		{
			GetComponent<MeshExploder>().Explode();
			Destroy(gameObject);
		}
	}

	// If we destroy the enemy by destroying its grandparent (i.e. spawning portal), trigger this for mesh explosion
	private void OnDestroy()
	{
		if (transform.parent.gameObject != null)
		{
			GetComponent<MeshExploder>().Explode();
			Destroy(transform.parent.gameObject);
		}
	}

}
