using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {

	//public UnitBase UnitB;
	public Player PlayerScript;

	public Options Opts;
	public Vector3 isoPos;
	public Quaternion isoRot;
	public Vector3 thirdPPos;
	public bool isIso;
	public bool lockedCamera;
	public Collider SelectedUnit;
	public UnitBase SelectedUnitUB;
	public bool ShowingOpts;
	public Texture MenuBackground;

	//A function to initialize values;
	public void initVarsToDefaultValue(){
		isIso = true;
		lockedCamera = false;

		ShowingOpts = false;
		Attacking = false;
	}

	//Called before Start
	public void Awake(){
		initVarsToDefaultValue();

	}

	// Use this for initialization
	public void Start () {
		transform.Rotate(45,0,0);
	}

	// Update is called once per frame
	public void Update () {
		if(isIso){
			Vector3 movementDirection = Vector3.zero;
			float MovementSpeed = 1;
			if(Input.mousePosition.x >= camera.pixelWidth-200 && !(Input.mousePosition.x > camera.pixelWidth)){
				movementDirection = Vector3.right * MovementSpeed;
				//transform.LookAt(PlayerScript.transform);
			}else if(Input.mousePosition.x <= 200 && !(Input.mousePosition.x <= 0)){
				 movementDirection = Vector3.left * MovementSpeed;
				//transform.LookAt(PlayerScript.transform);
			}
			if(Input.mousePosition.y >= camera.pixelHeight-100 && !(Input.mousePosition.y > camera.pixelHeight)){
				 movementDirection = ( Vector3.forward/2)+( Vector3.up/2) * MovementSpeed;
				//transform.LookAt(PlayerScript.transform);
			}else if(Input.mousePosition.y <= 100 && !(Input.mousePosition.y < 0)){
				movementDirection = ( Vector3.back/2)+(Vector3.down/2) * MovementSpeed;
				//transform.LookAt(PlayerScript.transform);
			}
			
			transform.localPosition += transform.localRotation * movementDirection;
			if(lockedCamera){
				transform.LookAt(PlayerScript.transform);
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
	
	public void ShootRay() {
	    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		RaycastHit hit2;
	 	
	    if(Physics.Raycast(ray, out hit, Mathf.Infinity)) {
	        // moveScript  UnitMoveable = unit.GetComponent("UnitMoveable");
	        /*if(hit.collider.name == "Player1"){
	        	//if(Physics.Raycast(PlayerScript.transform.position, hit.point - PlayerScript.transform.position, hit2, Mathf.Infinity)){
	        		PlayerScript.autoattack(hit.collider);
	        	//}
	        }*/
	        if(hit.collider.GetComponent(typeof(UnitBase)) != null){
	        	SelectedUnit = hit.collider;
	        	SelectedUnitUB = (SelectedUnit.GetComponent(typeof(UnitBase)) as UnitBase);
	        	SelectedUnitUB.toggleOutline(true);
	        }
	        else /*if (hit.collider.tag == "Ground")*/ {
	        	SelectedUnit =  null;
	        	if(SelectedUnitUB)SelectedUnitUB.toggleOutline(false);
	        	SelectedUnitUB = null;
	        	if (PlayerScript) {
	           		PlayerScript.Pather.PathTo(hit.point);
	           		//Debug.Log(hit.point);
	        	}
	        }
	    }
	}

	bool Attacking;
	public void CheckForAutoAttack(){
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
	    if(Physics.Raycast(ray, out hit, Mathf.Infinity)) {
	    	//Debug.Log(hit.collider.tag); Debug.Log(hit.collider.name);
	    	// tag = hit.collider.tag;
	    	if(hit.collider.GetComponent(typeof(UnitBase)) != null && hit.collider.name != PlayerScript.collider.name){
	    		//Debug.LogWarning(hit.collider.name);
	    		Attacking = true;
	    		if(SelectedUnit != hit.collider){
	    			SelectedUnit = hit.collider;
	    			SelectedUnitUB = (SelectedUnit.GetComponent(typeof(UnitBase)) as UnitBase);
	    			SelectedUnitUB.toggleOutline(true);
	    		}
	    		PlayerScript.BasicAttack(hit.collider);
	    	}else{
	    		Attacking = false;
	    		SelectedUnit = null;
	    		if(SelectedUnitUB)SelectedUnitUB.toggleOutline(false);
	    		SelectedUnitUB = null;
	    	}
	    }
	}
	
	public void Attack(){
		if(!SelectedUnit){
			Attacking = false;
			SelectedUnit = null;
			SelectedUnitUB = null;
		}else if(SelectedUnitUB.isDead){
			Attacking = false;
			SelectedUnit = null;
			SelectedUnitUB.toggleOutline(false);
			SelectedUnitUB = null;
		}else
		PlayerScript.BasicAttack(SelectedUnit);
	}
	
	public void OnGUI(){
		if(GUI.Button(new Rect(15,100,100,20), "Switch Camera")){
			if(isIso){
				isIso = false;
				
				isoPos = transform.position;
				//isoRot = transform.localRotation;
				
				Debug.Log(transform.position);
				//Debug.Log(transform.localRotation);
				
				transform.position = new Vector3(PlayerScript.transform.position.x, PlayerScript.transform.position.y + 2, PlayerScript.transform.position.z - 5);
				//transform.localRotation = Quaternion(transform.rotation.x, PlayerScript.transform.rotation.y,PlayerScript.transform.rotation.z,PlayerScript.transform.rotation.w);
				transform.LookAt(PlayerScript.transform);
				Debug.Log(transform.position);
				//Debug.Log(transform.localRotation);
				
				transform.parent = PlayerScript.transform;
			}else{
				isIso = true;
				transform.parent = null;
				transform.position = isoPos;
				//transform.localRotation = isoRot;
							Debug.Log(transform.position);
				Debug.Log(transform.rotation);
			}
		}
		lockedCamera = GUI.Toggle(new Rect(125,100,100,20), lockedCamera, "Locked camera?");
		GameObject GM = GameObject.Find("GameManager");
		GameObject Minion;
		if(ShowingOpts){
			GUILayout.BeginArea(new Rect(Screen.width/4, Screen.height-(Screen.height/5),Screen.width/2, Screen.height-(2*(Screen.height/5))), MenuBackground);
			GUILayout.BeginVertical();
			
			Opts.keepAttacking = GUILayout.Toggle(Opts.keepAttacking, "Keep attacking?");
			
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
		if(GUI.Button(new Rect(15,160, 100, 20), "Enemy Minion")){	
			Minion = (Network.Instantiate((GM.GetComponent(typeof(GameManager)) as GameManager).MinionObj, new Vector3(15, 15, 15), Quaternion.identity, 1) as GameObject);
			GM.networkView.RPC("SpawnMinion", RPCMode.All, Minion.name, PlayerScript.Team == 1 ? 2 : 1);
		}
		if(GUI.Button(new Rect(15,130, 100, 20), "Team Minion")){
			Minion = (Network.Instantiate((GM.GetComponent(typeof(GameManager)) as GameManager).MinionObj, new Vector3(15, 15, 15), Quaternion.identity, 1) as GameObject);
			GM.networkView.RPC("SpawnMinion", RPCMode.All, Minion.name, PlayerScript.Team);
		}
	}
}
