 #pragma strict
@script RequireComponent(GenericCreature)



var PlayerCamera : GameObject;
var Arrow: GameObject;
var GenCre: GenericCreature;
var Pathing: SimplePathing;
var controller: CharacterController;
//var speed: float;
var rotateSpeed : float = 3.0;
var curPos: Vector3;
//var nextAtk: float = 0;
//var targetPosition: Vector3;

//var path: Pathfinding.Path;

//var nextWaypointDistance: float = 3;
//var currentWaypoint: int = 0;
//var seeker: Seeker;
//var endReached: boolean = false;

/*var currentHealth: double = 1000;
var maxHealth: double = 1000;
var healthRegen: float = 10; //Per Second
var AtkSpeed: float = 2.5;

var isMine: boolean;
var isDead: boolean;

var Team: int = 0;*/
var Player: int = 0;

var CapCo: CapsuleCollider;
var Mat : Material;

function Awake(){
	GenCre = GetComponent(GenericCreature);
}
function Start () {
	CapCo = this.GetComponent(CapsuleCollider);
	Pathing = GetComponent(SimplePathing);
	//GenCre = GetComponent(GenericCreature);
	GenCre.DeathFunc = DeathImplement;
	GenCre.BasicAttackObj = Arrow;
	GenCre.objHeight = CapCo.height;
	this.tag = "Player";
	
	if(GenCre.isMine){ //These are the things that we do only if this is your player object
		Debug.Log("This Object Belongs to you and you can control it");
		
		Debug.Log(this.GetComponent(CharacterMotor).canControl);
		this.GetComponent(CharacterMotor).SetControllable(true);
		Debug.Log(this.GetComponent(CharacterMotor).canControl);
		
		var PCam = Instantiate(PlayerCamera, Vector3(this.transform.position.x,this.transform.position.y + 100,this.transform.position.z), this.transform.rotation);
		PCam.name = "PlayerCamera";//+networkView.viewID;
		PCam.tag = "MainCamera"; //For the GUIs to be properly rendered above the head
		//var testThing = GameObject.Find(PCam.name);
		//testThing.GetComponent(CameraControls).PlayerCheck = this;
		var CamControls = PCam.GetComponent(CameraControls);
		CamControls.PlayerCheck = this;
		CamControls.GenCre = GenCre;
		PlayerCamera = PCam;
		//testThing.transform.parent = this.transform;
	}else{
		PlayerCamera = Camera.main.gameObject;
	}
	//var viewId = networkView.viewID.ToString().Substring(12); 
	name = "Player " + Player;
	controller = GetComponent(CharacterController);
//	seeker = GetComponent(Seeker);
//	seeker.StartPath(transform.position, targetPosition, OnPathComplete);

}
/*function OnPathComplete(p : Pathfinding.Path){
	Debug.Log("Yay, we got a path back. Did it have an error? " +p.error);
	        if (!p.error) {
            path = p;
            //Reset the waypoint counter
            currentWaypoint = 0;
           	endReached = false;
        }
}*/


function Update () {
	if(GenCre.isMine){
		if(!GenCre.isDead){
	        //var controller : CharacterController = GetComponent(CharacterController);
        	// Rotate around y - axis
        	transform.Rotate(0, Input.GetAxis ("Horizontal") * rotateSpeed, 0);
        	
        	curPos = transform.position;
        	// Move forward / backward
        	var forward : Vector3 = transform.TransformDirection(Vector3.forward);
        	var curSpeed : float = (Pathing.speed * Input.GetAxis ("Vertical"))* Time.fixedDeltaTime;
        	controller.SimpleMove(forward * curSpeed);
        	/*if(currentHealth < maxHealth){
        		currentHealth += healthRegen * Time.fixedDeltaTime;
        	}else if(currentHealth > maxHealth){
        		currentHealth = maxHealth;
        	}*/
       	 
        	networkView.RPC("gainHealth", RPCMode.All, GenCre.healthRegen * Time.fixedDeltaTime);
    	}
    }
}

function DeathImplement(){
	CapCo.enabled = false; //So that people and objects can go through him
	yield WaitForSeconds(10); //Test death timer
	transform.position = Vector3(1,1,1);
	CapCo.enabled = true;
}

/*function FixedUpdate () {
if(isMine){
        if (path == null) {
            //We have no path to move after yet
            return;
        }
        

        if (currentWaypoint >= path.vectorPath.Count) {
          	if(endReached == false){
            	Debug.Log ("End Of Path Reached");
            	endReached = true;
            }
            return;
        }
        
        //Direction to the next waypoint
        var  dir: Vector3 = (path.vectorPath[currentWaypoint]-transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;
        controller.SimpleMove (dir);
        
        //Check if we are close enough to the next waypoint
        //If we are, proceed to follow the next waypoint
        if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
            currentWaypoint++;
            return;
        }
    }
}*/
    
/*function MoveTo(target: Vector3){
	if(isMine){
		seeker.StartPath(transform.position, target, OnPathComplete);
	}
}*/

/*function OnSerializeNetworkView(stream : BitStream, info : NetworkMessageInfo){
	var health: float = 0;
	var pos: Vector3 = Vector3(0,0,0);
	if(stream.isWriting){
		health = currentHealth;
		transform.position = curPos;
		stream.Serialize(health);
		stream.Serialize(transform.position);
	}else{
		stream.Serialize(health);
		stream.Serialize(transform.position);
		currentHealth = health;
		curPos = transform.position;
	}
}*/

