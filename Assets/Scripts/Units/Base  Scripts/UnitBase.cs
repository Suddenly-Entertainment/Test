using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SimplePathing))]
[RequireComponent (typeof (NetworkView))]

public class UnitBase : MonoBehaviour {

	
	/* Things that NEED to be set by specialized file for creature */
	public float objHeight; //The total height of the object
	public float objWidth; //The total width of the object
	public GameObject BasicAttackObj; //The object to use for Basic Attacks
	public System.Action<int> DeathFunc; //A public void to handle when the creature dies
	/*-------------------------------------------------------------*/
	
	/*-- Stats! --*/
	
	// Defense Stats
	public float curHealth;

	public float baseMaxHealth;
	public float curMaxHealth;
	
	public float baseHealthRegen;
	public float curHealthRegen;
	
	
	//Movement Stats
	public float baseSpeed;
	public float curSpeed;
	
	//Offense Stats
	
	//Basic Attack stats
	public float baseBasicDamage;
	public float curBasicDamage;

	public float baseAtkSpeed;
	public float curAtkSpeed;

	//Ability Stats
	public float baseAP;
	public float curAP;

	public float curResource;
	public float curMaxResource;
	public float baseMaxResource;

	public float baseResourceRegen;
	public float curResourceRegen;
	
	
	
	//Misc.  Stats
	public int level;
	public float goldOnDeath;
	
	/*------------*/
	
	public SimplePathing Pather;
	public GameObject PlayerCamera;
	
	public GUISkin MenuSkin;
	

	
	public Material Mat;
	
	public float nextRespawnTime;
	
	public float nextAtk;
	
	public bool isMine;
	public bool isDead;

	public int hpBarSize;
	public Texture2D hpBarImgPlayer;
	public Texture2D hpBarImgEnemy;
	public Texture2D hpBarImgAlly;
	public Vector3 HPBarPos;
	public Vector3 HPBarPosW;
	// HPBarPosS Vector3;
	public bool InterruptHPB;
	public string Name;
	
	public TEAMS Team;
	public NetworkView[] NetworkViews;
	
	public bool showingOutline;
	public Shader OutlineShader;
	public Shader NoOutlineShader;
	

	public GameObject GMObj;
	public GameManager GM;
	//A function to initialize values;
	public virtual void initVarsToDefaultValue(){
		GMObj = GameObject.Find ("GameManager");
		GM = GMObj.GetComponent<GameManager>();

		objHeight = 0f;
		objWidth = 0f;
		/*-- Stats! --*/
			
		// Defense Stats
		 curHealth = 1000;
		 baseMaxHealth = 1000;
		 curMaxHealth = 1000;
		
		 baseHealthRegen  = 10;
		 curHealthRegen  = 10;
		

		//Offense Stats
		
		//Basic Attack stats
		baseBasicDamage = 100f;
		curBasicDamage = 100f;

		baseAtkSpeed = 2.5f;
		curAtkSpeed = 2.5f;
		//Ability Stats
		baseAP = 0;
		curAP = 0;
		curResource  = 200f;
		baseMaxResource = 200f;
		curMaxResource  = 200f;
		curResourceRegen = 1f;
		baseResourceRegen  = 1f;
		
		
		
		//Misc.  Stats
		 level = 1;
		goldOnDeath = 50;
		
		/*------------*/

		nextRespawnTime = -1;

		nextAtk  = 0;

		isDead  = false;
		
		hpBarSize  = 100;
		InterruptHPB = false;
		Name = "";
		
		Team  = TEAMS.NA;
		NetworkViews = new NetworkView[10];
		
		showingOutline = false;
		OutlineShader = Shader.Find("Toon/Basic");
		NoOutlineShader = Shader.Find("Toon/Basic Outline");

	}

	virtual public void Awake(){
		isMine = networkView.isMine;
	}

