#pragma strict
var MinionObj: GameObject;

function Start () {

}

function Update () {

}

@RPC
function SpawnMinion(viewID: NetworkViewID, MinionName: String, Team: int){
	var Minion = GameObject.Find(MinionName);
	Minion.GetComponent(GenericCreature).NetworkViews[1].viewID = viewID;
	Minion.GetComponent(GenericCreature).Team = Team;
}

@RPC
function ClientRemovial(objName: String){
	Destroy(GameObject.Find(objName));
}