#pragma strict

var GenCre: GenericCreature;
var PlayerCheck: PlayerChecker;

var Opts : Options;
var isoPos: Vector3;
var isoRot: Quaternion;
var thirdPPos: Vector3;
var isIso: boolean = true;
var lockedCamera: boolean = false;
var SelectedUnit: Collider;
var SelectedUnitGC: GenericCreature;
var ShowingOpts: boolean = false;
var MenuBackground: Texture;

function Start () {
	transform.Rotate(45,0,0);
}

function Update () {
	if(isIso){
		var movementDirection = Vector3.zero;
		var MovementSpeed = 1;
		if(Input.mousePosition.x >= camera.pixelWidth-200 && !(Input.mousePosition.x > camera.pixelWidth)){
			movementDirection = Vector3.right * MovementSpeed;
			//transform.LookAt(PlayerCheck.transform);
		}else if(Input.mousePosition.x <= 200 && !(Input.mousePosition.x <= 0)){
			 movementDirection = Vector3.left * MovementSpeed;
			//transform.LookAt(PlayerCheck.transform);
		}
		if(Input.mousePosition.y >= camera.pixelHeight-100 && !(Input.mousePosition.y > camera.pixelHeight)){
			 movementDirection = (Vector3.forward/2)+(Vector3.up/2) * MovementSpeed;
			//transform.LookAt(PlayerCheck.transform);
		}else if(Input.mousePosition.y <= 100 && !(Input.mousePosition.y < 0)){
			movementDirection = (Vector3.back/2)+(Vector3.down/2) * MovementSpeed;
			//transform.LookAt(PlayerCheck.transform);
		}
		
		transform.localPosition += transform.localRotation * movementDirection;
		if(lockedCamera){
			transform.LookAt(PlayerCheck.transform);
		}
	}
	
	if(Input.GetMouseButtonDown(0)){
		ShootRay();
	}else if(Input.GetMouseButton(1)){
		CheckForAutoAttack();
	}
	if(Attacking && Opts.keepAttacking){
		Attack();
	}
	if(Input.GetButtonDown("Menu"))ShowingOpts = !ShowingOpts;
	
}

function ShootRay() {
    var ray = camera.ScreenPointToRay(Input.mousePosition);
    var hit : RaycastHit;
 	var hit2 : RaycastHit;
 	
    if(Physics.Raycast(ray, hit, Mathf.Infinity)) {
        //var moveScript : UnitMoveable = unit.GetComponent("UnitMoveable");
        /*if(hit.collider.name == "Player1"){
        	//if(Physics.Raycast(PlayerCheck.transform.position, hit.point - PlayerCheck.transform.position, hit2, Mathf.Infinity)){
        		PlayerCheck.autoattack(hit.collider);
        	//}
        }*/
        if(hit.collider.GetComponent(GenericCreature)){
        	SelectedUnit = hit.collider;
        	SelectedUnitGC = SelectedUnit.GetComponent(GenericCreature);
        	SelectedUnitGC.toggleOutline(true);
        }
        else /*if (hit.collider.tag == "Ground")*/ {
        	SelectedUnit =  null;
        	if(SelectedUnitGC)SelectedUnitGC.toggleOutline(false);
        	SelectedUnitGC = null;
        	if (PlayerCheck) {
           		PlayerCheck.Pathing.PathTo(hit.point);
           		//Debug.Log(hit.point);
        	}
        }
    }
}
var Attacking: boolean = false;
function CheckForAutoAttack(){
	var ray = camera.ScreenPointToRay(Input.mousePosition);
    var hit : RaycastHit;
    if(Physics.Raycast(ray, hit, Mathf.Infinity)) {
    	//Debug.Log(hit.collider.tag); Debug.Log(hit.collider.name);
    	//var tag = hit.collider.tag;
    	if(hit.collider.GetComponent(GenericCreature) && hit.collider.name != PlayerCheck.collider.name){
    		//Debug.LogWarning(hit.collider.name);
    		Attacking = true;
    		if(SelectedUnit != hit.collider){
    			SelectedUnit = hit.collider;
    			SelectedUnitGC = SelectedUnit.GetComponent(GenericCreature);
    			SelectedUnitGC.toggleOutline(true);
    		}
    		GenCre.BasicAttack(hit.collider);
    	}else{
    		Attacking = false;
    		SelectedUnit = null;
    		if(SelectedUnitGC)SelectedUnitGC.toggleOutline(false);
    		SelectedUnitGC = null;
    	}
    }
}

function Attack(){
	if(!SelectedUnit){
		Attacking = false;
		SelectedUnit = null;
		SelectedUnitGC = null;
	}else if(SelectedUnitGC.isDead){
		Attacking = false;
		SelectedUnit = null;
		SelectedUnitGC.toggleOutline(false);
		SelectedUnitGC = null;
	}else
	GenCre.BasicAttack(SelectedUnit);
}

function OnGUI(){
	if(GUI.Button(Rect(15,100,100,20), "Switch Camera")){
		if(isIso){
			isIso = false;
			
			isoPos = transform.position;
			//isoRot = transform.localRotation;
			
			Debug.Log(transform.position);
			//Debug.Log(transform.localRotation);
			
			transform.position = Vector3(PlayerCheck.transform.position.x,PlayerCheck.transform.position.y + 2, PlayerCheck.transform.position.z - 5);
			//transform.localRotation = Quaternion(transform.rotation.x, PlayerCheck.transform.rotation.y,PlayerCheck.transform.rotation.z,PlayerCheck.transform.rotation.w);
			transform.LookAt(PlayerCheck.transform);
			Debug.Log(transform.position);
			//Debug.Log(transform.localRotation);
			
			transform.parent = PlayerCheck.transform;
		}else{
			isIso = true;
			transform.parent = null;
			transform.position = isoPos;
			//transform.localRotation = isoRot;
						Debug.Log(transform.position);
			Debug.Log(transform.rotation);
		}
	}
	lockedCamera = GUI.Toggle(Rect(125,100,100,20), lockedCamera, "Locked camera?");
	var GM = GameObject.Find("GameManager");
	var Minion: GameObject;
	if(ShowingOpts){
		GUILayout.BeginArea(Rect(Screen.width/4, Screen.height-(Screen.height/5),Screen.width/2, Screen.height-(2*(Screen.height/5))), MenuBackground);
		GUILayout.BeginVertical();
		
		Opts.keepAttacking = GUILayout.Toggle(Opts.keepAttacking, "Keep attacking?");
		
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	if(GUI.Button(Rect(15,160, 100, 20), "Enemy Minion")){	
		Minion = Network.Instantiate(GM.GetComponent(GameManager).MinionObj, Vector3(15, 15, 15), Quaternion.identity, 1);
		GM.networkView.RPC("SpawnMinion", RPCMode.All, Network.AllocateViewID(), Minion.name, GenCre.Team == 1 ? 2 : 1);
	}
	if(GUI.Button(Rect(15,130, 100, 20), "Team Minion")){
		Minion = Network.Instantiate(GM.GetComponent(GameManager).MinionObj, Vector3(15, 15, 15), Quaternion.identity, 1);
		GM.networkView.RPC("SpawnMinion", RPCMode.All, Network.AllocateViewID(), Minion.name, GenCre.Team);
	}
}