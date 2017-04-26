using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MouseManager : MonoBehaviour {

	// Use this for initialization
	public bool showCursor = true;

	public static MouseManager Instance;

	[SerializeField]
	Canvas canvas;

	void Start () {
		Instance = this;
		ShowCursor ();
	}
	
	// Update is called once per frame
	public void ShowCursor()
	{
		if (!showCursor)
			Console.Instance.AddMessage ("show cursor");
		showCursor = true;
		Cursor.lockState = CursorLockMode.None;
		if (GameManager.Instance.connected)
			canvas.enabled = true;

	}

	public void HideCursor()
	{
		if (showCursor)
			Console.Instance.AddMessage ("hide cursor");
		showCursor = false;
		Cursor.lockState = CursorLockMode.Locked;
		if (GameManager.Instance.connected)
			canvas.enabled = false;	
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			showCursor = !showCursor;
		}
		if (showCursor) {
			ShowCursor ();
			//Cursor.visible = false;
		} 
		else 
		{
			HideCursor ();
			//Cursor.visible = true;
		}
	}
}
