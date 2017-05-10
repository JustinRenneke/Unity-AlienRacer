using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripodShooting : MonoBehaviour {

	public int damagePerShot = 1;                  // The damage inflicted by each bullet.
	public float timeBetweenBullets = 0.15f;        // The time between each shot.
	public float range = 100f;                      // The distance the gun can fire.
	public AudioSource audio;
	private Vector3 particleOffset;

	//public static bool inTrigger = false;

	float timer;                                    // A timer to determine when to fire.
	Ray shootRay;                                   // A ray from the gun end forwards.
	RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
													//int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
	public ParticleSystem laserParticles;                    // Reference to the particle system.
	LineRenderer laserLine;                           // Reference to the line renderer.
	AudioSource laserAudio;                           // Reference to the audio source.
	Light laserLight;                                 // Reference to the light component.
	public float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.

	void Start()
	{
		//laserParticles.Stop();
		//particleOffset = new Vector3(-.43f, 1.906f, .191f);
	}

public Vector3 location;
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag != "Pulse")
		{
			audio.Play();
			//laserParticles.Play();
			//inTrigger = true;
			GameState.beingShot = true;
		}

	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag != "Pulse")
		{
			//inTrigger = false;
			GameState.beingShot = false;
			audio.Stop();
			//laserParticles.Stop();
		}
	}


}
