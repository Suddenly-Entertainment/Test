using UnityEngine;
using System.Collections;
namespace SuddenlyEntertainment{
	public class PlayerScriptServer : MonoBehaviour {

		public PlayerScriptClient PSC;
		// Use this for initialization
		void Start () {
			GetComponent<PlayerScriptClient>();
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}