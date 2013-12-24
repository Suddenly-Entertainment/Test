#pragma strict
var MinionObj: GameObject;

function Start () {

}

function Update () {

}

@RPC
function SpawnMinion(viewID: NetworkViewID, MinionName: String, Team: int){
	var Minion = GameObject.Find(MinionName);
	Minion.networkView.viewID = viewID;
	Minion.GetComponent(GenericCreature).Team = Team;
}