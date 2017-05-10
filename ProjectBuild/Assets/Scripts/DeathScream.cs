using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScream : MonoBehaviour {

	public AudioSource scream;
	public static AudioSource screamStatic;
	public AudioClip screamClipCyan;
	private static AudioClip screamClipStaticCyan;
	public AudioClip screamClipYellow;
	private static AudioClip screamClipStaticYellow;
	public AudioClip screamClipWhite;
	private static AudioClip screamClipStaticWhite;
	public AudioClip screamClipGrey;
	private static AudioClip screamClipStaticGrey;
	public AudioClip tripodScream;
	private static AudioClip tripodScreamStatic;
	public AudioClip hoverCarScream;
	private static AudioClip hoverCarScreamStatic;

	public AudioClip portalDestroyed;
	private static AudioClip portalDestroyedStatic;

	void Start () {
		screamStatic = scream;
		screamClipStaticCyan = screamClipCyan;
		screamClipStaticYellow = screamClipYellow;
		screamClipStaticWhite = screamClipWhite;
		screamClipStaticGrey = screamClipGrey;
		tripodScreamStatic = tripodScream;
		hoverCarScreamStatic = hoverCarScream;

		portalDestroyedStatic = portalDestroyed;
	}

	public static void playDeathScreamCyan()
	{
		screamStatic.PlayOneShot(screamClipStaticCyan);
	}
	public static void playDeathScreamYellow()
	{
		screamStatic.PlayOneShot(screamClipStaticYellow);
	}
	public static void playDeathScreamWhite()
	{
		screamStatic.PlayOneShot(screamClipStaticWhite);
	}
	public static void playDeathScreamGrey()
	{
		screamStatic.PlayOneShot(screamClipStaticGrey);
	}
	public static void playDeathScreamTripod()
	{
		screamStatic.PlayOneShot(tripodScreamStatic);
	}

	public static void playDeathScreamPortal()
	{
		screamStatic.PlayOneShot(portalDestroyedStatic);
	}

	public static void playDeathScreamCar()
	{
		screamStatic.PlayOneShot(hoverCarScreamStatic);
	}
}
