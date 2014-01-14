using UnityEngine;
using System.Collections;

namespace SuddenlyEntertainment{
	public class PlayerObjSetup : MonoBehaviour {

		public NetworkPlayer OwnerClient;


		// Use this for initialization
		void Start () {
//			MainManager.PlayerDict[OwnerClient].PlayerObj = gameObject;
			//gameObject.name = MainManager.PlayerDict[Network.player].Nickname;
		}

		[RPC]
		public void SetPlayer(NetworkPlayer player, string nickname){
			OwnerClient = player;
			gameObject.name = nickname;
		}

		// Update is called once per frame
		void Update () {
		
		}
	}
}
