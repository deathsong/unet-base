using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {


	public Behaviour[] componentToDisable;

	void Start()
	{
		if (isServer)
			SpawnManager.Instance.Init ();
		
		Console.Instance.AddMessage ("[PlayerSetup] : Start()");

		if (!isLocalPlayer) {
			for (int i = 0; i < componentToDisable.Length; i++) {
				Console.Instance.AddMessage ("[PlayerSetup] : Disable component " + componentToDisable [i].name);
				componentToDisable [i].enabled = false;
			}
		} else 
		{
			NetworkHud.instance.Hide ();
			string sname = GameManager.Instance.Pseudo;
			//transform.name = sname;
			CmdRename (sname);
			Console.Instance.GetComponent<Canvas> ().enabled = true;
			GameManager.Instance.connected = true;
		}
	}



	[Command]
	void CmdRename(string sname)
	{
		transform.name = sname;
		RpcRename(sname);
	}
	[ClientRpc]
	void RpcRename(string sname)
	{
		transform.name = sname;
	}
}
