using ProgressBar;
using System.Collections;
using UnityEngine;

public class RadialProgressBehavior : MonoBehaviour
{
	ProgressRadialBehaviour BarBehaviour;
	[SerializeField]
	float UpdateDelay = 2f;

	void OnEnable()
	{
		BarBehaviour = GetComponent<ProgressRadialBehaviour>();
		InvokeRepeating("UpdateDial", 0f, UpdateDelay);
	}

	private void UpdateDial()
	{
		BarBehaviour.Value = HoverCarControl.thrustMultiplier / HoverCarControl.globalMaxThrustMultiplier * 100;
	}

	/*IEnumerator Start()
	{
		BarBehaviour = GetComponent<ProgressRadialBehaviour>();
		while (true)
		{
			yield return new WaitForSeconds(UpdateDelay);
			BarBehaviour.Value = HoverCarControl.thrustMultiplier / HoverCarControl.globalMaxThrustMultiplier * 100;
			//BarBehaviour.Value = Random.value * 100;
			//print("new value: " + BarBehaviour.Value);
		}
	}*/
}