	// Use this for initialization
	virtual public void Start () {
		initVarsToDefaultValue();
		Pather = (GetComponent(typeof(SimplePathing)) as SimplePathing);
		PlayerCamera = Camera.main.gameObject;
		baseSpeed = Pather.speed;
		curSpeed = baseSpeed;
		//isMine = networkView.isMine;
	}

	// Update is called once per frame
	virtual public void Update () {
		if(isMine){
			if(!isDead){
				if(curHealth <= 0){
					networkView.RPC("Die", RPCMode.All);
				}
				gainHealth(curHealthRegen * Time.fixedDeltaTime);
			}else{
				if(nextRespawnTime != -1 && nextRespawnTime >= Time.time){
					networkView.RPC ("Repsawn", RPCMode.Server);
				}
			}
		}
	}
	
	virtual public void BasicAttack(Collider target){
		if(!BasicAttackObj){
			Debug.LogWarning("The Creature wants to do a Basic Attack, but it has nothing to attack with!  (Did you forget to set the BasicAttackObj in the creaures script?)");
			return;
		}
		if(Time.time >= nextAtk){
			// thing = Network.Instantiate(BasicAttackObj, Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, 0);
			// viewID = Network.AllocateViewID();
			//Debug.LogWarning(viewID);
			/* uniqueNum = Random.value *100;
			 thingName = "Arrow" + uniqueNum;
			thing.name = thingName;
			 AutoAttack = GameObject.Find(thingName).GetComponent(ProjectileScript);
			AutoAttack.target = target;
			AutoAttack.firedBy = this.name;
			Debug.Log("Arrow Fired "+uniqueNum);*/
			if(!Network.isServer)
			networkView.RPC("createAtk", RPCMode.Server, target.name);//, viewID);
			else
				createAtk(target.name);
			
			nextAtk = Time.time + (1/curAtkSpeed);
		}
	}

