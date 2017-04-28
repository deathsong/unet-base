using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Explosion : NetworkBehaviour {

	// Use this for initialization
	[SyncVar]
	public int team;

	Transform myTransform;

	[SerializeField]
	LayerMask layerMask;

	void Start () {
		myTransform = GetComponent<Transform> ();
		if (isServer)
			Explode ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Explode()
	{
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("Player");
		foreach (var item in gos) 
		{
			if (item.GetComponent<PlayerTeam> ().team != team || true) 
			{
				//Console.Instance.AddMessage ("testing explosion range " + item.transform.name);
				//float distance = Vector3.Distance (item.transform.position, myTransform.position);
				//Console.Instance.AddMessage ("distance :" + distance);
				//if (distance <= 5) 
				//{
					//Console.Instance.AddMessage (item.transform.name + " in range");
					//if (distance < 1.5) 
					//{
					//	Console.Instance.AddMessage (item.transform.name + " explosé de force");
					//	Motor m = item.transform.GetComponent<Motor> ();
					//	m.AppliqueExplosion (10f, myTransform.position, 5f);
					//} else 
					//{
						RaycastHit hit;
				Vector3 direction = (item.transform.position - myTransform.position);
				direction.Normalize ();
						//Debug.DrawRay (item.transform.position, direction * 10, Color.red, 20f);
						if ((Physics.Raycast (myTransform.position, direction, out hit, 5f, layerMask))) 
						{
							Console.Instance.AddMessage (hit.transform.name + " explosé");
					Console.Instance.AddMessage ("pourcent degat " + (100-(hit.distance/5f*100)).ToString());
							Motor m = hit.transform.GetComponent<Motor> ();
							m.AppliqueExplosion (5f, myTransform.position-direction , 5f);
							//Debug.DrawRay (myTransform.position, Vector3.up*5, Color.red, 5f);
							Health h = item.GetComponent<Health> ();
					int damage = (int)(25f*(1f - (hit.distance / 5f)));
					h.GetDamage (damage);
						}
					//}
				//}
			}
		}
	}
}
