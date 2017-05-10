using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvasionStageUIController : MonoBehaviour {

	private float timePassed;
	private float timeRemaining;
	private int minutes, seconds, fraction;
	private int portalsRemaining;
	private int i;

	public static int aliensKilled;
	public static int portalsKilled;

	public int portalsPresent;
	public float timeAllowance;
	public Text aliensKilledText;
	public Text portalsRemainingText;
	public Text timeRemainingText;
	public Text speed;
	public Text aliensKilledTextfp;
	public Text portalsRemainingTextfp;
	public Text timeRemainingTextfp;
	public Text speedfp;
	public Text aliensKilledTextVR;
	public Text portalsRemainingTextVR;
	public Text timeRemainingTextVR;
	public Text speedVR;
	public Text aliensKilledTextfpVR;
	public Text portalsRemainingTextfpVR;
	public Text timeRemainingTextfpVR;
	public Text speedfpVR;

	public Text playerHealth;
	public Text playerHealthfp;
	public Text playerHealthVR;
	public Text playerHealthfpVR;

	public Text tripodWarning;
	public Text tripodWarningVR;
	public Text tripodWarningFP;
	public Text tripodWarningFPVR;
	private bool warningGiven = false;

	// Use this for initialization
	void Start () {
		timePassed = 0;
		aliensKilled = 0;
		portalsKilled = 0;
		GameState.portalsKilled = 0;
		warningGiven = false;
		tripodWarning.enabled = false;
		tripodWarningVR.enabled = false;
		tripodWarningFP.enabled = false;
		tripodWarningFPVR.enabled = false;

		//GameState.invasionGhost = true;
	}
	
	// Update is called once per frame
	void Update () {
		
		timePassed += Time.deltaTime;
		timeRemaining = timeAllowance - timePassed;

		minutes = (int)timeRemaining / 60;
		seconds = (int)timeRemaining % 60;
		fraction = (int)((timeRemaining * 100) % 100);

		if(timeRemaining <= 0)
		{
			timeRemainingText.text = ("Invasion repelled. Finish the tripods!");
			timeRemainingTextfp.text = ("Invasion repelled. Finish the tripods!");
			timeRemainingTextVR.text = ("Invasion repelled. Finish the tripods!");
			timeRemainingTextfpVR.text = ("Invasion repelled. Finish the tripods!");
		}
		else if (timeRemaining > 0)
		{
			timeRemainingText.text = "Time Remaining: " + string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
			timeRemainingTextfp.text = "Time Remaining: " + string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
			timeRemainingTextVR.text = "Time Remaining: " + string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
			timeRemainingTextfpVR.text = "Time Remaining: " + string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
		}
		else
		{
			timeRemainingText.text = "Time Remaining: " + string.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);
			timeRemainingTextfp.text = "Time Remaining: " + string.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);
			timeRemainingTextVR.text = "Time Remaining: " + string.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);
			timeRemainingTextfpVR.text = "Time Remaining: " + string.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);
		}

		aliensKilledText.text = "Aliens Killed: " + aliensKilled;
		aliensKilledTextfp.text = "Aliens Killed: " + aliensKilled;
		aliensKilledTextVR.text = "Aliens Killed: " + aliensKilled;
		aliensKilledTextfpVR.text = "Aliens Killed: " + aliensKilled;
		portalsRemaining = portalsPresent - portalsKilled;
		portalsRemainingText.text = "Portals Remaining: " + portalsRemaining + "/" + portalsPresent;
		portalsRemainingTextfp.text = "Portals Remaining: " + portalsRemaining + "/" + portalsPresent;
		portalsRemainingTextVR.text = "Portals Remaining: " + portalsRemaining + "/" + portalsPresent;
		portalsRemainingTextfpVR.text = "Portals Remaining: " + portalsRemaining + "/" + portalsPresent;
		speed.text = "SPEED: " + (int)HoverCarControl.currentSpeed * 4 + " KM/H";
		speedfp.text = "SPEED: " + (int)HoverCarControl.currentSpeed * 4 + " KM/H";
		speedVR.text = "SPEED: " + (int)HoverCarControl.currentSpeed * 4 + " KM/H";
		speedfpVR.text = "SPEED: " + (int)HoverCarControl.currentSpeed * 4 + " KM/H";
		playerHealth.text = "Health: " + HoverCarHealth.currentHealth;

		if (timeRemaining <= 0 && GameState.portalsKilled < 5)
		{
			GhostCarSetup();
			//GhostCar.run = true;
			//GameState.opponent = true;
			timeRemainingText.text = ("You failed to repel the invasion!");
			Application.LoadLevel("InvasionStageFailMenu");
		}

		if (GameState.portalsKilled == 5 && GameState.tripodsKilled >= 6)
		{
			GhostCarSetup();
			//GhostCar.run = true;
			//GameState.opponent = true;
			timeRemainingText.text = ("Closed all invasion portals and wiped out the tripods!");
			Application.LoadLevel("InvasionStageSuccessMenu");
		}

		if(GameState.portalsKilled == 5 && warningGiven == false)
		{
			warningGiven = true;

			tripodWarning.enabled = true;
			tripodWarning.CrossFadeAlpha(0.0f, 5f, false);
			tripodWarningVR.enabled = true;
			tripodWarningVR.CrossFadeAlpha(0.0f, 5f, false);
			tripodWarningFP.enabled = true;
			tripodWarningFP.CrossFadeAlpha(0.0f, 5f, false);
			tripodWarningFPVR.enabled = true;
			tripodWarningFPVR.CrossFadeAlpha(0.0f, 5f, false);
		}

	}

	private void OnDestroy()
	{
		GameState.aliensKilled = aliensKilledText.text;
		GameState.timeRemaining = timeRemainingText.text;
		GameState.portalsRemaining = portalsRemainingText.text;
	}

	private void GhostCarSetup()
	{
		i = 0;
		// Copy position data for next round
		GhostData.storedTransformX.Clear();
		GhostData.storedTransformY.Clear();
		GhostData.storedTransformZ.Clear();
		foreach (float x in GhostData.newTransformX)
		{
			GhostData.storedTransformX.Add(x);
			GhostData.storedTransformY.Add(GhostData.newTransformY[i]);
			GhostData.storedTransformZ.Add(GhostData.newTransformZ[i]);
			i++;
		}
		GhostData.newTransformX.Clear();
		GhostData.newTransformY.Clear();
		GhostData.newTransformZ.Clear();


		i = 0;
		// Copy rotation data for next round
		GhostData.storedQuaternionX.Clear();
		GhostData.storedQuaternionY.Clear();
		GhostData.storedQuaternionZ.Clear();
		GhostData.storedQuaternionW.Clear();
		foreach (float x in GhostData.newQuaternionX)
		{
			GhostData.storedQuaternionX.Add(x);
			GhostData.storedQuaternionY.Add(GhostData.newQuaternionY[i]);
			GhostData.storedQuaternionZ.Add(GhostData.newQuaternionZ[i]);
			GhostData.storedQuaternionW.Add(GhostData.newQuaternionW[i]);
			i++;
		}
		GhostData.newQuaternionX.Clear();
		GhostData.newQuaternionY.Clear();
		GhostData.newQuaternionZ.Clear();
		GhostData.newQuaternionW.Clear();


		i = 0;
		GhostData.storedLeftParticleAngle.Clear();
		GhostData.storedRightParticleAngle.Clear();
		foreach (float x in GhostData.newLeftParticleAngle)
		{
			GhostData.storedLeftParticleAngle.Add(GhostData.newLeftParticleAngle[i]);
			GhostData.storedRightParticleAngle.Add(GhostData.newRightParticleAngle[i]);
			i++;
		}
		GhostData.newLeftParticleAngle.Clear();
		GhostData.newRightParticleAngle.Clear();


		i = 0;
		GhostData.storedLeftParticleLifetime.Clear();
		GhostData.storedRightParticleLifetime.Clear();
		foreach (float x in GhostData.newLeftParticleLifetime)
		{
			GhostData.storedLeftParticleLifetime.Add(GhostData.newLeftParticleLifetime[i]);
			GhostData.storedRightParticleLifetime.Add(GhostData.newRightParticleLifetime[i]);
			i++;
		}
		GhostData.newLeftParticleLifetime.Clear();
		GhostData.newRightParticleLifetime.Clear();


		GhostCar.run = true;
		GameState.invasionGhost = true;
		GhostData.invasionGhost = true;
		Debug.Log("Ghost car setup executed!");
		Debug.Log("invasionGhost from UIController? " + GameState.invasionGhost);
	}
}
