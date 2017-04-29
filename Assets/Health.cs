using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

	// Use this for initialization
	[SerializeField]
	Image healthBar;

	[SyncVar(hook="ChangeHealth")]
	public int health = 100;

	public bool dead = false;
	float deathTimer = 5f;

	Motor motor;

	void Start () {
		motor = GetComponent<Motor> ();
	}

	public override void OnStartLocalPlayer ()
	{
		health = 100;
		ChangeHealth (100);
	}

	// Update is called once per frame
	void Update () {
		if (dead) {
			deathTimer -= Time.deltaTime;
			if (deathTimer <= 0)
				Rez ();
		}
	}

	public void GetDamage(int damage)
	{
		if (!dead)
			CmdGetDamage (damage);
	}

	[Command]
	void CmdGetDamage(int damage)
	{
		health -= damage;
		if (health <= 0)
			Die ();
		health = Mathf.Clamp (health, 0, 100);
	}

	[Server]
	void Die()
	{
		dead = true;
		deathTimer = 5f;
		motor.Die ();
		RpcDie ();
	}

	[ClientRpc]
	void RpcDie()
	{
		dead = true;
		deathTimer = 5f;
		motor.Die ();
	}

	[Server]
	void Rez()
	{
		dead = false;
		health = 100;
		motor.Rez ();
		RpcRez ();
	}
	[ClientRpc]
	void RpcRez()
	{
		dead = false;
		health = 100;
		motor.Rez ();
	}

	void ChangeHealth(int _value)
	{
		health = _value;
		Console.Instance.AddMessage (name + " Hook change health");
		healthBar.fillAmount = (float)health/100f;
	}
}
