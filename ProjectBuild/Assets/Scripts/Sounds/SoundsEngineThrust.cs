using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsEngineThrust : MonoBehaviour {
	public AudioSource leftEngineAudioSource;
	public AudioSource rightEngineAudioSource;
	public float startingPitch = 0.5f;
	public float baseVolume = 0.5f;
	public float pitchScalingMultiplier = 3;
	public float volumeScalingMultiplier = 2;
	float leftStartVolume;
	float rightStartVolume;

	void Start () {
		leftEngineAudioSource.Stop();
		rightEngineAudioSource.Stop();
		leftEngineAudioSource.volume = baseVolume;
		rightEngineAudioSource.volume = baseVolume;
	}
	
	void Update () {
		if(leftEngineAudioSource.isPlaying && HoverCarControl.currThrust == 0 && HoverCarControl.leftThrustMultiplier == 0)
		{
			leftStartVolume = leftEngineAudioSource.volume;
			leftEngineAudioSource.volume -= leftStartVolume * Time.deltaTime / .05f;
			if (leftEngineAudioSource.volume <= .01f)
			{
				leftEngineAudioSource.Stop();
			}
		}
		else if (!leftEngineAudioSource.isPlaying && (HoverCarControl.leftThrustMultiplier > 0 || HoverCarControl.currThrust > 0))
		{
			leftEngineAudioSource.pitch = startingPitch;
			leftEngineAudioSource.volume = baseVolume;
			leftEngineAudioSource.Play();
		}
		if (rightEngineAudioSource.isPlaying && HoverCarControl.currThrust == 0 && HoverCarControl.rightThrustMultiplier == 0)
		{
			rightStartVolume = rightEngineAudioSource.volume;
			rightEngineAudioSource.volume -= rightStartVolume * Time.deltaTime / .05f;
			if (rightEngineAudioSource.volume <= .01f)
			{
				rightEngineAudioSource.Stop();
			}
		}
		else if(!rightEngineAudioSource.isPlaying && (HoverCarControl.rightThrustMultiplier > 0 || HoverCarControl.currThrust > 0))
		{
			rightEngineAudioSource.pitch = startingPitch;
			rightEngineAudioSource.volume = baseVolume;
			rightEngineAudioSource.Play();
		}
		if(HoverCarControl.currThrust > 0 && HoverCarControl.rightThrustMultiplier == 0 && HoverCarControl.leftThrustMultiplier == 0)
		{
			leftEngineAudioSource.pitch = startingPitch;
			leftEngineAudioSource.volume = baseVolume;
			rightEngineAudioSource.pitch = startingPitch;
			rightEngineAudioSource.volume = baseVolume;
		}
		if (HoverCarControl.leftThrustMultiplier > 0)
		{
			leftEngineAudioSource.pitch = startingPitch * HoverCarControl.leftThrustMultiplier * pitchScalingMultiplier;
			leftEngineAudioSource.volume = baseVolume * HoverCarControl.leftThrustMultiplier * volumeScalingMultiplier + baseVolume;
		}
		if (HoverCarControl.rightThrustMultiplier > 0)
		{
			rightEngineAudioSource.pitch = startingPitch * HoverCarControl.rightThrustMultiplier * pitchScalingMultiplier;
			rightEngineAudioSource.volume = baseVolume * HoverCarControl.rightThrustMultiplier * volumeScalingMultiplier + baseVolume;
		}
	}
}
