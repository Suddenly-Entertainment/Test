#pragma strict

var speed : int = 100;
var damage : float = 50;
var target : Collider;
var firedBy : String;

function Start () {
	Debug.DrawLine(transform.position, target.transform.position, Color.red, 10, false);
	Debug.DrawRay(transform.position, target.transform.position - transform.position, Color.green, 10, false);
}

function Update () {
	transform.LookAt(target.transform);
	// The step size is equal to speed times frame time.
	var step = speed * Time.deltaTime;
	
	// Move our position a step closer to the target.
	transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
}

function OnCollisionEnter(info: Collision){
	Debug.Log("Colided");
	if(info.gameObject.name == target.name){
		Network.Destroy(this.networkView.viewID); //We need this so it is properly destroyed after harming them on all clients.
		info.gameObject.networkView.RPC("takeDamage", RPCMode.AllBuffered, damage); // /Sends the message to all clients that this player is taking damage!
	}
}
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
