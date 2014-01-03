using UnityEngine;
using System.Collections;

public class Player : UnitBase {
	
	public CharacterController controller;
	public float rotateSpeed = 3.0f;
	public Vector3 curPos;
	public int PlayerNum;
	public NetworkPlayer NetPlayer;
	public CapsuleCollider CapCo;

	public float deathTime; //In seconds.
	

	//A function to initialize values;
	public override void initVarsToDefaultValue(){
		base.initVarsToDefaultValue();
		PlayerNum = 0;
		rotateSpeed = 3.0f;


	}

	public override void Start () {
		if(isMine){ //These are the things that we do only if this is your player object
			Debug.Log("This Object Belongs to you and you can control it");
			
			GetComponent<CharacterMotor>().SetControllable(true);
			
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

		objHeight = 2;
		objWidth = .5f*2;

		this.tag = "Player";
		

	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
		//Debug.Log (controller.center);
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
	public override void Die(){
		isDead = true;
		curHealth = 0;
		deathTime = GMObj.GetComponent<ClassicMapSettings>().GetDeathTime(level);
		nextRespawnTime = Time.time + deathTime;
		GetComponent<CharacterMotor>().SetControllable(false);
		collider.enabled = false; //So that people and objects can go through him

		//transform.position = new Vector3(-1,-10,-1);
		//.enabled = true;
		//isDead = false;
	}
	
	[RPC]
	public override void Respawn(){
		isDead = false;
		curHealth = curMaxHealth;
		curResource = curMaxResource;
		nextRespawnTime = -1;
		transform.position = GMObj.GetComponent<ClassicMapSettings>().FindSpawnPos(Team);
		GetComponent<CharacterMotor>().SetControllable(false);
		collider.enabled = true;

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
