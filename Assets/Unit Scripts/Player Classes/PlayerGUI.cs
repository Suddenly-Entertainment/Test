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
		void OnGui(){
			GUILayout.BeginArea (new Rect (0, Screen.height-50, Screen.width, 50));
			scrollPosition = GUILayout.BeginScrollView(scrollPosition: scrollPosition);
			GUILayout.BeginVertical ();

			GUILayout.BeginHorizontal ();

			GUILayout.Label(PSC.Stats.GetNiceString(true));
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();


			GUILayout.FlexibleSpace ();
			GUILayout.EndVertical ();

			GUILayout.EndScrollView();
			GUILayout.FlexibleSpace ();
			GUILayout.EndArea ();
		}
	}
}