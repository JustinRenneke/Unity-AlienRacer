using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RaceStageEndMenuController : MonoBehaviour {

	public Text time;
	public Text portalsClosed;
	public Text score;

	// Use this for initialization
	void Start () {
		time.text = "Time: " + GameState.raceTime;
		portalsClosed.text = GameState.racePortalsClosed;
		GameState.opponent = false;
		//Scene scene = SceneManager.GetActiveScene();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			Scene scene = SceneManager.GetActiveScene();
			if (scene.name.CompareTo("RaceStageFailMenu") == 0)
			{
				Application.LoadLevel("RaceStage");
			}
			else
			{
				Application.LoadLevel("InvasionStage");
			}
		}
		
	}

	public void LoadLevel(string level)
	{
		//Debug.Log("Level load requested for: " + level);
		//SceneManager.LoadScene(level);
		Application.LoadLevel(level);
	}

	public void Opponent()
	{
		GameState.opponent = !GameState.opponent;
	}
}
