using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using fastJSON;

namespace SuddenlyEntertainment{
	public class ClientSetup : MonoBehaviour {
		Dictionary<string, object> Info;

		public GameObject PlayerCameraObj;

		// Use this for initialization
		void Start () {
			PlayerCameraObj = MainManager.GM.GetComponent<GameManager>().PlayerCameraObj;
		}

		// Update is called once per frame
		void Update () {

		}

		public void JoinServer(ClientSetupInfo clientInfo){
			Info = clientInfo.NetPrep();
			Network.Connect(ServerInfo.IP, ServerInfo.Port);
		}

		public void OnConnectedToServer(){
			string Serial = JSON.Instance.ToJSON(Info);
				
			networkView.RPC ("InitializeClient", RPCMode.Server, Network.player, Serial);
		}
		[RPC]
		public void InitializeClient(NetworkPlayer player, string b){
			Debug.Log("Something for the client");
		}
		[RPC]
		public void AddPlayerInfo(NetworkPlayer player, string Serial){
			MainManager.PlayerDict.Add(player, JSON.Instance.ToObject<ClientSetupInfo>(Serial));
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
		void OnLevelWasLoaded(int level){
			// Allow receiving data again
			Network.isMessageQueueRunning = true;
			// Now the level has been loaded and we can start sending out data to clients
			Network.SetSendingEnabled(0, true);
			
			networkView.RPC ("ClientLoadedLevel", RPCMode.Server, Network.player);
		}
		[RPC]
		public  void ClientLoadedLevel(NetworkPlayer Client){}
	}
}
