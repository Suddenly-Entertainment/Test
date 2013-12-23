#pragma strict

var PlayerCheck: PlayerChecker;
var isoPos: Vector3;
var isoRot: Quaternion;
var thirdPPos: Vector3;
var isIso: boolean = true;

function Start () {
	transform.Rotate(45,0,0);
}

function Update () {
	if(isIso){
		if(Input.mousePosition.x >= camera.pixelWidth-200 && !(Input.mousePosition.x > camera.pixelWidth)){
			transform.Translate(Vector3(1,0,0));
			transform.LookAt(PlayerCheck.transform);
		}else if(Input.mousePosition.x <= 200 && !(Input.mousePosition.x <= 0)){
			transform.Translate(Vector3(-1,0,0));
			transform.LookAt(PlayerCheck.transform);
		}
		if(Input.mousePosition.y >= camera.pixelHeight-100 && !(Input.mousePosition.y > camera.pixelHeight)){
			transform.Translate(Vector3(0,1,0));
			transform.LookAt(PlayerCheck.transform);
		}else if(Input.mousePosition.y <= 100 && !(Input.mousePosition.y < 0)){
			transform.Translate(Vector3(0,-1,0));
			transform.LookAt(PlayerCheck.transform);
		}
	}
	if(Input.GetMouseButtonDown(0)){
		ShootRay();
	}else if(Input.GetMouseButton(1)){
		CheckForAutoAttack();
	}
	
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
        if (PlayerCheck) {
           PlayerCheck.MoveTo(hit.point);
           Debug.Log(hit.point);
        }
    }
}
function CheckForAutoAttack(){
	var ray = camera.ScreenPointToRay(Input.mousePosition);
    var hit : RaycastHit;
    if(Physics.Raycast(ray, hit, Mathf.Infinity)) {
    	Debug.Log(hit.collider.tag); Debug.Log(hit.collider.name);
    	if(hit.collider.tag == "Player" && hit.collider.name != PlayerCheck.collider.name){
    		Debug.Log(hit.collider.name);
    		PlayerCheck.autoattack(hit.collider);
    	}
    }
}

function OnGUI(){
	if(GUI.Button(Rect(15,100,30,20), "Switch Camera")){
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
}