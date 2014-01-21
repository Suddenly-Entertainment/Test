using UnityEngine;
using System.Collections;
namespace SuddenlyEntertainment
{

	public class PlayerGUI : MonoBehaviour
	{
			
		PlayerScriptClient PSC;
		// Use this for initialization
		void Start ()
		{
			PSC = GetComponent<PlayerScriptClient> ();
		}	
	
		// Update is called once per frame
		void Update () {

		}
		public Vector2 scrollPosition;
		void OnGUI(){
			GUILayout.BeginArea (new Rect (0, Screen.height-100, Screen.width, 100));
			string LOLK = PSC.Stats.GetNiceString(true);
			GUILayout.Label(LOLK);
			GUILayout.EndArea ();
		}
	}
}