using UnityEngine;
using System.Collections;
namespace SuddenlyEntertainment{

	public class GameManager : MonoBehaviour {
		// This is called before Start, be careful of what you do here.  Errors abound.
		void Awake(){
			DontDestroyOnLoad(this);
			MainManager.GM = gameObject;
		}
		// Use this for initialization
		void Start () {

		}
		
		// Update is called once per frame
		void Update () {
		
		}
		[RPC]
		public void loadGame(){
			Network.maxConnections = -1;
			
			Network.SetSendingEnabled(0, false);
			Network.isMessageQueueRunning = false;
			
			Application.LoadLevel("Game");
			Debug.Log ("We have reached the point where we would load the game");
		}
	}
}