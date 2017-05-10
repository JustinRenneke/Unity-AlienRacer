using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripodController : MonoBehaviour {

	public float wanderRadius;
	public float wanderTimer;

	private Transform target;
	private UnityEngine.AI.NavMeshAgent agent;
	private float timer;

	public CharacterController controller;

	public GameObject player;
	public float minDistance = 25;
	private float step;
	public float speed = 10;

	private bool chasing = false;
	private Vector3 playerPos;
	private float detectionRadius = 100;
	private float escapeRadius = 150;

	private bool collided = false;
	public GameObject mesh;



	// Use this for initialization
	void Start()
	{
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		timer = wanderTimer;
		GameState.tripodsKilled = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (GameState.portalsKilled < 5)
		{
			timer += Time.deltaTime;

			if (timer >= wanderTimer)
			{
				if (agent.isOnNavMesh)
				{
					Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
					agent.SetDestination(newPos);
					timer = 0;
				}
			}
		}else
		{
			//meshCollider.SetActive(true);
			if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= detectionRadius)
			{
				//chasing = true;
				agent.enabled = false;
				controller.enabled = false;
				gameObject.transform.LookAt(player.transform);
				if (Vector3.Distance(gameObject.transform.position, player.transform.position) > minDistance)
				{
					step = speed * Time.deltaTime;
					playerPos = new Vector3(player.transform.position.x, player.transform.position.y - 3, player.transform.position.z);
					gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, playerPos, step);
				}
			}else if (Vector3.Distance(gameObject.transform.position, player.transform.position) >= escapeRadius)
			{
				agent.enabled = true;
				controller.enabled = true;
				timer += Time.deltaTime;

				if (timer >= wanderTimer)
				{
					if (agent.isOnNavMesh)
					{
						Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
						agent.SetDestination(newPos);
						timer = 0;
					}
				}
			}

		}
	}

	void OnCollisionEnter(Collision other)
	{
		Debug.Log("Collision with: " + other.gameObject.tag);
		if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Pulse") && !collided && GameState.portalsKilled >= 5)
		{
			Debug.Log("Tripod kill collision!");
			collided = true;
			ExplodeEnemy();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("Trigger with: " + other.gameObject.tag);
		if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Pulse") && !collided && GameState.portalsKilled >= 5)
		{
			Debug.Log("Tripod kill collision!");
			collided = true;
			ExplodeEnemy();
		}
	}

	public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
	{
		Vector3 randDirection = Random.insideUnitSphere * dist;

		randDirection += origin;

		UnityEngine.AI.NavMeshHit navHit;

		UnityEngine.AI.NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

		return navHit.position;
	}

	private void ExplodeEnemy()
	{
		GameState.beingShot = false;
		mesh.GetComponent<MeshExploder>().Explode();
		DeathScream.playDeathScreamTripod();
		GameState.tripodsKilled++;
		//transform.parent.gameObject.SetActive(false);
		collided = false;
		gameObject.SetActive(false);
		//transform.parent.gameObject.transform.parent = null;
	}
}

