using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fastJSON;

namespace SuddenlyEntertainment{
	/// <summary>
	/// This class is for setting up the server!
	/// </summary>
	public class ServerSetup : MonoBehaviour {

		public GameObject PlayerObj; //This is hte one you will instantiate on the client

		// Use this for initialization
		void Start () {
			PlayerObj = MainManager.GM.GetComponent<GameManager>().PlayerObj;
		}

		// Update is called once per frame
		void Update () {

		}
		/// <summary>
		/// Starts the Server. 
		/// Note that the reason this function does not take any parameters
		/// is because it gets them from the static class ServerInfo.
		/// </summary>
		public void StartServer(){
			Network.InitializeServer(24, ServerInfo.Port, false);
			Camera.main.GetComponent<Startup>().Menu = "Lobby";
		}

		void OnPlayerConnected(NetworkPlayer Player){
			MainManager.uninitializedPlayers.Add(Player);

		}

		[RPC]
		public void InitializeClient(NetworkPlayer Player, string Serial){

			ClientSetupInfo PlayerInfo = JSON.Instance.ToObject<ClientSetupInfo>(Serial);


			MainManager.PlayerDict.Add(Player, PlayerInfo);
			MainManager.uninitializedPlayers.Remove (Player);
			UpdateOtherClientPlayerInfo(Player, Serial);

			SetupClientPlayerInfo(Player);

			networkView.RPC ("InitializationFinished", Player);
		}
		[RPC]
		public void AddPlayerInfo(NetworkPlayer player, string a){}
		[RPC]
		public void InitializationFinished(){}

		public void SetupClientPlayerInfo(NetworkPlayer Client){

			foreach(KeyValuePair<NetworkPlayer, ClientSetupInfo> clientInfo in MainManager.PlayerDict){
				string Serial = JSON.Instance.ToJSON(clientInfo.Value);

				
				networkView.RPC("AddPlayerInfo", Client, clientInfo.Key, Serial);
			}

		
		}

		public void UpdateOtherClientPlayerInfo(NetworkPlayer Client, string Serial){

			foreach(KeyValuePair<NetworkPlayer, ClientSetupInfo> clientInfo in MainManager.PlayerDict){
				if(clientInfo.Key != Client)
				networkView.RPC("AddPlayerInfo", clientInfo.Key, Client, Serial);
			}

		}

		void OnLevelWasLoaded(int level){
			// Allow receiving data again
			Network.isMessageQueueRunning = true;
			// Now the level has been loaded and we can start sending out data to clients
			Network.SetSendingEnabled(0, true);

			networkView.RPC ("loadGame", RPCMode.OthersBuffered);
		}

		[RPC]
		public void ClientLoadedLevel(NetworkPlayer Client){
			MainManager.LoadedClients.Add(Client);

			if(MainManager.LoadedClients.Count == MainManager.PlayerDict.Count){
				SendGameInitializationData();
			}
		}
		Vector3 basePos = new Vector3(5, 0, 5);
		public void SendGameInitializationData(){
			int cntr = 1;
			List<GameObject> PlayerObjs = new List<GameObject>();
			//GameObject PlayerObj2 = (Instantiate(PlayerObj) as GameObject);
			//PlayerObj2.GetComponent<PlayerObjSetup>().OwnerClient = clientInfo.Key;
			//PlayerObj2.AddComponent<PlayerScriptClient>();
			//PlayerObj2.networkView.observed = PlayerObj.GetComponent<PlayerScriptClient>();

			foreach(KeyValuePair<NetworkPlayer, ClientSetupInfo> clientInfo in MainManager.PlayerDict){
				Debug.Log (JSON.Instance.Beautify(JSON.Instance.ToJSON(clientInfo)));

				UnityEngine.Object playerobj = Network.Instantiate(PlayerObj, new Vector3(0, 1, 0) + (basePos*cntr), Quaternion.identity, 0);
				(playerobj as GameObject).AddComponent<PlayerScriptServer>();
				PlayerObjs.Add((playerobj as GameObject));
				(playerobj as GameObject).networkView.RPC ("SetPlayer", RPCMode.All, clientInfo.Key, clientInfo.Value.Nickname);
				clientInfo.Value.PlayerObj = (playerobj as GameObject);



				networkView.RPC ("CreateCamera", clientInfo.Key);
			}
			//Destroy(PlayerObj2);

			foreach(KeyValuePair<NetworkPlayer, ClientSetupInfo> clientInfo in MainManager.PlayerDict){
				//networkView.RPC ("ACTIVATEOBJ", RPCMode.All, clientInfo.Key);
				networkView.RPC ("GameIsInitializedNow", clientInfo.Key);
			}

			MainManager.GameInitialized = true;
		}

		[RPC]
		public void CreateCamera(){}
	}
}