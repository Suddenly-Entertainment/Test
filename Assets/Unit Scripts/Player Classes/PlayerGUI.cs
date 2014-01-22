using UnityEngine;
using System.Collections;
namespace SuddenlyEntertainment
{

	public class PlayerGUI : MonoBehaviour
	{
			
		PlayerScriptClient PSC;
		public bool StatMenu;
		public bool OptionsMenu;
		// Use this for initialization
		void Start ()
		{
			StatMenu = false;
			OptionsMenu = false;
			PSC = GetComponent<PlayerScriptClient> ();
		}	
	
		// Update is called once per frame
		void Update () {
			if(Input.GetButtonUp ("StatMenu")){
				StatMenu = !StatMenu;
			}
			if(Input.GetButtonUp ("OptionsMenu")){
				OptionsMenu = !OptionsMenu;
			}
		}
		public Vector2 scrollPosition;
		void OnGUI(){
			if(StatMenu){
				GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
				string LOLK = PSC.Stats.GetNiceString(true);
				GUILayout.Label(LOLK);
				GUILayout.EndArea ();
			}
			if(OptionsMenu){
				GUILayout.BeginArea(new Rect(0,Screen.height-50,200,50));
				GUILayout.BeginHorizontal();
				if(GUILayout.Button("Quit")){
					Application.Quit();
				}
				if(GUILayout.Button ("Reset")){
					MainManager.Reset();
				}
				GUILayout.EndHorizontal();
				GUILayout.EndArea();
			}

		}
	}
}