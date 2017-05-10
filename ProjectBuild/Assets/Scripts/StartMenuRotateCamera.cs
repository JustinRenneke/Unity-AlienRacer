using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuRotateCamera : MonoBehaviour {

	public Transform target;
	public float rotationModifier;

	void Update()
	{
		// Rotate the camera every frame so it keeps looking at the target
		Vector3 targetAdjust = new Vector3(.65f, 0f, .65f);
		transform.LookAt(target.position + targetAdjust);
		transform.Translate(Vector3.right * Time.deltaTime * rotationModifier);
	}
}
