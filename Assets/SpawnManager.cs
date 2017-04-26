using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SpawnManager : MonoBehaviour {

	// Use this for initialization
	List<Spawner> Spawners;
	public static SpawnManager Instance;

	void Awake()
	{
		Instance = this;

	}
	public void Init()
	{
		Spawners = new List<Spawner>();
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("SpawnPoint");
		foreach (var item in gos) {
			Spawners.Add (item.GetComponent<Spawner> ());
		}
		Console.Instance.AddMessage ("spawn manager " + Spawners.Count + "spawn found");
	}
	void Start () {
		
	}

	public Vector3 GetSpawnPoint(int team)
	{
		if (Spawners == null)
			Init ();
		Console.Instance.AddMessage ("looking spawnpoint team " + team);
		List<Spawner> temp = new List<Spawner> ();
		foreach (var item in Spawners) {
			if (item.team == team && item.free)
				temp.Add (item);
		}
		Spawner S = temp [Random.Range (0, temp.Count)];
		Console.Instance.AddMessage ("=> " + S.name);
		return temp [Random.Range (0, temp.Count)].SpawnPoint.position;
	}

}
