#pragma strict
@script RequireComponent(GenericCreature);
@script RequireComponent(SphereCollider);

var GenCre: GenericCreature;
var BasicAttackObj: GameObject;
var SpCo: SphereCollider;

function Start () {
	SpCo = GetComponent(SphereCollider);
	GenCre = GetComponent(GenericCreature);
	GenCre.DeathFunc = DeathFunction;
	GenCre.objHeight =  SpCo.radius*2;
	//GenCre.BasicAttackObj = BasicAttackObj;  //I have commented this out because I set a default on the prefab (Refrain from doing that, btw) until there is a projectile for minions.
}

function Update () {

}

function DeathFunction(){
	if(Network.isServer){
		Network.Destroy(networkView.viewID);
		//TODO: Add the rest :P
	}
}