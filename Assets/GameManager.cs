using UnityEngine;
using System.Collections;
namespace SuddenlyEntertainment{

	public delegate void NewPlayerEventHandler(object sender, string player);

	public class GameManager : MonoBehaviour {

		public event NewPlayerEventHandler NewPlayer;

		public GameObject PlayerCameraObj;

		public GameObject PlayerObj; //Do not instatiate on the client

		public GameObject TestCollider;

		// This is called before Start, be careful of what you do here.  Errors abound.
		void Awake(){
			DontDestroyOnLoad(this);
			MainManager.GM = gameObject;
			Debug.Log (System.IO.Directory.GetCurrentDirectory());
			XMLFileManager.SetupItems();
		}
		// Use this for initialization
		void Start () {

		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public void CallNewPlayer(object sender, string player){
			NewPlayer(sender, player);
		}

		[RPC]
		public void loadGame(){
			Network.maxConnections = -1;
			
			Network.SetSendingEnabled(0, false);
			Network.isMessageQueueRunning = false;
			
			Application.LoadLevel("Game");
			Debug.Log ("We have reached the point where we would load the game");
		}

		[RPC]
		public void ACTIVATEOBJ(NetworkPlayer Client){
			ClientSetupInfo clientInfo = MainManager.PlayerDict[Client.guid];
			GameObject clientObj = clientInfo.PlayerObj;
			clientObj.BroadcastMessage("ACTIVATE", SendMessageOptions.DontRequireReceiver);
		}
		[RPC]
		public void GameIsInitializedNow(){
			MainManager.GameInitialized = true;
		}
	}
}