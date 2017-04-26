using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using RichTextExtention;

public class Player : NetworkBehaviour {

	// Use this for initialization
	[SyncVar(hook="RenameHook")]
	public string pseudo = "";

	[SerializeField]
	Text namePlate;

	RichString style="";

	void Start () {
		style.bold = true;
		style.Color = "blue";

		if (isLocalPlayer) {
			pseudo = GameManager.Instance.Pseudo;

			Console.Instance.AddMessage ((name + " Player Start : "+pseudo).FromStyle(style));
			CmdRename (pseudo);
		}
	}
	public override void OnStartClient ()
	{
		if (isLocalPlayer)
			pseudo = GameManager.Instance.Pseudo;
		Console.Instance.AddMessage ((name + " Player OnStartClient : "+pseudo).FromStyle(style));
		//CmdRename (pseudo);
	}

	[Command]
	void CmdRename(string sname)
	{
		// synca var => hook
		Console.Instance.AddMessage ((name + " Player Serveur Rename: "+pseudo).FromStyle(style));
		pseudo = sname;
	}
	// Update is called once per frame
	void RenameHook(string sname)
	{
		pseudo = sname;
		Console.Instance.AddMessage ((name + " Player hookrename : "+sname).FromStyle(style));
		//if (!isLocalPlayer) {
			transform.name = sname;
			namePlate.text = sname;
		//}
	}
}
