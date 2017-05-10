using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceStageUIController : MonoBehaviour {

	private float time;
	private int minutes;
	private int seconds;
	private int fraction;

	public Text timer;
	public Text portalsClosed;
	public Text speed;
	public Text timerVR;
	public Text portalsClosedVR;
	public Text speedVR;
	public Text timerfp;
	public Text portalsClosedfp;
	public Text speedfp;
	public Text timerfpVR;
	public Text portalsClosedfpVR;
	public Text speedfpVR;

	//public Text thrustMultiplier;
	//public Text leftThrustMultiplier;
	//public Text rightThrustMultiplier;
	public int portalsPresent;
	public int portalMissesAllowed;
	public static int portalsKilled;
	public static int portalKillsRequired;

	void Start () {
		portalsKilled = 0;
		time = 0;
		portalKillsRequired = portalsPresent - portalMissesAllowed;
		GhostData.invasionGhost = false;
	}


	void Update () {
		time += Time.deltaTime;

		minutes = (int)time / 60;
		seconds = (int)time % 60;
		fraction = (int)((time* 100) % 100);

		timer.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
		timerVR.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
		timerfp.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
		timerfpVR.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
		portalsClosed.text = "Portals Closed: " + portalsKilled.ToString() + "/" + portalsPresent;
		portalsClosedfp.text = "Portals Closed: " + portalsKilled.ToString() + "/" + portalsPresent;
		portalsClosedVR.text = "Portals Closed: " + portalsKilled.ToString() + "/" + portalsPresent;
		portalsClosedfpVR.text = "Portals Closed: " + portalsKilled.ToString() + "/" + portalsPresent;
		speed.text = "SPEED: " + (int)HoverCarControl.currentSpeed*4 + " KM/H";
		speedfp.text = "SPEED: " + (int)HoverCarControl.currentSpeed * 4 + " KM/H";
		speedVR.text = "SPEED: " + (int)HoverCarControl.currentSpeed * 4 + " KM/H";
		speedfpVR.text = "SPEED: " + (int)HoverCarControl.currentSpeed * 4 + " KM/H";
		//thrustMultiplier.text = "thrustMultiplier: " + HoverCarControl.thrustMultiplier;
		//leftThrustMultiplier.text = "leftThrustMultiplier: " + HoverCarControl.leftThrustMultiplier;
		//rightThrustMultiplier.text = "rightThrustMultiplier: " + HoverCarControl.rightThrustMultiplier;
		//portalsKilled = DestroyRacePortal.portalsKilled;
	}

	private void OnDestroy()
	{
		GameState.raceTime = timer.text;
		GameState.racePortalsClosed = portalsClosed.text;
	}
}
