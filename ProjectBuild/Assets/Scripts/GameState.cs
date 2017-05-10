using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {
	// Race stage stats
	public static string raceTime = "";
	public static string racePortalsClosed= "";

	// Invasion stage stats
	public static string aliensKilled = "";
	public static string timeRemaining = "";
	public static string portalsRemaining = "";

	public static bool opponent = false;
	public static bool demo = false;
	public static bool invasionGhost = false;

	public static int portalsKilled = 0;
	public static int tripodsKilled = 0;
	public static bool beingShot = false;
}
