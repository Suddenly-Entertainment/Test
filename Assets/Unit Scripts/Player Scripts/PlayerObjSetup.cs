using UnityEngine;
using System.Collections;

namespace SuddenlyEntertainment{
	public class PlayerObjSetup : MonoBehaviour {

		public NetworkPlayer OwnerClient;


		// Use this for initialization
		void Start () {
			MainManager.PlayerDict[OwnerClient].PlayerObj = gameObject;
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
