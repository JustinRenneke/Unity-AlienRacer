using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
	private bool paused = false;
	public GameObject canvas;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			togglePause();
		}
	}

	public void resume()
	{
		togglePause();
	}

	private void togglePause()
	{
		if (paused)
		{
			Time.timeScale = 1.0f;
			canvas.SetActive(false);
			paused = false;

		}else
		{
			Time.timeScale = 0.0f;
			canvas.SetActive(true);
			paused = true;
		}
	}
}
