using UnityEngine;
using System.Collections;

namespace SuddenlyEntertainment{
	public class PlayerScriptClient : MonoBehaviour {
		// Use this for initialization
		void Start () {
		}
		
		// Update is called once per frame
		void Update () {
			
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
	}
}