//function autoattack(target : Collider){
//	var hit2: RaycastHit;
//	if(Time.time > nextAtk){
//		var thing = Network.Instantiate(Arrow, Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, 0);
//		var viewID = Network.AllocateViewID();
//		/*var uniqueNum = Random.value *100;
//		var thingName = "Arrow" + uniqueNum;
//		thing.name = thingName;
//		var AutoAttack = GameObject.Find(thingName).GetComponent(ProjectileScript);
//		AutoAttack.target = target;
//		AutoAttack.firedBy = this.name;
//		Debug.Log("Arrow Fired: "+uniqueNum);*/
//		this.networkView.RPC("createAtk", RPCMode.All, thing.name, target.name, viewID);
//		
//		nextAtk = Time.time + AtkSpeed;
//	}
//}

/*function OnCollisionEnter(info: Collision){
	Debug.Log("Colided");
	//if(info.gameObject.name != firedBy){
		//Destroy(this);
		//info.gameObject.networkView.RPC("takeDamage", RPCMode.AllBuffered, damage); //.currentHealth -= damage;
	//}
}
function OnTriggerEnter(info: Collider){
	//Debug.Log("Triggerd");
	if(info.name.Substring(0,5) == "Arrow"){
		Debug.Log("Arrow Triggered!");
	}
	//if(info.gameObject.name != firedBy){
		//Destroy(this);
		//info.gameObject.networkView.RPC("takeDamage", RPCMode.AllBuffered, damage); //.currentHealth -= damage;
	//}
}*/

//private var hpBarSize: int = 100;
//var hpBarImgPlayer: Texture2D;
//var hpBarImgEnemy: Texture2D;
//var hpBarImgAlly: Texture2D;
//
//var HPBarPos: Vector3;
//var HPBarPosW: Vector3;
////var HPBarPosS: Vector3;
//var InterruptHPB: boolean = false;


/*function OnGUI(){
	//if(isMine){
		//var hpBarStyle = new GUIStyle();
		//var offSets: Vector3 = PlayerCamera.camera.ScreenToWorldPoint(Vector3(-(hpBarSize/2), -40, 0));
		var MainPC = PlayerCamera.GetComponent(CameraControls).PlayerCheck;
		HPBarPosW = transform.position - Vector3(0, -(CapCo.height/2) -0.5, 0);
		var screenPos : Vector3 = Camera.main.WorldToScreenPoint(HPBarPosW);
		//HPBarPosS = screenPos;
		//Debug.Log(screenPos);
		var percHP = currentHealth / maxHealth;
		var curHPBarSize = parseInt(100*percHP);
		//Debug.Log(curHPBarSize);
		//GUI.Box(Rect(Screen.width/2-(hpBarSize/2),Screen.height-20,hpBarSize,20), "");
		//GUI.Box(Rect(Screen.width/2-(hpBarSize/2),Screen.height-20,curHPBarSize,20), hpBarImg);
		//GUI.Label(Rect(Screen.width/2-25,Screen.height-20, 50, 20), currentHealth+"/"+maxHealth);
		if(!InterruptHPB){
			HPBarPos.x = screenPos.x -(hpBarSize/2);
			HPBarPos.y = Screen.height-screenPos.y;//-40;
			HPBarPos.z = screenPos.z;
		}
		if(HPBarPos.z >= 0){
			GUI.Box(Rect(HPBarPos.x ,HPBarPos.y,hpBarSize,20), "");
			GUI.Box(Rect(HPBarPos.x,HPBarPos.y,curHPBarSize,20), isMine ? hpBarImgPlayer : MainPC.Team == Team ? hpBarImgAlly : hpBarImgEnemy);
			GUI.Label(Rect(screenPos.x-(hpBarSize/4),Screen.height-screenPos.y, 100, 20), currentHealth+"/"+maxHealth);
		}
	//}
}*/

//@RPC
//function gainHealth(hp: float){
//	if(currentHealth + hp <= maxHealth){
//		currentHealth += hp;
//	}else if(currentHealth + hp > maxHealth){
//		currentHealth += (currentHealth + hp) - maxHealth;
//	}
//	if(currentHealth > maxHealth){
//		currentHealth = maxHealth;
//	}
//}
//
//@RPC
//function takeDamage(hp: float){
//	currentHealth -= hp;
//	if(currentHealth <= 0){
//		networkView.RPC("Die", RPCMode.All);
//	}
//}
//
//@RPC
//function createAtk(thingName: String, targetName: String, viewID: NetworkViewID){
//		//Debug.Log(thing); Debug.Log(target);
//		var thing = GameObject.Find(thingName);
//		var AutoAttack = thing.GetComponent(ProjectileScript);
//		AutoAttack.target = GameObject.Find(targetName).collider;
//		
//		var uniqueNum = ""+viewID;
//		//var thingName = "Arrow" + uniqueNum;
//		//.GetComponent(ProjectileScript);
//		thing.name = "Arrow" + uniqueNum;
//		
//		var nView = thing.GetComponent(NetworkView);
//		nView.viewID = viewID;
//		
//
//		
//		AutoAttack.firedBy = this.name;
//		AutoAttack.doneSet = true;
//		Debug.Log("Arrow Fired: "+uniqueNum);
//
//}
//@RPC
//function Die(){
//	isDead = true; //So that he can't move
//	CapCo.enabled = false; //So that people and objects can go through him
//	yield WaitForSeconds(10); //Test death timer
//	transform.position = Vector3(1,1,1);
//	CapCo.enabled = true;
//}

@RPC
function SetupPlayerColor(R: float, G: float, B:float){
	renderer.material = new Material(Mat);
	renderer.material.color = new Color(R, G, B);
}
@RPC
function SetupPlayer(PlayerName: String){
	GenCre.Name = PlayerName;
}

