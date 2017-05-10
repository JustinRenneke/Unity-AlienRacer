using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CollisionScript : MonoBehaviour {

	public AudioClip portalSound;
	private AudioSource portalSource { get { return GetComponent<AudioSource> (); } } 
	private AudioSource alienSource { get { return GetComponent<AudioSource> (); } }


	// Use this for initialization
	void Start () {
		gameObject.AddComponent<AudioSource> ();
		portalSource.clip = portalSound;

		portalSource.playOnAwake = false;
	}
	

	void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.tag == "RacePortal") {
			
			portalSource.PlayOneShot (portalSound);
		}
	}

}
