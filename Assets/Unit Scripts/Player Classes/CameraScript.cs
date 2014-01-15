using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			AttackTarget();
		}
	}

	void AttackTarget(){
		Ray mouseRay = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if(Physics.Raycast(mouseRay, out hitInfo)){
			if(hitInfo.collider.tag == "Player"){
				string player = hitInfo.collider.GetComponent<SuddenlyEntertainment.PlayerScriptClient>().OwnerClient;
				SuddenlyEntertainment.MainManager.PlayerDict[Network.player.guid].PlayerObj.networkView.RPC("BasicAttack", RPCMode.Server, Network.player.guid, player);
			}
		}
	}
}
