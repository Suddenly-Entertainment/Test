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

		public float health;

		public float maxHealth;

		public float rotateSpeed;

		public GameObject Cam;

		// Use this for initialization
		void Start () {
			Stats = new UnitStats();
			Stats._moveSpeed.Base = 10;
			B_Stats = Stats;
			rotateSpeed = 2.5f;
			health = 1000;
			maxHealth = 1000;
		}
		
		// Update is called once per frame
		void Update () {
			if(Network.isClient){
				Vector3 AxisPress = new Vector3(Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
				networkView.RPC ("ClientAxis", RPCMode.Server, Network.player.guid, AxisPress, Input.GetAxis("Rotate"));

			}
		}

		[RPC]
		void ClientAxis(string player, Vector3 AxisPress, float rotate){
			if(Network.isServer){
				if(MainManager.PlayerDict[player].PlayerObj == gameObject){
					gameObject.GetComponent<CharacterController>().SimpleMove(transform.TransformDirection(AxisPress * (float)Stats.MoveSpeed));
					transform.Rotate(new Vector3(0, rotateSpeed*rotate,0));
				}
			}
		}

		[RPC]
		public void BasicAttack(string Attacker, string Target){
			if(Attacker == Target || Network.isClient)return;
			if(Attacker == OwnerClient){
				GameObject Proj = (Network.Instantiate(Projectile, transform.position + new Vector3(0, 2, 0), Quaternion.identity, 0) as GameObject);
				ProjectileServer PS = (Proj.AddComponent<ProjectileServer>() as ProjectileServer);
				Rigidbody PSR = Proj.AddComponent<Rigidbody>();
				PSR.isKinematic = true;
				PS.Target = Target;

			}
		}

		[RPC]
		public void SetPlayer(string player, string nickname){
			OwnerClient = player;
			gameObject.name = nickname;
			MainManager.PlayerDict[player].PlayerObj = gameObject;
		}

		void OnSerializeNetworkView(BitStream Stream, NetworkMessageInfo Msg){
			Vector3 Pos = Vector3.zero;
			Quaternion Rot = Quaternion.identity;

			float CurrentHealth = 0;
			float Gold = 0;
			float Expierence = 0;
			if(Stream.isReading){
				Stream.Serialize(ref Pos);
				transform.position = Pos;

				Stream.Serialize(ref Rot);
				transform.rotation = Rot;

				Stream.Serialize(ref CurrentHealth);
				Stats.CurrentHealth = CurrentHealth;

				Stream.Serialize(ref Gold);
				Stats.Gold = Gold;

				Stream.Serialize(ref Expierence);
				Stats.Expierence = Expierence;
			}else{
				Pos = transform.position;
				Stream.Serialize(ref Pos);

				Rot = transform.rotation;
				Stream.Serialize(ref Rot);

				CurrentHealth = (float)Stats.CurrentHealth;
				Stream.Serialize(ref CurrentHealth);

				Gold = (float)Stats.Gold;
				Stream.Serialize(ref Gold);

				Expierence = (float)Stats.Expierence;
				Stream.Serialize(ref Expierence);
			}
		}

		[RPC]
		public void RecieveStats(string Serial){
			if(Network.isClient){
				Stats = fastJSON.JSON.Instance.ToObject<UnitStats>(Serial);
			}
		}

		[RPC]
		public void Freeze(string guid){
			Cam.transform.parent = null;
			GetComponent<MeshRenderer>().enabled = false;
		}
		[RPC]
		public void Unfreeze(string guid){
			Cam.transform.parent = transform;
			GetComponent<MeshRenderer>().enabled = true;
		}

		[RPC]
		public void SpectateOther(){
			GameObject spec = gameObject;
			foreach(KeyValuePair< string, ClientSetupInfo > clientInfo in MainManager.PlayerDict){
				if(clientInfo.Key != OwnerClient){
					spec = clientInfo.Value.PlayerObj;
				}
			}
			Cam.transform.position = spec.transform.position + new Vector3(-5, 2, 0);
			Cam.transform.parent = spec.transform;
		}
		[RPC]
		public void SpectateSelf(){
			Cam.transform.position = transform.position + new Vector3(-5, 2, 0);
			Cam.transform.parent = transform;
		}
	}
}