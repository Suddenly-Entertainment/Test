using UnityEngine;
using System.Collections;

public class Player : UnitBase {
	
	public CharacterController controller;
	public float rotateSpeed = 3.0f;
	public Vector3 curPos;
	public int PlayerNum;
	public NetworkPlayer NetPlayer;
	public CapsuleCollider CapCo;
	

	//A function to initialize values;
	public override void initVarsToDefaultValue(){
		base.initVarsToDefaultValue();
		PlayerNum = 0;
		rotateSpeed = 3.0f;


	}

	public override void Start () {
		if(isMine){ //These are the things that we do only if this is your player object
			Debug.Log("This Object Belongs to you and you can control it");
			
			(GetComponent(typeof(CharacterMotor)) as CharacterMotor).SetControllable(true);
			
			GameObject PCam = (Instantiate(PlayerCamera, new Vector3(this.transform.position.x,this.transform.position.y + 100,this.transform.position.z), this.transform.rotation) as GameObject);
			PCam.name = "PlayerCamera";//+networkView.viewID;
			PCam.tag = "MainCamera"; //For the GUIs to be properly rendered above the head
			//var testThing = GameObject.Find(PCam.name);
			//testThing.GetComponent(CameraControls).PlayerCheck = this;
			CameraControls CamControls = (PCam.GetComponent(typeof(CameraControls)) as CameraControls);
			CamControls.PlayerScript = this;
			PlayerCamera = PCam;
			//testThing.transform.parent = this.transform;
		}else{
			PlayerCamera = Camera.main.gameObject;
		}
		base.Start();

		objHeight = CapCo.height;
		objWidth = CapCo.radius*2;

		this.tag = "Player";
		

	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
		if(isMine){
			if(!isDead){
				//var controller : CharacterController = GetComponent(CharacterController);
				// Rotate around y - axis
				transform.Rotate(0, Input.GetAxis ("Horizontal") * rotateSpeed, 0);
				
				curPos = transform.position;
				// Move forward / backward
				Vector3 forward = transform.TransformDirection(Vector3.forward);
				float curSpeed = (Pather.speed * Input.GetAxis ("Vertical"))* Time.fixedDeltaTime;
				controller.SimpleMove(forward * curSpeed);
				/*if(currentHealth < maxHealth){
	        		currentHealth += healthRegen * Time.fixedDeltaTime;
	        	}else if(currentHealth > maxHealth){
	        		currentHealth = maxHealth;
	        	}*/
				
				//networkView.RPC("gainHealth", RPCMode.All, GenCre.healthRegen * Time.fixedDeltaTime);
			}
		}
	}

	[RPC]
	public override IEnumerator Die(){
		CapCo.enabled = false; //So that people and objects can go through him
		float endTime = Time.time+10;
		int cntr = 0;
		while(Time.time < endTime){
			if(cntr >= 100){Debug.Log("A tick on the old century frame"); cntr = 0; }
			yield return new WaitForFixedUpdate();
			cntr++;
		}
		transform.position = new Vector3(1,1,1);
		CapCo.enabled = true;
		isDead = false;
	}
	
	
	
	[RPC]
	public void SetupPlayerColor(float R, float G, float B){
		Material newMat = new Material(Mat);
		newMat.color = new Color(R, G, B);
		renderer.material = newMat;
		Mat = newMat;
	}

	[RPC]
	public void SetupPlayer(string PlayerName){
		Name = PlayerName != "" ? PlayerName : "Player without a name!";
	}

	// Use this for initialization



}
