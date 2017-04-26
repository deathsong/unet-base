using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Spawner : NetworkBehaviour {

	// Use this for initialization
	public int team = 0;

	[SyncVar]
	public bool free = true;

	int count;

	public Transform SpawnPoint;
	public override void OnStartServer ()
	{
		free = true;
	}
	void Start () {
		GetComponent<Renderer> ().material.color = TeamManager.Instance.GetTeamColor (team);
	}
	
	// Update is called once per frame
		
	[Command]
	public void CmdBlock(){
		count++;
		free = false;
	}

	[Command]
	public void CmdUnBlock(){
		count--;
		if (count==0)
			free = true;
	}

}
