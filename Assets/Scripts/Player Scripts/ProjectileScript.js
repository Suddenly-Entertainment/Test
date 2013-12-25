#pragma strict

var speed : int = 10;
var damage : float = 50;
var target : Collider = null;
var firedBy : String;
var doneSet : boolean = false;
var viewID : NetworkViewID;
var firedOnce: boolean = false;


function Start () {
	if(!doneSet){
		gameObject.SetActive(false);
	}
/*	
	Debug.DrawLine(transform.position, target.transform.position, Color.red, 10, false);
	Debug.DrawRay(transform.position, target.transform.position - transform.position, Color.green, 10, false);
*/
}

function Update () {
	if(doneSet){
		gameObject.SetActive(true);
	}
	if(target != null){
		transform.LookAt(target.transform);
		// The step size is equal to speed times frame time.
		var step = speed * Time.deltaTime;
	
		// Move our position a step closer to the target.
		transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
	}
}

function OnCollisionEnter(info: Collision){
	Debug.Log("Colided");
	//if(Network.isServer){
		if(target != null){
			if(info.gameObject.name == target.name){
				if(Network.isClient){
					networkView.RPC("ServerRemovial", RPCMode.Server);
				}else if(Network.isServer){
					ServerRemovial();
				}
			}
		}
	//}
}
@RPC
function ServerRemovial(){
	if(!firedOnce){
		firedOnce = true;
		target.networkView.RPC("takeDamage", target.GetComponent(PlayerChecker).NetPlayer, damage);
		Network.RemoveRPCs(viewID);
		GameObject.Find("GameManager").networkView.RPC("ClientRemovial", RPCMode.OthersBuffered, gameObject.name);
		Destroy(this.gameObject);
		
		Debug.Log("I really hope this works");
		//Network.Destroy(viewID); //We need this so it is properly destroyed after harming them on all clients.
	}
}

/*@RPC
function ClientRemovial(){
	networkView.RPC("ClientFinished", RPCMode.Server);
	Destroy(this.gameObject);
}*/

var clientsFinished = 0;
/*@RPC
function ClientFinished(){
	clientsFinished++;
	if(clientsFinished >= Network.connections.Length){
		//Network.RemoveRPCs(viewID);
		Destroy(this.gameObject);
	}
}*/
/*function OnTriggerEnter(info: Collider){
	Debug.Log("Colided");
	if(info.gameObject.name == target.name){
		Destroy(this.gameObject);
		info.gameObject.networkView.RPC("takeDamage", RPCMode.AllBuffered, damage); //.currentHealth -= damage;
	}
}
function OnTriggerStay(info: Collider){
	Debug.Log("Stayed");
}*/
