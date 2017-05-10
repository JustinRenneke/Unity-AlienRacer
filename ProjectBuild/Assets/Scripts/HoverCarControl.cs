using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.VR;

[RequireComponent(typeof(Rigidbody))]
public class HoverCarControl : MonoBehaviour
{
	Rigidbody body;
	// If we want to use a joystick based controller, check a variable like this to allow for poor stick calibration
	//float deadZone = 0.1f;

	public float hoverForce = 1000.0f;
	public float hoverCompensationForce = 1000.0f;
	public float hoverHeight = 1.5f;
	public GameObject[] hoverPoints;

	public float forwardAcl = 10000.0f;
	public float backwardAcl = 2500.0f;
	public static float currThrust = 0.0f;
	private float dot;

	public float turnStrength = 1000f;
	float currTurn = 0.0f;
	int layerMask;

	public float jumpStrength = 1000000.0f;
	private float jumpCooldown = 5.0f;
	private float timeSinceLastJump;

	public GameObject leftEngine;
	public GameObject leftEngineParticle;
	ParticleSystem leftEngineParticles;
	public GameObject rightEngine;
	public GameObject rightEngineParticle;
	ParticleSystem rightEngineParticles;
	public float enginePower = 10000.0f;
	public float maxThrustMultiplier = 3.0f;
	public float maxHoverMultiplier = 4.0f;
	public float thrustReductionMultiplier;
	public static float globalMaxThrustMultiplier;
	private float specialMoveThreshold;
	private bool leftThrust = false;
	private bool rightThrust = false;
	public static float thrustMultiplier = 0.0f;
	public static float rightThrustMultiplier = 0.0f;
	public static float leftThrustMultiplier = 0.0f;
	private float hoverMultiplier = 1.0f;
	public static float currentSpeed;
	private Vector3 previousSpeed;
	public float powerSteeringFactor;

	private Gradient originalGradient;
	private Gradient specialMoveGradient;
	public GameObject leftEngineSpecialCollider;
	public GameObject rightEngineSpecialCollider;

	public GameObject mainCamera;
	public GameObject mainCameraVR;
	public GameObject firstPersonCamera;
	public GameObject rearViewCamera;
	private int cameraToggle;

	private Quaternion initialRotation;

	public ParticleSystem pulse;
	public ParticleSystem pulsefp;
	public ParticleSystem pulseVR;
	public GameObject pulseCollider;
	public AudioSource pulseSound;
	public AudioClip pulseClip;



