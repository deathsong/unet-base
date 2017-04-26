using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Login : MonoBehaviour {

	// Use this for initialization
	[SerializeField]
	InputField pseudo;
	[SerializeField]
	InputField password;
	[SerializeField]
	Text message;

	Canvas canvas;

	void Start () {
		canvas = GetComponent<Canvas> ();
		canvas.enabled = true;
	}
	

	public void LogIn()
	{
		Debug.Log("click");
		string _pseudo = pseudo.text;
		string _pass = password.text;
		WWWForm form = new WWWForm();
		form.AddField("pseudo", _pseudo);
		form.AddField("password", _pass);
		form.AddField("command", "login");
		//WWW download = new WWW(url, form);
		message.text = "Log IN...";
		StartCoroutine(Download("http://localhost:8080/Game/LogIn.php", form));
	}

	IEnumerator Download(string url,WWWForm form)
	{
		message.text = "sending";
		WWW download = new WWW(url, form);
		yield return download;
		Debug.Log("ok");
		if (!string.IsNullOrEmpty(download.text))
		{
			message.text = download.text;
			Debug.Log (message.text);
		}else
		{
			message.text = "erreur de connection...";
		}
		if (message.text == "OK")
		{
			GameManager.Instance.Pseudo = pseudo.text;
			canvas.enabled = false;
			NetworkHud.instance.Show ();

		}
	}


	public void Register()
	{
		if (string.IsNullOrEmpty(pseudo.text) || string.IsNullOrEmpty(password.text))
		{
			message.text = "entrez pseudo et password";
			return;
		}
		Debug.Log("click");
		string _pseudo = pseudo.text;
		string _pass = password.text;
		WWWForm form = new WWWForm();
		form.AddField("pseudo", _pseudo);
		form.AddField("password", _pass);
		form.AddField("command", "register");
		//WWW download = new WWW(url, form);
		message.text = "Register...";
		StartCoroutine(Download("http://localhost:8080/Game/LogIn.php", form));
	}
}
