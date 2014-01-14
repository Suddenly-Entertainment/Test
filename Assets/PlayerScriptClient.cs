using UnityEngine;
using System.Collections;

namespace SuddenlyEntertainment{
	public class PlayerScriptClient : MonoBehaviour {
		public Vector3 AxisPress;
		public float MoveSpeed = 10;
		// Use this for initialization
		void Start () {
		}
		
		// Update is called once per frame
		void Update () {
			Vector3 AxisPress = new Vector3(Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
			networkView.RPC ("ClientAxis", RPCMode.Server, Network.player, AxisPress);
		}

		[RPC]
		void ClientAxis(NetworkPlayer player, Vector3 AxisPress){
			gameObject.GetComponent<CharacterController>().SimpleMove(AxisPress * MoveSpeed);
		}

		void OnSerializeNetworkView(BitStream Stream, NetworkMessageInfo Msg){
			Vector3 Pos = Vector3.zero;
			float MS = MoveSpeed;
			if(Stream.isReading){
				Stream.Serialize(ref Pos);
				transform.position = Pos;

				Stream.Serialize(ref MS);
				MoveSpeed = MS;
			}else{
				Pos = transform.position;
				Stream.Serialize(ref Pos);

				Stream.Serialize(ref MS);
			}
		}
	}
}