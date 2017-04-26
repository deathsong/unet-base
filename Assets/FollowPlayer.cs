using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	// Use this for initialization
	public Transform follow;

	Transform myTransform;

	void Start () {
		myTransform = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (follow != null) {
			myTransform.position = follow.position;
			myTransform.rotation = follow.rotation;
		}
	}
}
