using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TeamManager : MonoBehaviour {

	// Use this for initialization
	public static TeamManager Instance;

	[SerializeField]
	Canvas canvas;

	[SerializeField]
	Team[] teams;

	void Awake () {
		Instance = this;
	}

	public void Update()
	{
		if (Cursor.lockState != CursorLockMode.Locked)
			return;
		if (Input.GetKeyDown (KeyCode.T)) {
			canvas.enabled = true;
			MouseManager.Instance.ShowCursor ();
		}
	}

	public string GetTeamName(int n)
	{
		return teams [n].name;
	}

	public Color GetTeamColor(int n)
	{
		return teams [n].color;
	}

	public void Show()
	{
		MouseManager.Instance.ShowCursor();
		canvas.enabled = true;
	}

	public void CloseTeamWindow()
	{
		canvas.enabled = false;
		MouseManager.Instance.HideCursor ();
	}

	public void ChangeTeam(int _team)
	{
		Console.Instance.AddMessage (PlayerTeam.Instance.name+ " change team");
		MouseManager.Instance.HideCursor ();
		canvas.enabled = false;
		if (PlayerTeam.Instance.team != _team) {
			PlayerTeam.Instance.ChangeTeam (_team);
		}
	}
}
