using UnityEngine;
using System.Collections;

public class RespawnScript : MonoBehaviour {
	public bool isTriggered = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(){
		isTriggered = true;

	}
	void OnTriggerStay(){
		isTriggered = true;
	}
	void OnTriggerExit(){
		isTriggered = false;
	}
}
