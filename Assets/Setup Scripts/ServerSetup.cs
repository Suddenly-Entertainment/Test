using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SuddenlyEntertainment{
	/// <summary>
	/// This class is for setting up the server!
	/// </summary>
	public class ServerSetup : MonoBehaviour {

		public GameObject PlayerObj;

		// Use this for initialization
		void Start () {

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
		public void InitializeClient(NetworkPlayer Player, string Nickname, int Team){
			ClientSetupInfo PlayerInfo = new ClientSetupInfo(Nickname, Team);
			MainManager.PlayerDict.Add(Player, PlayerInfo);
			MainManager.uninitializedPlayers.Remove (Player);

			SetupClientPlayerInfo(Player);
			networkView.RPC ("InitializationFinished", Player);
		}
		[RPC]
		public void AddPlayerInfo(NetworkPlayer player, string Nickname, int Team){}
		[RPC]
		public void InitializationFinished(){}

		public void SetupClientPlayerInfo(NetworkPlayer Client){

			foreach(KeyValuePair<NetworkPlayer, ClientSetupInfo> clientInfo in MainManager.PlayerDict){
				Dictionary<string, object> Prep = clientInfo.Value.NetPrep();

				networkView.RPC("AddPlayerInfo", Client, clientInfo.Key, (string)Prep["Nickname"], (int)Prep["Team"]);
			}

		
		}

		void OnLevelWasLoaded(int level){
			// Allow receiving data again
			Network.isMessageQueueRunning = true;
			// Now the level has been loaded and we can start sending out data to clients
			Network.SetSendingEnabled(0, true);

			foreach(KeyValuePair<NetworkPlayer, ClientSetupInfo> clientInfo in MainManager.PlayerDict){
				networkView.RPC ("loadGame", RPCMode.OthersBuffered);
			}
		}

		[RPC]
		public void ClientLoadedLevel(NetworkPlayer Client){
			MainManager.LoadedClients.Add(Client);

			if(MainManager.LoadedClients.Count == MainManager.PlayerDict.Count){
				SendGameInitializationData();
			}
		}

		public void SendGameInitializationData(){
			foreach(KeyValuePair<NetworkPlayer, ClientSetupInfo> clientInfo in MainManager.PlayerDict){
				PlayerObj.GetComponent<PlayerObjSetup>().OwnerClient = clientInfo.Key;

				//Network.Instantiate(PlayerObj,
			}
		}
	}
}