	void Start()
	{
		initialRotation = transform.rotation;
		thrustMultiplier = 0.0f;
		leftThrustMultiplier = 0.0f;
		rightThrustMultiplier = 0.0f;
		currentSpeed = 0;
		leftThrust = false;
		rightThrust = false;
		specialMoveThreshold = maxThrustMultiplier - maxThrustMultiplier * .2f;
		globalMaxThrustMultiplier = maxThrustMultiplier;
		body = GetComponent<Rigidbody>();
		leftEngineParticles = leftEngineParticle.GetComponent<ParticleSystem>();
		rightEngineParticles = rightEngineParticle.GetComponent<ParticleSystem>();
		leftEngineParticles.Stop();
		rightEngineParticles.Stop();

		timeSinceLastJump = Time.timeSinceLevelLoad;

		// Set up some gradients to change the color of our thruster emissions later
		originalGradient = new Gradient();
		originalGradient.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.magenta, 0.5f), new GradientColorKey(Color.cyan, 1.0f) },
									new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f), new GradientAlphaKey(1.0f, 1.0f) });
		specialMoveGradient = new Gradient();
		specialMoveGradient.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.red, 1.0f) },
								new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f), new GradientAlphaKey(1.0f, 1.0f) });

		/* Set a layermask for characters then invert it.
		We will assign the hover car (and its camera) to the Characters layermask and use it in our hover force applications.
		This way, we hit everything except the things on the Characters layer (i.e. not ourselves) when we apply our physics 
		forces to achieve the hovering effect.
		*/
		//layerMask = 1 << LayerMask.NameToLayer("Characters");
		layerMask = LayerMask.GetMask("Characters", "TripodLights");
		layerMask = ~layerMask;

		// Set up cameras
		if (VRDevice.isPresent) {
			mainCameraVR.SetActive (true);
			mainCamera.SetActive (false);
		} else {
			mainCameraVR.SetActive (false);
			mainCamera.SetActive (true);
		}
		firstPersonCamera.SetActive(false);
		rearViewCamera.SetActive(false);
		cameraToggle = 0;

		pulseCollider.SetActive(false);

		InvokeRepeating("AddCarState", 0f, 0.01f);
	}

	void Update()
	{
		thrustMultiplier = Mathf.Max(leftThrustMultiplier, rightThrustMultiplier);
		// Get all the particle system components we need to work with
		var leftPsMain = leftEngineParticles.main;
		var rightPsMain = rightEngineParticles.main;
		var leftPsShape = leftEngineParticles.shape;
		var rightPsShape = rightEngineParticles.shape;
		var leftPsColorSpeed = leftEngineParticles.colorBySpeed;
		var rightPsColorSpeed = rightEngineParticles.colorBySpeed;

		// Hover powered thrust
		currThrust = 0.0f;
		float aclAxis = Input.GetAxis("Vertical");
		if (aclAxis > 0)
		{
			//move forward
			currThrust = aclAxis * forwardAcl;
			leftEngineParticles.Play();
			rightEngineParticles.Play();
		}
		else if (aclAxis < 0)
		{   //move backwards
			currThrust = aclAxis * backwardAcl;
			thrustMultiplier = 0.0f;
		}
		else if (aclAxis == 0 && thrustMultiplier == 0)
		{
			leftEngineParticles.Stop();
			rightEngineParticles.Stop();
		}

		// Turning with hover power
		currTurn = 0.0f;
		float turnAxis = Input.GetAxis("Horizontal");
		if (Mathf.Abs(turnAxis) > 0)
		{
			currTurn = turnAxis;
		}

		// Build thrust multiplier
		if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
		{
			if (leftThrustMultiplier <= maxThrustMultiplier || rightThrustMultiplier <= maxThrustMultiplier)
			{
				// If user activates both thrusters
				if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.LeftArrow)) && (Input.GetMouseButton(1) || Input.GetKey(KeyCode.RightArrow)))
				{
					if (leftThrustMultiplier < maxThrustMultiplier)
					{
						leftThrustMultiplier += Time.deltaTime;
					}
					if (rightThrustMultiplier < maxThrustMultiplier)
					{
						rightThrustMultiplier += Time.deltaTime;
					}
				}
				else
				{
					// if user is activating right thrust
					if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.LeftArrow))
					{
						if (leftThrustMultiplier < maxThrustMultiplier)
						{
							leftThrustMultiplier += Time.deltaTime;
						}
					}
					else
					{
						if (leftThrustMultiplier > 0f)
						{
							leftThrustMultiplier -= Time.deltaTime * thrustReductionMultiplier;
						}
					}

					if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.RightArrow))
					{
						if (rightThrustMultiplier < maxThrustMultiplier)
						{
							rightThrustMultiplier += Time.deltaTime;
						}
					}
					else
					{
						if (rightThrustMultiplier > 0)
						{
							rightThrustMultiplier -= Time.deltaTime * thrustReductionMultiplier;
						}
					}
				}
				// compensate for the increased forward velocity by increasing hover force to maintain balance
				if (hoverMultiplier < maxHoverMultiplier)
				{
					hoverMultiplier += Time.deltaTime;
				}
			}
			if (leftThrustMultiplier > maxThrustMultiplier)
			{
				leftThrustMultiplier = maxThrustMultiplier;
			}
			if (rightThrustMultiplier > maxThrustMultiplier)
			{
				rightThrustMultiplier = maxThrustMultiplier;
			}
			// If thrust multiplier is above 80% of max value, distort thrust emission
			if (thrustMultiplier >= specialMoveThreshold && !Input.GetKey(KeyCode.E))
			{
				rightPsShape.angle = 7f;
				leftPsShape.angle = 7f;
			}
		}
		else if (thrustMultiplier <= 0f)
		{
			thrustMultiplier = 0.0f;
			rightThrustMultiplier = 0;
			leftThrustMultiplier = 0;
			hoverMultiplier = 1.0f;
			rightPsShape.angle = 0f;
			leftPsShape.angle = 0f;
		}
		else    // else we are slowly scaling down thrustMultiplier effects
		{
			if (thrustMultiplier > 0f)
			{
				if (!Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftArrow))
				{
					leftThrustMultiplier -= Time.deltaTime * thrustReductionMultiplier;
				}
				if (!Input.GetMouseButton(1) && !Input.GetKey(KeyCode.RightArrow))
				{
					rightThrustMultiplier -= Time.deltaTime * thrustReductionMultiplier;
				}
				// compensate for the decreased forward velocity by decreasing hover force to maintain balance
				if (hoverMultiplier > 1)
				{
					hoverMultiplier -= Time.deltaTime;
				}
			}

			// Return thrust shape to default when thrustMultiplier falls below 80% of max
			if (thrustMultiplier < specialMoveThreshold && !Input.GetKey(KeyCode.E))
			{
				rightPsShape.angle = 0f;
				leftPsShape.angle = 0f;
			}
		}

		leftPsMain.startLifetimeMultiplier = leftThrustMultiplier * .2f;
		rightPsMain.startLifetimeMultiplier = rightThrustMultiplier * .2f;
		if(thrustMultiplier < specialMoveThreshold)
		{
			rightPsShape.angle = 0f;
			leftPsShape.angle = 0f;
		}

		// If the user is activating either the right or left thrust and the thrust is not in its default state
		if (thrustMultiplier > .1f && ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.LeftArrow)) ||
			(Input.GetMouseButton(1) || Input.GetKey(KeyCode.RightArrow))))
		{
			//leftPsMain.startLifetimeMultiplier = leftThrustMultiplier * .2f;
			//rightPsMain.startLifetimeMultiplier = rightThrustMultiplier * .2f;
			// if user is activating left thrust
			if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.LeftArrow))
			{
				leftThrust = true;
				leftEngineParticles.Play();
			}

			// if user is activating right thrust
			if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.RightArrow))
			{
				rightThrust = true;
				rightEngineParticles.Play();
			}

			if (!Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftArrow))
			{
				leftThrust = false;
			}
			if (!Input.GetMouseButton(1) && !Input.GetKey(KeyCode.RightArrow))
			{
				rightThrust = false;
			}
		}
		else if (thrustMultiplier == 0.0f)
		{
			leftPsMain.startLifetimeMultiplier = .1f;
			rightPsMain.startLifetimeMultiplier = .1f;
		}

		// Perform thruster overburn special move
		if ((Input.GetKey(KeyCode.LeftShift) || (Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.Mouse0) ||
			Input.GetKey(KeyCode.LeftArrow))) || (Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.Mouse1) ||
			Input.GetKey(KeyCode.RightArrow))))
			&& (thrustMultiplier >= specialMoveThreshold))
		{
			// temporarily revert cone to default shape for special move
			rightPsShape.angle = 0f;
			leftPsShape.angle = 0f;
			//Debug.Log("Special Move ready!");
			if ((Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.Mouse0) ||
			Input.GetKey(KeyCode.LeftArrow))))
			{
				leftPsColorSpeed.color = specialMoveGradient;   //change color of thrust
				leftPsMain.startLifetimeMultiplier = leftThrustMultiplier * 1f;     //increase length of exhaust
																					//Debug.Log(leftPsMain.startLifetimeMultiplier);
				leftEngineSpecialCollider.SetActive(true);  //activate thrust collider to destroy enemies
			}

			if ((Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.Mouse1) ||
			Input.GetKey(KeyCode.RightArrow))))
			{
				rightPsColorSpeed.color = specialMoveGradient;
				rightPsMain.startLifetimeMultiplier = rightThrustMultiplier * 1f;
				rightEngineSpecialCollider.SetActive(true);
			}
		}
		else
		{
			// revert settings after special move use is over
			leftPsColorSpeed.color = originalGradient;
			rightPsColorSpeed.color = originalGradient;
			leftEngineSpecialCollider.SetActive(false);
			rightEngineSpecialCollider.SetActive(false);
		}

		
		if (Input.GetKeyDown(KeyCode.F))
		{
			transform.Translate(Vector3.up * 90);
			//InvasionStageUIController.portalsKilled = 5;
			//GameState.portalsKilled = 5;
		}
		
		if (Input.GetKeyDown(KeyCode.I)){
			Application.LoadLevel("InvasionStage");
		}
		if (Input.GetKeyDown(KeyCode.O)){
			Application.LoadLevel("RaceStage");
		}
		if (Input.GetKeyDown(KeyCode.E) && (thrustMultiplier >= specialMoveThreshold))
		{
			if (mainCamera.activeInHierarchy)
			{
				pulse.Play();
				pulseSound.PlayOneShot(pulseClip);
				pulseCollider.SetActive(true);
				InvokeRepeating("PulsePenalty", 0f, .1f);
				Invoke("CancelPulseRepeaters", 8);
				Invoke("DisablePulseCollider", 1);//this will happen after 1 seconds
			}
			else if (firstPersonCamera.activeInHierarchy)
			{
				pulsefp.Play();
				pulseSound.PlayOneShot(pulseClip);
				pulseCollider.SetActive(true);
				InvokeRepeating("PulsePenalty", 0f, .1f);
				Invoke("CancelPulseRepeaters", 8);
				Invoke("DisablePulseCollider", 1);
			}
			else if (mainCameraVR.activeInHierarchy)
			{
				pulseVR.Play();
				pulseSound.PlayOneShot(pulseClip);
				pulseCollider.SetActive(true);
				InvokeRepeating("PulsePenalty", 0f, .1f);
				Invoke("CancelPulseRepeaters", 8);
				Invoke("DisablePulseCollider", 1);
			}
			leftThrustMultiplier = 0f;
			rightThrustMultiplier = 0f;
			thrustMultiplier = 0f;
		}

			if (Input.GetKeyDown(KeyCode.Space) && Time.timeSinceLevelLoad - timeSinceLastJump > jumpCooldown)
		{
			body.AddForce(transform.up * jumpStrength * (thrustMultiplier + 1));
			timeSinceLastJump = Time.timeSinceLevelLoad;
		}

		if (Input.GetKeyDown(KeyCode.C))
		{
			SwapCamera();
		}

	}

	void FixedUpdate()
	{
		if (currThrust > 0)
		{
			PowerSteer();
		}
		if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.LeftArrow)) && (Input.GetMouseButton(1) || Input.GetKey(KeyCode.RightArrow)))
		{

			PowerSteer();
		}
		// calulate speed
		currentSpeed = ((transform.position - previousSpeed).magnitude) / Time.deltaTime;
		previousSpeed = transform.position;

		// Try to prevent nosedives when the car gets stuck on bad geometry
		dot = Vector3.Dot(transform.up, Vector3.up);
		//Debug.Log("current dot: " + dot);
		//Debug.Log("current transform.up: " + transform.up);
		// If the dot product of the car's vector and the vertical reference vector is < .4 we're probably heading towards a flip
		if (dot < .4f)
		{
			//Debug.Log("current (<.4) dot: " + dot);
			// Confirm it's a forward facing flip
			if (Mathf.Abs(transform.up.x) > Mathf.Abs(transform.up.y))
			{

				transform.up = new Vector3(initialRotation.x, transform.up.y, transform.up.z);
				//transform.up = new Vector3(transform.up.x, 0, transform.up.z);
				//transform.up = Vector3.up;
				leftThrustMultiplier = 0;
				rightThrustMultiplier = 0;
				thrustMultiplier = 0;
			}
		}
		
		//  Create hover effect by applying force at each hover point
		RaycastHit hit;
		for (int i = 0; i < hoverPoints.Length; i++)
		{
			var hoverPoint = hoverPoints[i];
			// Cast a ray downwards towards the ground from the hoverpoint and measure the distance from the hoverpoint to the ground
			// If the raycast intersects something (i.e. the ground)
			if (Physics.Raycast(hoverPoint.transform.position, -Vector3.up, out hit, hoverHeight, layerMask))
			{
				// Push away from the ground with an amount of force that is dependent on current height of the car from the ground
				body.AddForceAtPosition(Vector3.up * hoverForce * (1.0f - (hit.distance / hoverHeight)), hoverPoint.transform.position);
			}
			// Else the raycast for the current hoverpoint didn't intersect anything, so we are starting to flip and need to compensate
			else
			{
				// If the hover point is below the center of the car, we need to apply force upwards to compensate and level the car out
				if (transform.position.y > hoverPoint.transform.position.y)
					body.AddForceAtPosition(hoverPoint.transform.up * hoverForce, hoverPoint.transform.position);
				// Else the hover point is above the center of the car and we need to apply force downwards to compensate
				else
					body.AddForceAtPosition(hoverPoint.transform.up * -hoverCompensationForce, hoverPoint.transform.position);
			}
		}

		// Move forward or backward by application of force
		if (Mathf.Abs(currThrust) > 0)
			body.AddForce(transform.forward * currThrust);  //currThrust is user input forward/backward * base acceleration settings

		// Turn the hover car via application of torque
		// applying torque in an upward direction (with positive currTurn variable) turns to the right
		// applying torque in a downward direction (with negative currTurn variable) turns to the left
		body.AddRelativeTorque(Vector3.up * currTurn * turnStrength);

		// Apply left / right engine thrust at variable rate (up to max speed) depending on how long mouse button has been down
		if (leftThrust)
		{
			body.AddForceAtPosition(transform.forward * enginePower * leftThrustMultiplier, leftEngine.transform.position);

		}
		if (rightThrust)
		{
			body.AddForceAtPosition(transform.forward * enginePower * rightThrustMultiplier, rightEngine.transform.position);
		}

	}

	private void SwapCamera()
	{
		if (cameraToggle < 2)
		{
			cameraToggle++;
		}
		else
		{
			cameraToggle = 0;
		}

		if (cameraToggle == 0)
		{
			if (VRDevice.isPresent) {
				mainCameraVR.SetActive (true);
				mainCamera.SetActive (false);
			} else {
				mainCameraVR.SetActive (false);
				mainCamera.SetActive (true);
			}
			firstPersonCamera.SetActive(false);
			rearViewCamera.SetActive(false);
		}
		else if (cameraToggle == 1)
		{
			mainCamera.SetActive(false);
			mainCameraVR.SetActive (false);
			firstPersonCamera.SetActive(true);
			rearViewCamera.SetActive(false);
		}
		else if (cameraToggle == 2)
		{
			mainCamera.SetActive(false);
			mainCameraVR.SetActive (false);
			firstPersonCamera.SetActive(false);
			rearViewCamera.SetActive(true);
		}

	}

	/* Stabilizing rotation after an imbalance in thruster charge has become necessary after changing thruster downcharge to 
	 * follow a linearly decreasing curve instead of instantly going to 0 to slowly scale down thruster force application, so we have 
	 * to apply a 'powerSteeringFactor' to assist in stabilizing the car when trying to move forward again. Without this the car is 
	 * incredibly difficult to control because the smallest power imbalance between the thrusters causes severe horizontal rotation due
	 *  to the effects of torque on an essentially frictionless object.
	*/
	void PowerSteer()
	{
		if (body.angularVelocity.magnitude > 4)
		{
			body.angularVelocity *= (powerSteeringFactor - .2f);
		}
		else if (body.angularVelocity.magnitude > 3)
		{
			body.angularVelocity *= (powerSteeringFactor - .1f);
		}
		else if (body.angularVelocity.magnitude > 2)
		{
			body.angularVelocity *= (powerSteeringFactor);
		}
		else if (body.angularVelocity.magnitude > 1.8)
		{
			body.angularVelocity *= (powerSteeringFactor + .2f);
		}
		else if (body.angularVelocity.magnitude > 1.6)
		{
			body.angularVelocity *= (powerSteeringFactor + .4f);
		}
			
	}

	void AddCarState()
	{
		GhostData.newTransformX.Add(transform.position.x);
		GhostData.newTransformY.Add(transform.position.y);
		GhostData.newTransformZ.Add(transform.position.z);

		GhostData.newQuaternionX.Add(transform.rotation.x);
		GhostData.newQuaternionY.Add(transform.rotation.y);
		GhostData.newQuaternionZ.Add(transform.rotation.z);
		GhostData.newQuaternionW.Add(transform.rotation.w);

		GhostData.newLeftParticleAngle.Add(leftEngineParticles.shape.angle);
		GhostData.newRightParticleAngle.Add(rightEngineParticles.shape.angle);

		GhostData.newLeftParticleLifetime.Add(leftEngineParticles.main.startLifetimeMultiplier);
		GhostData.newRightParticleLifetime.Add(rightEngineParticles.main.startLifetimeMultiplier);
	}

	void DisablePulseCollider()
	{
		pulseCollider.SetActive(false);
	}

	void PulsePenalty()
	{
		leftThrustMultiplier = 0f;
		rightThrustMultiplier = 0f;
		thrustMultiplier = 0f;
	}

	void CancelPulseRepeaters()
	{
		CancelInvoke();
	}
}