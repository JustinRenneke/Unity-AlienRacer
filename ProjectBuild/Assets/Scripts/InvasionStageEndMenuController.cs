using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InvasionStageEndMenuController : MonoBehaviour {

	public Text aliensKilledText;
	public Text timeRemainingText;
	public Text portalsRemainingText;
	public Text scoreText;

	// Use this for initialization
	void Start () {
		aliensKilledText.text = GameState.aliensKilled;
		timeRemainingText.text = GameState.timeRemaining;
		portalsRemainingText.text = GameState.portalsRemaining;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return))
		{
			Scene scene = SceneManager.GetActiveScene();
			if (scene.name.CompareTo("InvasionStageSuccessMenu") == 0)
			{
				Application.LoadLevel("StartMenu");
			}
			else
			{
				Application.LoadLevel("InvasionStage");
			}
		}
	}

	public void LoadLevel(string level)
	{
		//SceneManager.LoadScene(level);
		Application.LoadLevel(level);
	}
}
