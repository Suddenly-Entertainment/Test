using UnityEngine;
using System.Collections;

namespace SuddenlyEntertainment{
	public class PlayerScriptClient : MonoBehaviour {
		public Vector3 AxisPress;

		public string OwnerClient;

		public GameObject Projectile;
		public UnitStats B_Stats;
		public UnitStats Stats;

		// Use this for initialization
		void Start () {
			Stats = new UnitStats();
			Stats._moveSpeed.Base = 10;
			B_Stats = Stats;
		}
		
		// Update is called once per frame
		void Update () {
			if(Network.isClient){
				Vector3 AxisPress = new Vector3(Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
				networkView.RPC ("ClientAxis", RPCMode.Server, Network.player.guid, AxisPress);
			}
		}

		[RPC]
		void ClientAxis(string player, Vector3 AxisPress){
			if(Network.isServer){
				if(MainManager.PlayerDict[player].PlayerObj == gameObject){
					gameObject.GetComponent<CharacterController>().SimpleMove(AxisPress * (float)Stats.MoveSpeed);
				}
			}
		}

		[RPC]
		public void BasicAttack(NetworkPlayer Attacker, NetworkPlayer Target){
			if(Attacker == Target || Network.isClient)return;
			if(Attacker.guid == OwnerClient){
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
			if(Stream.isReading){
				Stream.Serialize(ref Pos);
				transform.position = Pos;
			}else{
				Pos = transform.position;
				Stream.Serialize(ref Pos);

			}
		}

		[RPC]
		public void RecieveStats(string Serial){
			if(Network.isClient){
				Stats = fastJSON.JSON.Instance.ToObject<UnitStats>(Serial);
			}
		}
	}
}