	virtual public void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info){
		/*if(stream.isWriting){
			stream.Serialize(curHealth);
	
		} else {
		
		}*/
		
		//Debug.LogError("It's not an error!  It's working!");
		// curHealth = curHealth;
		stream.Serialize(ref curHealth);

		Vector3 pos = transform.position;
		stream.Serialize(ref pos);
		transform.position = pos;
		
		stream.Serialize (ref isDead);
		//stream.Serialize(Name);
	}

	[RPC]
	virtual public void gainHealth(float hp){
		if(hp == 0)return;

		if(hp < 0){
			takeDamage (hp);
			return;
		}

		if(curHealth + hp <= curMaxHealth){
			curHealth += hp;
		}else if(curHealth + hp > curMaxHealth){
			curHealth += (curHealth + hp) - curMaxHealth;
		}
		if(curHealth > curMaxHealth){
			curHealth = curMaxHealth;
		}
	}
	
	[RPC]
	virtual public void takeDamage(float hp){
		if(hp == 0)return;
		
		if(hp > 0){
			gainHealth (hp);
			return;
		}
		curHealth -= hp;
		if(curHealth <= 0){
			networkView.RPC("Die", RPCMode.All);
		}
	}
	
	[RPC]
	virtual public void createAtk(string targetName){//, viewID NetworkViewID){
			//Debug.Log(thing); Debug.Log(target);
			Debug.Log("Creating attack!");
			Collider target = GameObject.Find(targetName).collider;
			Debug.Log(target);
			
			GameObject basicAttack = (Network.Instantiate(BasicAttackObj, transform.position+new Vector3(objWidth/2f+0.1f,0f,0f), Quaternion.identity, 0) as GameObject);
			Debug.Log(basicAttack);
			
			ProjectileBase AutoAttack = (basicAttack.GetComponent(typeof(ProjectileBase)) as ProjectileBase);//.target = GameObject.Find(targetName).collider;
			Debug.Log(AutoAttack);
			AutoAttack.target = target;
			AutoAttack.targetViewID = target.networkView.viewID;
			
			// uniqueNum = ""+viewID.ToString().Substring(12);
			// thingName = "Arrow " + viewID.ToString().Substring(12);
			//.GetComponent(ProjectileScript);
			//basicAttack.name = thingName;
			
			/* nView = basicAttack.GetComponent(NetworkView);
			nView.viewID = viewID;
			AutoAttack.viewID = viewID;*/
	
			
			AutoAttack.firedBy = this.name;
	
			AutoAttack.doneSet = true;
			//Debug.Log("Arrow Fired "+uniqueNum);
	
	}

	[RPC]
	virtual public void Die(){
		isDead = true;
	}

	[RPC]
	virtual public void Respawn(){
		isDead = false;
		nextRespawnTime = -1;
	}
	
	Texture2D hpBar;
	public float Scale;
	public float Mag;
	virtual public void OnGUI(){
		if(objHeight == 0){
			Debug.LogWarning("Shit!  objHeight isn't set!  Hopefully, you just forgot to set it in the creatures script!  For now, we are going to use a guess (2 units), but this is probably not perfect!  Expect the HP Bar to be placed, and/or behave weird!");
		}
			Player MainGC = (PlayerCamera.GetComponent(typeof(CameraControls))as CameraControls).PlayerScript; //Gets the Generic Creature script from the main (read only) camera's player
			HPBarPosW = transform.position - new Vector3(0f, -((objHeight != 0f ? objHeight : 2f)/2f) -0.5f, 0f); //This is the world position of the health bar that we will convert into screen cordinates so we can keep the hp bars right above their heads
			Vector3 screenPos = Camera.main.WorldToScreenPoint(HPBarPosW); //Convert it to where it should be on the screen
			float percHP = curHealth / curMaxHealth;
			
			if(!hpBar)hpBar = isMine ? hpBarImgPlayer : MainGC.Team == Team ? hpBarImgAlly : hpBarImgEnemy;
			//hpBar.Resize(parseInt(hpBar.width*percHP),hpBar.height);
			//hpBar.Apply();
			int curHPBarSize = (int)(hpBarSize*percHP);
			
		Mag = Vector3.Distance(PlayerCamera.transform.position, transform.position);

			if(!InterruptHPB){ //This allows us to modify the position from the inspector
				HPBarPos.x = screenPos.x -(hpBarSize/2); //Centering it
				HPBarPos.y = Screen.height-screenPos.y;//-40;  //I don't know exactly why, but we have to do it like this.  It makes it so it isent on the bottom of the model.
				HPBarPos.z = screenPos.z; //Is it infront of the camera? This value tells us that!  If it is positive, it is front of the camera, else it is not, and we shouldnt show the player the HP bar.
			}
		Scale = 1;//-1/Mathf.Abs(PlayerCamera.camera.WorldToViewportPoint(HPBarPosW).z);
			if(HPBarPos.z >= 0){
			GUI.Box(new Rect(HPBarPos.x ,HPBarPos.y,(int)(hpBarSize*Scale),(int)(20*Scale)), ""); //A empty container box
			GUI.Box(new Rect(HPBarPos.x,HPBarPos.y,(int)(curHPBarSize*Scale),(int)(20*Scale)), hpBar); //This is the hp bar, which conviently also changes depending on wither it is you, ally, or enemy.
			GUI.Label(new Rect(screenPos.x-(hpBarSize/4),Screen.height-screenPos.y, (int)(100*Scale), (int)(20*Scale)), curHealth+"/"+curMaxHealth); //This tells you the actual health
				if(Name != "")GUI.Box(new Rect(screenPos.x-(hpBarSize/2),Screen.height-screenPos.y-20, (int)(100*Scale), (int)(20*Scale)), Name, MenuSkin.box); //The anem above the player
				//GUI.Label(Rect(screenPos.x-(hpBarSize/4),Screen.height-screenPos.y-20, 100, 20), Name);
			}
		//}
	}
	
	virtual public void toggleOutline(bool toggle){
		if(showingOutline == toggle)return;
		showingOutline = toggle;
		Material newMat = new Material(Mat);
		newMat.shader = showingOutline ? NoOutlineShader : OutlineShader;
		renderer.material = newMat;
		Mat = newMat;
	}
}
