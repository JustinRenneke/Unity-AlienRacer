using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GhostCar : MonoBehaviour {
	private int i = 0;
	private int j = 0;
	private int k = 0;

	[SerializeField]private List<float> positionX;
	[SerializeField]private List<float> positionY;
	[SerializeField]private List<float> positionZ;
	[SerializeField]private List<float> rotationX;
	[SerializeField]private List<float> rotationY;
	[SerializeField]private List<float> rotationZ;
	[SerializeField]private List<float> rotationW;
	[SerializeField]private List<float> leftParticleAngle;
	[SerializeField]private List<float> rightParticleAngle;
	[SerializeField]private List<float> leftParticleLifetime;
	[SerializeField]private List<float> rightParticleLifetime;

	private List<Vector3> position;
	private List<Quaternion> rotation;

	public GameObject leftEngineParticle;
	ParticleSystem leftEngineParticles;
	public GameObject rightEngineParticle;
	ParticleSystem rightEngineParticles;

	// To record a run of data, set run = true, loadData = false, and saveData = true -- otherwise all should be false here
	public static bool run = false;
	public bool loadData = false;	// may need to check this on the car in inspector
	private bool saveData = false;
	private string saveDirectory;
	void Start () {
		i = 0;
		j = 0;
		k = 0;
		
		Debug.Log("run? " + run);
		Debug.Log("opponent? " + GameState.opponent);
		Debug.Log("invasionGhost? " + GameState.invasionGhost);

		if (GameState.opponent == true)
		{
			loadData = true;
		}

		if (loadData == false)
		{
			gameObject.SetActive(false);
		}

		saveDirectory = Application.dataPath + "/Resources/";

//		Debug.Log("application path: " + Application.dataPath);

		position = new List<Vector3>();
		rotation = new List<Quaternion>();

		leftEngineParticles = leftEngineParticle.GetComponent<ParticleSystem>();
		rightEngineParticles = rightEngineParticle.GetComponent<ParticleSystem>();

		if (loadData == true)
		{
			Debug.Log("Loading data...");
			// Load transform position data
			GhostData.storedTransformX = loadList("positionX");
			GhostData.storedTransformY = loadList("positionY");
			GhostData.storedTransformZ = loadList("positionZ");

			// Load transform rotation data
			GhostData.storedQuaternionX = loadList("rotationX");
			GhostData.storedQuaternionY = loadList("rotationY");
			GhostData.storedQuaternionZ = loadList("rotationZ");
			GhostData.storedQuaternionW = loadList("rotationW");

			// Load particle system data
			GhostData.storedRightParticleAngle = loadList("rightParticleAngle");
			GhostData.storedLeftParticleAngle = loadList("leftParticleAngle");
			GhostData.storedRightParticleLifetime = loadList("rightParticleLifetime");
			GhostData.storedLeftParticleLifetime = loadList("leftParticleLifetime");

			//Debug.Log("points same object? " + Object.ReferenceEquals(GhostData.storedTransformX, GhostData.storedRightParticleAngle));
		}

		if (run == true || loadData == true || GameState.invasionGhost == true || GhostData.invasionGhost == true)
		{
			Debug.Log("Ghost Car SETACTIVE");
			gameObject.SetActive(true);

			// Assemble stored position floats into vectors
			foreach (float x in GhostData.storedTransformX)
			{
				positionX.Add(GhostData.storedTransformX[i]);
				positionY.Add(GhostData.storedTransformY[i]);
				positionZ.Add(GhostData.storedTransformZ[i]);

				rotationX.Add(GhostData.storedQuaternionX[i]);
				rotationY.Add(GhostData.storedQuaternionY[i]);
				rotationZ.Add(GhostData.storedQuaternionZ[i]);
				rotationW.Add(GhostData.storedQuaternionW[i]);

				rightParticleAngle.Add(GhostData.storedRightParticleAngle[i]);
				leftParticleAngle.Add(GhostData.storedLeftParticleAngle[i]);
				leftParticleLifetime.Add(GhostData.storedLeftParticleLifetime[i]);
				rightParticleLifetime.Add(GhostData.storedRightParticleLifetime[i]);
				// Assemble transform position vector
				position.Add(new Vector3(GhostData.storedTransformX[i], GhostData.storedTransformY[i], GhostData.storedTransformZ[i]));
				// Assemble transform rotation Quaternion
				rotation.Add(new Quaternion(GhostData.storedQuaternionX[i], GhostData.storedQuaternionY[i], GhostData.storedQuaternionZ[i], GhostData.storedQuaternionW[i]));
				i++;
			}

			if (saveData == true)
			{
				//Debug.Log("size of positionX before saving: " + positionX.Count);
				// Save transform position data
				saveList(positionX, "positionX");
				saveList(positionY, "positionY");
				saveList(positionZ, "positionZ");

				saveList(rotationX, "rotationX");
				saveList(rotationY, "rotationY");
				saveList(rotationZ, "rotationZ");
				saveList(rotationW, "rotationW");

				saveList(rightParticleAngle, "rightParticleAngle");
				saveList(leftParticleAngle, "leftParticleAngle");
				saveList(rightParticleLifetime, "rightParticleLifetime");
				saveList(leftParticleLifetime, "leftParticleLifetime");
			}

			InvokeRepeating("updateTransform", 0f, .01f);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void updateTransform()
	{
		var leftPsMain = leftEngineParticles.main;
		var rightPsMain = rightEngineParticles.main;
		var leftPsShape = leftEngineParticles.shape;
		var rightPsShape = rightEngineParticles.shape;

		if (j < GhostData.storedTransformX.Count)
		{
			transform.position = position[j];
			transform.rotation = rotation[j];
			rightPsShape.angle = GhostData.storedRightParticleAngle[j];
			leftPsShape.angle = GhostData.storedLeftParticleAngle[j];
			leftPsMain.startLifetimeMultiplier = GhostData.storedLeftParticleLifetime[j];
			rightPsMain.startLifetimeMultiplier = GhostData.storedRightParticleLifetime[j];
			j++;
		}
		else
		{
			gameObject.SetActive(false);
		}
	}

	private void saveList(List<float> list, string filename)
	{
		Debug.Log("saving...");
		BinaryFormatter bf = new BinaryFormatter();
		//FileStream file = File.Create(Application.persistentDataPath + "/"+filename+".gd");
		FileStream file = File.Create(Application.persistentDataPath + "/" + filename + ".bytes");
		bf.Serialize(file, list);
		file.Close();
	}

	private List<float> loadList(string filename)
	{
		List<float> temp;
		//Load texture from disk
		TextAsset bindata = Resources.Load(filename, typeof(TextAsset)) as TextAsset;
		//TextAsset bindata = Resources.Load("/RaceSaveData/" + filename, typeof(TextAsset)) as TextAsset;
		Stream stream = new MemoryStream(bindata.bytes);
		BinaryFormatter bf = new BinaryFormatter();
		temp = (List<float>)bf.Deserialize(stream);
		return temp;

		/*
		if (File.Exists(saveDirectory + filename + ".gd"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(saveDirectory + filename + ".bytes", FileMode.Open);
			temp = (List<float>)bf.Deserialize(file);
			file.Close();
			return temp;
		}else
		{
			return null;
		}*/
	}

	public void SetLoadData(bool state)
	{
		loadData = state;
	}
	
}
