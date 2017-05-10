using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.UI;
public class UICanvasController : MonoBehaviour {

	//public Canvas canvasNormal;
	//public Canvas canvasVR;

	public GameObject canvasNormalGO;
	public GameObject canvasVRGO;

	private bool vrPresent = false;

	void Start () {
		Debug.Log("Is a VR Device present according to VRDevice.isPresent? " + VRDevice.isPresent);
		vrPresent = true;

		//if (vrPresent)
		if (VRDevice.isPresent)
		{
			canvasNormalGO.SetActive(false);
			canvasVRGO.SetActive(true);
			//canvasNormal.enabled = false;
			//canvasVR.enabled = true;
		}
		else
		{
			canvasNormalGO.SetActive(true);
			canvasVRGO.SetActive(false);
		}
	}
	
	void Update () {
		/*if (Input.GetKeyDown(KeyCode.R))
		{
			vrPresent = !vrPresent;
			Debug.Log("vrPresent: " + vrPresent);
		}
		if (vrPresent)
		{
			canvasNormalGO.SetActive(false);
			canvasVRGO.SetActive(true);
			//canvasNormal.enabled = false;
			//canvasVR.enabled = true;
		}
		else
		{
			canvasNormalGO.SetActive(true);
			canvasVRGO.SetActive(false);
		}*/
	}
}
