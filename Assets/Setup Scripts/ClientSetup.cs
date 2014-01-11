using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SuddenlyEntertainment{
	public class ClientSetup : MonoBehaviour {
		Dictionary<string, object> Info;

		public GameObject PlayerCameraObj;
		public 
		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}

		public void JoinServer(ClientSetupInfo clientInfo){
			Info = clientInfo.NetPrep();
			Network.Connect(ServerInfo.IP, ServerInfo.Port);
		}

		public void OnConnectedToServer(){
			networkView.RPC ("InitializeClient", RPCMode.Server, Network.player, (string)Info["Nickname"], (int)Info["Team"]);
		}
		[RPC]
		public void InitializeClient(NetworkPlayer player, string b, int c){
			Debug.Log("Something for the client");
		}
		[RPC]
		public void AddPlayerInfo(NetworkPlayer player, string Nickname, int Team){
			MainManager.PlayerDict.Add(player, new ClientSetupInfo(Nickname, Team));
			MainManager.GM.GetComponent<GameManager>().CallNewPlayer(this, player);
		}

		[RPC]
		public void InitializationFinished(){
			Camera.main.GetComponent<Startup>().Menu = "Lobby";
		}

		[RPC]
		public void CreateCamera(){
			Instantiate(PlayerCameraObj);
		}
	}
}
