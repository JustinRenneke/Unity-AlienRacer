using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverCarHealth : MonoBehaviour {

	public float startingHealth = 150;            // The amount of health the player starts the game with.
	public static float currentHealth = 150;                   // The current health the player has.
	private bool dead = false;
	private float healthPercent;
	private int healthPercentText = 100;
	public Text healthPercentField;
	public Slider slider;
	public Text healthPercentFieldVR;
	public Slider sliderVR;
	public Text healthPercentFieldFP;
	public Slider sliderFP;
	public Text healthPercentFieldFPVR;
	public Slider sliderFPVR;

	public ParticleSystem laserParticles;
	public ParticleSystem deathParticles;                // Reference to the particle system that plays when the enemy is damaged.
	public SkinnedMeshRenderer mesh;
	//private GameObject bar;
	//private UIBarScript barScript;

	private void Start()
	{
		currentHealth = 150;
		healthPercent = currentHealth / startingHealth;
		//healthPercentText = currentHealth / startingHealth * 100;
		if (healthPercentField != null)
		{
			healthPercentField.text = healthPercentText + "%";
		}
		if (healthPercentFieldFP != null)
		{
			healthPercentFieldFP.text = healthPercentText + "%";
		}
		if (healthPercentFieldVR != null)
		{
			healthPercentFieldVR.text = healthPercentText + "%";
		}
		if (healthPercentFieldFPVR != null)
		{
			healthPercentFieldFPVR.text = healthPercentText + "%";
		}
		//bar = GameObject.Find("/MizzouRacer/Main Camera/UI Camera/Canvas/HealthBarObject/HealthBar");
		//barScript = bar.GetComponent<UIBarScript>();
		//barScript.UpdateValue(currentHealth, startingHealth);
	}

	void Update()
	{
		/*
		if (Input.GetKey(KeyCode.Q)){
			currentHealth--;
			laserParticles.Play();
			healthPercent = currentHealth / startingHealth;
			//healthPercentText = (int)currentHealth / startingHealth * 100;
			//Debug.Log(healthPercent);
			healthPercentField.text = (int)(healthPercent * 100) + "%";
			healthPercentFieldFP.text = (int)(healthPercent * 100) + "%";
			healthPercentFieldVR.text = (int)(healthPercent * 100) + "%";
			healthPercentFieldFPVR.text = (int)(healthPercent * 100) + "%";
			slider.value = healthPercent;
			sliderVR.value = healthPercent;
			sliderFP.value = healthPercent;
			sliderFPVR.value = healthPercent;
			//barScript.UpdateValue(currentHealth, startingHealth);
		}
		*/
		//if (TripodShooting.inTrigger == true)
		if (GameState.beingShot == true)
		{
			currentHealth--;
			laserParticles.Play();
			healthPercent = currentHealth / startingHealth;
			//healthPercentText = currentHealth / startingHealth * 100;
			healthPercentField.text = (int)(healthPercent * 100) + "%";
			healthPercentFieldFP.text = (int)(healthPercent * 100) + "%";
			healthPercentFieldVR.text = (int)(healthPercent * 100) + "%";
			healthPercentFieldFPVR.text = (int)(healthPercent * 100) + "%";
			slider.value = healthPercent;
			sliderVR.value = healthPercent;
			sliderFP.value = healthPercent;
			sliderFPVR.value = healthPercent;
			//barScript.UpdateValue(currentHealth, startingHealth);
		}
		else
		{
			laserParticles.Stop();
		}


		if(currentHealth <= 0 && !dead)
		{
			dead = true;
			laserParticles.Stop();
			DeathScream.playDeathScreamCar();
			deathParticles.Play();
			mesh.enabled = false;
			Invoke("EndLevel", 1.9f);
		}
	}

	void EndLevel()
	{
		Application.LoadLevel("InvasionStageFailMenu");
	}
}
