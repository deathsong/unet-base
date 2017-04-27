using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

	// Use this for initialization
	[SerializeField]
	Image healthBar;

	[SyncVar(hook="ChangeHealth")]
	int health = 100;

	void Start () {
	
	}

	public override void OnStartLocalPlayer ()
	{
		health = 100;
		ChangeHealth (100);
	}

	// Update is called once per frame

	void ChangeHealth(int _value)
	{
		health = _value;
		Console.Instance.AddMessage (name + " Hook change health");
		health = _value;
		healthBar.fillAmount = (int)(100f / health);
	}
}
