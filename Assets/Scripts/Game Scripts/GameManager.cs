using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public GameObject MinionObj;
	//Dictionary clientsFinished;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[RPC]
	public void SpawnMinion(string MinionName, int Team){
		GameObject Minion = GameObject.Find(MinionName);
		//Minion.networkView.viewID = viewID;
		(Minion.GetComponent(typeof(UnitBase)) as UnitBase).Team = Team;
	}
	
	[RPC]
	public void ClientRemovial(string objName){
		Destroy(GameObject.Find(objName));
		
		networkView.RPC("ClientFinished", RPCMode.Server, objName);
	}
	//clientsFinished = 0;
	/*[RPC]
	public void ClientFinished(string objName){
		if(!clientsFinished){
			clientsFinished.Add(objName, 1);
		}else{
			clientsFinished[objName] = parseInt(clientsFinished[objName].ToString()) + 1;
		}
		if(parseInt(clientsFinished[objName].ToString()) >= Network.connections.Length){
			//Network.RemoveRPCs(viewID);
			Destroy(this.gameObject);
			clientsFinished[objName] = 0;
		}
	}*/

}
