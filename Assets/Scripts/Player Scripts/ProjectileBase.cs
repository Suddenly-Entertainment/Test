using UnityEngine;
using System.Collections;

public class ProjectileBase : MonoBehaviour {

	public Collider target;
	public NetworkViewID targetViewID;

	public string firedBy;

	public bool doneSet;
	public int speed;
	public float damage;
	public NetworkViewID viewID;
	public bool firedOnce;
	public bool hadTarget;
	public bool alreadyHit;

	//A function to initialize values;
	public virtual void initVarsToDefaultValue(){
		firedBy = "";
		doneSet = false;
		alreadyHit = false;
		hadTarget = false;
		firedOnce = false;
		damage = 50f;
		speed = 10;
	}

	public virtual void Awake(){
		initVarsToDefaultValue();
	}
	// Use this for initialization
	public virtual void Start () {
		if(!doneSet){
			gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if(doneSet){
			gameObject.SetActive(true);
		}
		if(target != null){
			transform.LookAt(target.transform);
			// The step size is equal to speed times frame time.
			float step = speed * Time.deltaTime;
			
			// Move our position a step closer to the target.
			transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
			
			hadTarget = true;
		}else if(target == null && hadTarget){
			
		}
	}
	public virtual void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo  info){
		var pos = transform.position;
		stream.Serialize(ref pos);
		transform.position = pos;
		if(stream.isReading){
			if(target == null && !hadTarget){
				stream.Serialize(ref targetViewID);
				target = NetworkView.Find(targetViewID).collider;
			}
			//if(!firedBy)stream.Serialize(firedBy);
			stream.Serialize(ref speed);
			stream.Serialize(ref damage);
			stream.Serialize(ref doneSet);
			//if(!firedOnce)stream.Serialize(firedOnce);
		}
		
		
		
	}

	public virtual void OnCollisionEnter(Collision info){
		Debug.Log("Colided");
		if(Network.isServer){
			if(target != null){
				if(info.gameObject.name == target.name && !alreadyHit){
					/*if(Network.isClient){
						networkView.RPC("ServerRemovial", RPCMode.Server);
					}else if(Network.isServer){
						ServerRemovial();
					}*/
					if(target.tag == "Player"){
						target.networkView.RPC("takeDamage", (target.GetComponent(typeof(Player))as Player).NetPlayer, damage);
					}else{
						target.networkView.RPC("takeDamage", RPCMode.All, damage);
					}
					
					Network.Destroy(gameObject);
					alreadyHit = true;
					//Network.RemoveRPCs(networkView.viewID);
				}
			}
		}
	}
}
