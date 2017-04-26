using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class projectile : NetworkBehaviour {

	// Use this for initialization
	public LayerMask layerMask;

	[SyncVar]
	public Quaternion rotation;

	[SerializeField]
	float DestroyDelay = 5f;

	//[SerializeField]
	ParticleSystem ps;

	[SerializeField]
	float lifeTime = 5f;

	bool alive=true;

	[SyncVar]
	public int team = 0;

	[SerializeField]
	GameObject impactInstantiate;

	void Start () {
		ps = GetComponent<ParticleSystem> ();
		transform.rotation = rotation;
	}

	// Update is called once per frame
	void Update () {
		if (!isServer)
			return;
		if (!alive)
			return;
		lifeTime -= Time.deltaTime;
		if (lifeTime <= 0) {
			alive = false;
			RpcDisable ();
			Destroy (gameObject, DestroyDelay);
		}
	}


	void OnTriggerEnter(Collider other)
	{
		if (!isServer)
			return;
		if (!alive)
			return;
		
		if (layerMask == (layerMask | (1 << other.gameObject.layer))) {
			PlayerTeam pteam = other.GetComponent<PlayerTeam> ();
			if (pteam != null) {
				if (pteam.team == team)
					return;
			}
			Console.Instance.AddMessage (transform.name + "OnTriggerEnter("+ other.name +")");
			//Destroy (gameObject);
			alive=false;
			RpcDisable();
			Destroy (gameObject, DestroyDelay);
			if (impactInstantiate != null) {
				Vector3 position  = transform.position;
				//CmdFire (position,direction,rotation);
				CmdImpactInstantiate (position); 
			}
		}
	}

	[Command]
	void CmdImpactInstantiate(Vector3 position)
	{
		GameObject GO = (GameObject)Instantiate (impactInstantiate,position,Quaternion.identity);
		//GO.GetComponent<Rigidbody> ().velocity = direction * 2;
		//GO.GetComponent<projectile> ().rotation = rotation;
		GO.GetComponent<Explosion>().team = team;
		NetworkServer.Spawn (GO);
		Destroy (GO, 2);
	}

	[ClientRpc]
	public void RpcDisable()
	{
		Console.Instance.AddMessage (transform.name + " Rpc disable");
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
		if (ps != null) {
			var em = ps.emission;
			em.enabled = false;
		}
	}
}
