using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(Motor))]
public class Controller : NetworkBehaviour {

	// Use this for initialization
	[SerializeField]
	float speed = 5f;
	[SerializeField]
	float lookSensivity = 3;

	[SerializeField]
	GameObject projectile;

	Motor motor;
	Transform myTransform;
	Health health;
	PlayerTeam team;

	void Awake()
	{
		health = GetComponent<Health> ();
		team = GetComponent<PlayerTeam> ();
		motor = GetComponent<Motor> ();
		myTransform = GetComponent<Transform> ();
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// calcul mouvement
		if (health.dead)
			return;
		
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			Vector3 position  = motor.CameraHolder.position;
			Vector3 direction = motor.CameraHolder.forward;
			Quaternion rotation = motor.CameraHolder.rotation;
			CmdFire (position,direction,rotation);
		}

		float xMov = Input.GetAxisRaw ("Horizontal");
		float zMov = Input.GetAxisRaw ("Vertical");
		Vector3 movHonrizontal = myTransform.right   * xMov;
		Vector3 movVertical    = myTransform.forward * zMov;


		//float speedStat = speed * StatsManager.Instance.GetValue ("speed");
		float speedStat =speed;

		Vector3 velocity = (movHonrizontal + movVertical).normalized*speedStat;
		// pass mouvement to motor
		motor.Move (velocity);

		// calcul rotation Y (horizontal camera)
		//Cursor.lockState = CursorLockMode.Locked;
		if (Cursor.lockState == CursorLockMode.Locked) {
			float yRot = Input.GetAxisRaw ("Mouse X");
			Vector3 yRotation = new Vector3 (0, yRot, 0) * lookSensivity;
			motor.Rotate (yRotation);

			// calcul rotation z (vertical camera)
			float xRot = Input.GetAxisRaw ("Mouse Y");
			Vector3 xRotation = new Vector3 (xRot, 0, 0) * lookSensivity;
			motor.RotateCamera (xRotation);
		} 
		else
		{
			Vector3 yRotation = new Vector3 (0, 0, 0) * lookSensivity;
			motor.Rotate (yRotation);
			Vector3 xRotation = new Vector3 (0, 0, 0) * lookSensivity;
			motor.RotateCamera (xRotation);
		}

	}

	[Command]
	void CmdFire(Vector3 position,Vector3 direction,Quaternion rotation )
	{	
		//Vector3 position  = motor.CameraHolder.position;
		//Vector3 direction = motor.CameraHolder.forward;
		//Quaternion rotation = motor.CameraHolder.rotation;

		GameObject GO = (GameObject)Instantiate (projectile,position+direction,rotation);
		GO.GetComponent<Rigidbody> ().velocity = direction * 8;
		projectile p = GO.GetComponent<projectile> ();
		p.rotation = rotation;
		p.team = team.team;
		NetworkServer.Spawn (GO);
		//Destroy (GO, 4);
	}
}
