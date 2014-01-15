using UnityEngine;
using System.Collections;
namespace SuddenlyEntertainment{
public class ProjectileServer : MonoBehaviour {
	public string Target;
	public float Speed;

	// Use this for initialization
	void Start () {
		Speed = 1;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, SuddenlyEntertainment.MainManager.PlayerDict[Target].PlayerObj.transform.position, Speed);
	}

	void OnTriggerEnter(Collider collider){
		if(collider.tag == "Player"){
			PlayerScriptClient PSC = collider.GetComponent<PlayerScriptClient>();
				PSC.health -= 100;
			Network.Destroy(gameObject);
		}

	}
}
}
