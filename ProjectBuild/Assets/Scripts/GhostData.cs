using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostData : MonoBehaviour {
	public static bool invasionGhost = false;
	public static List<float> newTransformX = new List<float>();
	public static List<float> newTransformY = new List<float>();
	public static List<float> newTransformZ = new List<float>();

	public static List<float> storedTransformX = new List<float>();
	public static List<float> storedTransformY = new List<float>();
	public static List<float> storedTransformZ = new List<float>();


	public static List<float> newQuaternionX = new List<float>();
	public static List<float> newQuaternionY = new List<float>();
	public static List<float> newQuaternionZ = new List<float>();
	public static List<float> newQuaternionW = new List<float>();

	public static List<float> storedQuaternionX = new List<float>();
	public static List<float> storedQuaternionY = new List<float>();
	public static List<float> storedQuaternionZ = new List<float>();
	public static List<float> storedQuaternionW = new List<float>();


	public static List<float> newLeftParticleAngle = new List<float>();
	public static List<float> newRightParticleAngle = new List<float>();
	public static List<float> newLeftParticleLifetime = new List<float>();
	public static List<float> newRightParticleLifetime = new List<float>();

	public static List<float> storedLeftParticleAngle = new List<float>();
	public static List<float> storedRightParticleAngle = new List<float>();
	public static List<float> storedLeftParticleLifetime = new List<float>();
	public static List<float> storedRightParticleLifetime = new List<float>();

	void Start () {
		DontDestroyOnLoad(gameObject);
	}
}
