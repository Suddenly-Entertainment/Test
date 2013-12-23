 #pragma strict

var PlayerCamera : GameObject;
var Arrow: GameObject;
var targetPosition: Vector3;
var controller: CharacterController;
var path: Pathfinding.Path;
var speed: float = 100000;
var nextWaypointDistance: float = 3;
var currentWaypoint: int = 0;
var seeker: Seeker;
var endReached: boolean;

var currentHealth: double = 100;
var maxHealth: double = 100;
var healthRegen: float = 2.5;
var AtkSpeed: float = 1;
var curPos: Vector3;
var isMine: boolean;

function Awake(){

}
function Start () {
	isMine = networkView.isMine;
	if(isMine){
		Debug.Log("This Object Belongs to you and you can control it");
		
		Debug.Log(this.GetComponent(CharacterMotor).canControl);
		this.GetComponent(CharacterMotor).SetControllable(true);
		Debug.Log(this.GetComponent(CharacterMotor).canControl);
		
		var PCam = Instantiate(PlayerCamera, Vector3(this.transform.position.x,this.transform.position.y + 100,this.transform.position.z), this.transform.rotation);
		PCam.name = "PlayerCamera"+networkView.viewID;
		var testThing = GameObject.Find(PCam.name);
		testThing.GetComponent(CameraControls).PlayerCheck = this;
		PlayerCamera = testThing;
		//testThing.transform.parent = this.transform;
	}
	var viewId = networkView.viewID.ToString().Substring(12); 
	name = "Player" + viewId;
	controller = GetComponent(CharacterController);
	seeker = GetComponent(Seeker);
	seeker.StartPath(transform.position, targetPosition, OnPathComplete);

}
function OnPathComplete(p : Pathfinding.Path){
	Debug.Log("Yay, we got a path back. Did it have an error? " +p.error);
	        if (!p.error) {
            path = p;
            //Reset the waypoint counter
            currentWaypoint = 0;
           	endReached = false;
        }
}
    var rotateSpeed : float = 3.0;

function Update () {
	if(isMine){
	        var controller : CharacterController = GetComponent(CharacterController);
        // Rotate around y - axis
        transform.Rotate(0, Input.GetAxis ("Horizontal") * rotateSpeed, 0);
        
        curPos = transform.position;
        // Move forward / backward
        var forward : Vector3 = transform.TransformDirection(Vector3.forward);
        var curSpeed : float = (speed * Input.GetAxis ("Vertical"))* Time.fixedDeltaTime;
        controller.SimpleMove(forward * curSpeed);
        /*if(currentHealth < maxHealth){
        	currentHealth += healthRegen * Time.fixedDeltaTime;
        }else if(currentHealth > maxHealth){
        	currentHealth = maxHealth;
        }*/
        
        //networkView.RPC("gainHealth", RPCMode.AllBuffered, healthRegen * Time.fixedDeltaTime);
    }
}

function FixedUpdate () {
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
    }
    
function MoveTo(target: Vector3){
	if(isMine){
		seeker.StartPath(transform.position, target, OnPathComplete);
	}
}

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
var nextAtk: float = 0;
function autoattack(target : Collider){
	var hit2: RaycastHit;
	if(Time.time > nextAtk){
		var thing = Network.Instantiate(Arrow, Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, 0);
		
		/*var uniqueNum = Random.value *100;
		var thingName = "Arrow" + uniqueNum;
		thing.name = thingName;
		var AutoAttack = GameObject.Find(thingName).GetComponent(ProjectileScript);
		AutoAttack.target = target;
		AutoAttack.firedBy = this.name;
		Debug.Log("Arrow Fired: "+uniqueNum);*/
		this.networkView.RPC("createAtk", RPCMode.AllBuffered, thing.name, target.name);
		
		nextAtk = Time.time + AtkSpeed;
	}
}

function OnCollisionEnter(info: Collision){
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
}

private var hpBarSize: int = 100;
var hpBarImg: Texture2D;

function OnGUI(){
	if(isMine){
		//var hpBarStyle = new GUIStyle();
		var percHP = currentHealth / maxHealth;
		var curHPBarSize = parseInt(100*percHP);
		//Debug.Log(curHPBarSize);
		GUI.Box(Rect(Screen.width/2-(hpBarSize/2),Screen.height-20,hpBarSize,20), "");
		GUI.Box(Rect(Screen.width/2-(hpBarSize/2),Screen.height-20,curHPBarSize,20), hpBarImg);
		GUI.Label(Rect(Screen.width/2-25,Screen.height-20, 50, 20), currentHealth+"/"+maxHealth);
	}
}

@RPC
function gainHealth(hp: float){
	if(currentHealth + hp <= maxHealth){
		currentHealth += hp;
	}else if(currentHealth + hp > maxHealth){
		currentHealth += (currentHealth + hp) - maxHealth;
	}
	if(currentHealth > maxHealth){
		currentHealth = maxHealth;
	}
}

@RPC
function takeDamage(hp: float){
	currentHealth -= hp;
}

@RPC
function createAtk(thingName: String, targetName: String){
		//Debug.Log(thing); Debug.Log(target);
		var uniqueNum = Random.value *100;
		//var thingName = "Arrow" + uniqueNum;
		var thing = GameObject.Find(thingName);//.GetComponent(ProjectileScript);
		thing.name = "Arrow" + uniqueNum;
		var AutoAttack = thing.GetComponent(ProjectileScript);
		AutoAttack.target = GameObject.Find(targetName).collider;
		AutoAttack.firedBy = this.name;
		Debug.Log("Arrow Fired: "+uniqueNum);

}

