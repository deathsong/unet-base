using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;


public class NetworkHud : MonoBehaviour
{

    // Use this for initialization
	public static NetworkHud instance;

    [SerializeField]
    private InputField inputIp;

	[SerializeField]
	Canvas canvas;

	void Awake()
	{
		instance = this;
	}
		
	public void Show ()
	{
		canvas.gameObject.SetActive(true);
		MouseManager.Instance.ShowCursor ();
	}
	public void Hide ()
	{
		canvas.gameObject.SetActive(false);
		MouseManager.Instance.HideCursor ();
	}

    public void Host()
    {
        //GameManager.Instance.networkMenu.SetActive(false);
		//canvas.enabled=false;
        NetworkManager.singleton.StartHost();
    }
    public void Join()
    {
        string ip = inputIp.text;
        if (string.IsNullOrEmpty(ip))
        {
            ip = "localhost";
        }
        //GameManager.Instance.networkMenu.SetActive(false);
		//canvas.enabled=false;
        NetworkManager.singleton.networkAddress = ip;
        NetworkManager.singleton.StartClient();
    }
    public void Stop()
    {
        //GameManager.Instance.GameMenu.SetActive(false);
        //GameManager.Instance.networkMenu.SetActive(true);
        Debug.Log("stop");
        NetworkManager.singleton.StopHost();
        NetworkManager.singleton.StopClient();
		canvas.enabled = true;
		GameManager.Instance.connected = false;
		GameObject.Find ("GameCanvas").GetComponent<Canvas> ().enabled = false;
    }
    
}
