using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTransition : MonoBehaviour
{
	private int i;
	private bool collided;

	private void Start()
	{
		collided = false;
	}

	void OnTriggerEnter(Collider other)
	{
		if (!collided)
		{
			EnemyManager.enemyManager.cleanUp();
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
			foreach(float x in GhostData.newQuaternionX)
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
			foreach(float x in GhostData.newLeftParticleAngle)
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

			if(GameState.demo == true)
			{
				Debug.Log("loading start menu from demo");
				GameState.demo = false;
				Application.LoadLevel("StartMenu");
			}
			else if (other.tag == "Player" && RaceStageUIController.portalsKilled >= RaceStageUIController.portalKillsRequired)
			{
				Application.LoadLevel("RaceStageSuccessMenu");
			}
			else if (other.tag == "Player" && RaceStageUIController.portalsKilled < RaceStageUIController.portalKillsRequired)
			{
				Application.LoadLevel("RaceStageFailMenu");
			}
		}
		collided = true;
	}

}
