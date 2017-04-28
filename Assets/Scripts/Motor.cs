using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public static class GameObjectExtension {

	public static void SetLayer(this GameObject parent, int layer, bool includeChildren = true)
	{
		parent.layer = layer;
		if (includeChildren)
		{
			foreach (Transform trans in parent.transform.GetComponentsInChildren<Transform>(true))
			{
				trans.gameObject.layer = layer;
			}
		}
	}
}

[RequireComponent(typeof(Rigidbody))]
public class Motor : NetworkBehaviour {

	// Use this for initialization
	public Vector3 _velocity = Vector3.zero;
	public Vector3 _rotation = Vector3.zero;
	public Vector3 _cameraRotation = Vector3.zero;

	[SerializeField]
	Transform camAxe;
	[SerializeField]
	Transform cameraHolder;
	Rigidbody rb;

	[SerializeField]
	bool spawnmode = false;

	public Transform CameraHolder 
	{
		get{
			return cameraHolder;
		}
	}

	Collider collider;

	public bool SpawnMode
	{
		get { return spawnmode; }
		set {
			spawnmode = value;
			if (value) 
			{
				this.gameObject.SetLayer (10);
				spawnModeTimer = 1f;
			} else 
			{
				this.gameObject.SetLayer (9);
			}
		}
	}

	public float spawnModeTimer = 1f;

	void Awake()
	{
		rb = GetComponent<Rigidbody> ();
		collider = GetComponent<Collider> ();
	}

	void Start () {
		//if (isLocalPlayer) {
		RichString test = "Motor Start...";
		SpawnMode = true;
		Console.Instance.AddMessage (test);
		FollowPlayer fp = Camera.main.GetComponent<FollowPlayer> ();
		fp.follow = cameraHolder;
		//Camera.main.transform.SetParent (cameraHolder, false);
		//Camera.main.transform.localPosition = Vector3.zero;
		//Camera.main.transform.rotation = cameraHolder.rotation;
		Console.Instance.AddMessage ("camera Grabbed ...");
		//}
		Spawn(PlayerTeam.Instance.team);

	}

	public override void OnStartLocalPlayer ()
	{
		Console.Instance.AddMessage ("OnStartLocalPlayer...");
		//GetComponentInChildren<Renderer> ().material.color = Color.blue;
	}

	public void Spawn(int team)
	{
		if (isLocalPlayer) {
			SpawnMode = true;
			Console.Instance.AddMessage ("Spawning");
			Vector3 spawn = SpawnManager.Instance.GetSpawnPoint (team);
			rb.position = spawn;
		}
	}

	// Update is called once per frame
	void Update () {
		if (spawnmode && spawnModeTimer > 0) 
		{
			spawnModeTimer -= Time.deltaTime;
			if (spawnModeTimer <= 0)
				SpawnMode = false;
		}
		if (Input.GetKeyDown (KeyCode.Keypad0)) 
		{
			appliqueForce (Random.insideUnitSphere.normalized);
		}
	}

	public void Move (Vector3 direction)
	{
		_velocity = direction;
	}

	public void Rotate(Vector3 rotation)
	{
		_rotation = rotation;	
	}

	void FixedUpdate()
	{
		AppliqueMouvement ();
		AppliqueRotation ();
	}

	public void appliqueForce(Vector3 dir)
	{
		rb.AddForce (dir * 10f, ForceMode.Impulse);
	}

	[Server]
	public void AppliqueExplosion(float force,Vector3 position,float radius)
	{
		RpcExplosion (force, position, radius);
		/*if (isLocalPlayer) {
			Console.Instance.AddMessage ("explosion force");
			//rb.AddExplosionForce (force, position, radius,1,ForceMode.Impulse);
			//RpcExplosion (force, position, radius);
		}*/
	}
		
	[ClientRpc]
	public void RpcExplosion(float force,Vector3 position,float radius)
	{
		if (isLocalPlayer)
			rb.AddExplosionForce (force, position, radius,1,ForceMode.Impulse);
	}

	void AppliqueMouvement()
	{
		if (_velocity != Vector3.zero)
			rb.MovePosition (rb.position + _velocity * Time.fixedDeltaTime);
	}

	void AppliqueRotation()
	{
		rb.MoveRotation (rb.rotation*Quaternion.Euler(_rotation));
		if (camAxe != null) {
			camAxe.transform.Rotate (-_cameraRotation);
		}
	}

	public void RotateCamera(Vector3 cameraRotation)
	{
		_cameraRotation = cameraRotation;
	}

	void OnTriggerEnter(Collider other)
	{
		if (!isServer)
			return;
		Console.Instance.AddMessage ("player Trigger enter");
		if (other.tag == "SpawnPoint") {
			Spawner s = other.GetComponent<Spawner> ();
			s.CmdBlock ();
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (!isServer)
			return;
		Console.Instance.AddMessage ("player Trigger Exit");
		if (other.tag == "SpawnPoint") {
			Spawner s = other.GetComponent<Spawner> ();
			s.CmdUnBlock ();
		}
	}
}
