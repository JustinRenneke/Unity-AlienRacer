using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuUI : MonoBehaviour {

	void Start()
	{
		Time.timeScale = 1.0f;
		GameState.opponent = false;
		GhostCar.run = false;
		GameState.opponent = false;
		GameState.invasionGhost = false;
		GhostData.invasionGhost = false;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			Scene scene = SceneManager.GetActiveScene();
			if (scene.name.CompareTo("StartMenu") == 0)
			{
				Application.LoadLevel("RaceStageIntro");
			}
			else
			{
				Application.LoadLevel("RaceStage");
			}
		}
	}
	public void LoadLevel(string level)
	{
		Debug.Log("Level load requested for: " + level);
		//SceneManager.LoadScene(level);
		Application.LoadLevel(level);
			
	}

	public void Exit()
	{
			Application.Quit();
	}

	public void toggleOpponent()
	{
		GameState.opponent = !GameState.opponent;
	}

	public void toggleDemo()
	{
		GameState.demo = !GameState.demo;
	}
}
