using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlayer : MonoBehaviour {
	public GameObject player;
	public float minDistance = 25;
	private float step;
	public float speed = .5f;
	// Use this for initialization
	void Start () {
		
	}

	void Update()
	{

		gameObject.transform.LookAt(player.transform);
		if (Vector3.Distance(gameObject.transform.position, player.transform.position) > minDistance) {
			step = speed * Time.deltaTime;
			gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, player.transform.position, speed);
		}
	}
}
