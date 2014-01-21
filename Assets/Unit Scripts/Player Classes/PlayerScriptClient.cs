using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SuddenlyEntertainment{

	public class PlayerScriptClient : MonoBehaviour {

		public event EventHandler onSpawn;

		public Vector3 AxisPress;

		public string OwnerClient;

		public GameObject Projectile;
		public UnitStats B_Stats;

		public UnitStats Stats;

		public float rotateSpeed;

		public GameObject Cam;
		public GameObject Killer;

		public bool isDead;
		public float nextAttack;
		// Use this for initialization
		void Start () {
			Stats = new UnitStats(true);
			B_Stats = Stats;

			rotateSpeed = 2.5f;
			nextAttack = 0;
		}
		
		// Update is called once per frame
		void Update () {
			if(Network.isClient){
				if(txtMesh != null)txtMesh.text = beginTxt + hpTxt;
				Vector3 AxisPress = new Vector3(Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
				networkView.RPC ("ClientAxis", RPCMode.Server, Network.player.guid, AxisPress, Input.GetAxis("Rotate"));
				int Ability = 0;
				if(Input.GetButtonUp ("Ability1"))
					Ability = 1;
				else if(Input.GetButtonUp ("Ability2"))
					Ability = 2;
				else if(Input.GetButtonUp ("Ability3"))
					Ability = 3;
				else if(Input.GetButtonUp ("Ability4"))
					Ability = 4;

				if(Ability != 0)
					networkView.RPC ("UseAbility", RPCMode.Server, Network.player.guid, Ability);

			}
		}

		[RPC]
		public void UseAbility(string guid, int Ability){
			switch(Ability){
				case 1:
					break;
				case 2:
					break;
				case 3:
					break;
				case 4:
					break;
				default:
					break;
			}
		}
		public void CallOnSpawn(){
			if(onSpawn != null)
				onSpawn(this, System.EventArgs.Empty);
		}
		[RPC]
		void ClientAxis(string player, Vector3 AxisPress, float rotate){
			if(Network.isServer){
				if(MainManager.PlayerDict[player].PlayerObj == gameObject){
					if(!isDead){
						gameObject.GetComponent<CharacterController>().SimpleMove(transform.TransformDirection((AxisPress) * ((float)Stats.MoveSpeed*Time.deltaTime)));
						transform.Rotate(new Vector3(0, (rotateSpeed*Time.deltaTime)*rotate,0));
					}
				}
			}
		}

		[RPC]
		public void BasicAttack(string Attacker, string Target){
			if(Attacker == Target || Network.isClient || Time.time < nextAttack)return;
			if(Attacker == OwnerClient){
				GameObject TargetGO = MainManager.PlayerDict[Target].PlayerObj;
				if(MainManager.GM.GetComponent<GameManager>().AttackRange){
					Collider col = TargetGO.collider;
					Ray ray = new Ray(transform.position, col.transform.position);
					RaycastHit hitInfo;
					if(!col.Raycast(ray, out hitInfo, (float)Stats.AttackRange))return;
				}
				Vector3 LookPos = TargetGO.transform.position;
				LookPos.y = transform.position.y;
				transform.LookAt(LookPos);
				nextAttack = Time.time + (1/(float)Stats.AttackSpeed);

				GameObject Proj = (Network.Instantiate(Projectile, transform.TransformPoint(Vector3.forward), Quaternion.identity, 0) as GameObject);
				ProjectileServer PS = (Proj.AddComponent<ProjectileServer>() as ProjectileServer);
				PS.damage = setupBasicAttackDamage();
				Rigidbody PSR = Proj.AddComponent<Rigidbody>();
				PSR.isKinematic = true;
				PS.Target = Target;

			}
		}

		private Damage setupBasicAttackDamage(){
			Damage BAD = new Damage();
			BAD.Attacker = gameObject;
			BAD.AttackerName = MainManager.PlayerDict[OwnerClient].Nickname;
			BAD.AttackerTag = "Player";
			BAD.AttackerTeam = MainManager.PlayerDict[OwnerClient].Team;
			BAD.AttackerGUID = OwnerClient;
			BAD.isPlayer = true;

			BAD.TrueDamage = 1;
			BAD.PhysicalDamage = (float)Stats.AttackDamage;
			BAD.ArmorPenFlat = (float)Stats.ArmorPenFlat;
			BAD.ArmorPenPercent = (float)Stats.ArmorPenPercent;

			return BAD;
		}
		public TextMesh txtMesh;
		public string beginTxt;
		public string hpTxt;

		[RPC]
		public void SetPlayer(string player, string nickname){
			OwnerClient = player;
			gameObject.name = nickname;
			gameObject.AddComponent<PlayerGUI>();
			txtMesh = transform.FindChild("New Text").GetComponent<TextMesh>();
			beginTxt = nickname + "\n";
			hpTxt = Stats.CurrentHealth + " / " + Stats.MaxHealth + "\n";
			txtMesh.text = beginTxt + hpTxt;
			MainManager.PlayerDict[player].PlayerObj = gameObject;
		}

		void OnSerializeNetworkView(BitStream Stream, NetworkMessageInfo Msg){
			Vector3 Pos = Vector3.zero;
			Quaternion Rot = Quaternion.identity;

			//float CurrentHealth = 0;
			//float Gold = 0;
			//float Expierence = 0;

			bool Dead = false;
			if(Stream.isReading){
				Stream.Serialize(ref Pos);
				transform.position = Pos;

				Stream.Serialize(ref Rot);
				transform.rotation = Rot;

				/*Stream.Serialize(ref CurrentHealth);
				Stats.CurrentHealth = CurrentHealth;

				Stream.Serialize(ref Gold);
				Stats.Gold = Gold;

				Stream.Serialize(ref Expierence);
				Stats.Expierence = Expierence;*/

				UnitStats.StreamRecieve(Stream, Stats);

				Stream.Serialize (ref Dead);
				isDead = Dead;
			}else{
				Pos = transform.position;
				Stream.Serialize(ref Pos);

				Rot = transform.rotation;
				Stream.Serialize(ref Rot);

				/*CurrentHealth = (float)Stats.CurrentHealth;
				Stream.Serialize(ref CurrentHealth);

				Gold = (float)Stats.Gold;
				Stream.Serialize(ref Gold);

				Expierence = (float)Stats.Expierence;
				Stream.Serialize(ref Expierence);*/

				Dead = isDead;
				Stream.Serialize(ref Dead);

				UnitStats.StreamSend(Stream, B_Stats, Stats);
				B_Stats = Stats;
			}
		}

		[RPC]
		public void RecieveStats(string Serial){
			if(Network.isClient){
				Stats = fastJSON.JSON.Instance.ToObject<UnitStats>(Serial);
				hpTxt = Stats.CurrentHealth + " / " + Stats.MaxHealth + "\n";
			}
		}


		[RPC]
		public void SpectateOther ()
		{
			GameObject spec = gameObject;
			foreach (KeyValuePair< string, ClientSetupInfo > clientInfo in MainManager.PlayerDict) {
				if(clientInfo.Key != OwnerClient && !clientInfo.Value.PlayerObj.GetComponent<PlayerScriptClient> ().isDead) {
					spec = clientInfo.Value.PlayerObj;
				}
			}
			if(spec != gameObject) {
				Cam.transform.position = spec.transform.position;
				Cam.transform.parent = spec.transform;
				Cam.transform.position += new Vector3(-5, 2, 0);
				Cam.transform.LookAt(spec.transform);
			}
		}
		[RPC]
		public void SpectateSelf(){

			Cam.transform.position = transform.position;
			Cam.transform.parent = transform;
			Cam.transform.position += new Vector3(-5, 2, 0);
			Cam.transform.LookAt(transform);
		}
	}
}