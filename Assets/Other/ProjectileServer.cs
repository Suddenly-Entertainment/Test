using UnityEngine;
using System.Collections;

public class ProjectileServer : MonoBehaviour {
	public NetworkPlayer Target;
	public float Speed;

	// Use this for initialization
	void Start () {
		Speed = 1;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, SuddenlyEntertainment.MainManager.PlayerDict[Target.guid].PlayerObj.transform.position, Speed);
	}

	void OnTriggerEnter(){
		Network.Destroy(gameObject);
	}
}
