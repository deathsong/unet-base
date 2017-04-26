using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using RichTextExtention;

public class PlayerTeam : NetworkBehaviour {

	// Use this for initialization

	[SyncVar(hook="ChangeTeamHook")]
	public int team = 0;

	Motor motor;

	[SerializeField]
	Renderer renderer;

	public static PlayerTeam Instance;

	RichString style="";

	void Awake()
	{
		//renderer = GetComponent<Renderer> ();
	}

	void Start()
	{
		style.bold = true;
		style.Color = "red";
		if (isLocalPlayer) 
		{
			Console.Instance.AddMessage ((name+ " PlayerTeam start").FromStyle(style));
			Instance = this;
			motor = GetComponent<Motor> ();

			if (team == 0)
				TeamManager.Instance.Show ();
		}
	}

	public override void OnStartClient ()
	{
		Console.Instance.AddMessage ((name+ " PlayerTeam on start client").FromStyle(style));
		ChangeTeamHook (team);
	}

	void Update()
	{
		
	}

	public void ChangeTeam(int _team)
	{
		// local player change team 
		//if (!isLocalPlayer)
		//	return;
		Console.Instance.AddMessage ((name + " local PlayerTeam ChangeTeam : "+_team).FromStyle(style));
		//team = _team;
		//renderer.material.color = TeamManager.Instance.GetTeamColor (_team);
		CmdChangeTeam (_team);
		if (isLocalPlayer)
			motor.Spawn (_team);
	}
		
	[Command]
	public void CmdChangeTeam(int _team)
	{
		Console.Instance.AddMessage ((name + " server PlayerTeam ChangeTeam : "+_team).FromStyle(style));
		team = _team;
		//motor.Spawn (_team);
	}

	[ClientRpc]
	public void RpcChangeTeam(int _team)
	{
		Console.Instance.AddMessage (name+"rpc change team(client)");
		//renderer.material.color = TeamManager.Instance.GetTeamColor (team);

	}

	public void ChangeTeamHook(int value)
	{
		team = value;
		Console.Instance.AddMessage ((name + " PlayerTeam ChangeTeamHook : "+value).FromStyle(style));
		renderer.material.color = TeamManager.Instance.GetTeamColor (value);
	}

}
