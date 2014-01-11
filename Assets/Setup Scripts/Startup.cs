using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SuddenlyEntertainment{
	public class Startup : MonoBehaviour {
		public string Menu;
		public string[] PlayerNames;

		ServerSetup serverSetup;
		ClientSetup clientSetup;
		public ClientSetupInfo clientInfo;

		public string[] BlueTeamNamesA;
		public string[] OrangeTeamNamesA;
		string PortStr;

		public Teams TestTeam;

		// Use this for initialization
		void Start () {
			Menu = "Start";
			PlayerNames = new string[12];

			clientInfo = new ClientSetupInfo();

			MainManager.GM.GetComponent<GameManager>().NewPlayer += new NewPlayerEventHandler(OnNewPlayer);
		}

		// Update is called once per frame
		void Update () {

		}

		void OnGUI(){
			GUILayout.BeginArea(new Rect(0,0,400,400));
			switch(Menu){
			case "Start":
				StartMenu();
				break;
			case "ServerSetup":
				ServerSetupMenu();
				break;
			case "ClientSetup":
				ClientSetupMenu();
				break;
			case "Lobby":
				LobbyMenu();
				break;
			case "Connecting":
				break;
			default:
				Debug.Log ("Menu's string is unknown! String is " + Menu);
				break;
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndArea ();
		}

		void StartMenu(){
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Which are you?: ");

			if(GUILayout.Button ("Server")){
				Menu = "ServerSetup";
				serverSetup = MainManager.GM.AddComponent<ServerSetup>();
				return;
			}
			if(GUILayout.Button("Client")){
				Menu = "ClientSetup";
				clientSetup = MainManager.GM.AddComponent<ClientSetup>();
				return;
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}

		/// <summary>
		/// Sets up and controls the GUI for the Server Setup Menu.
		/// </summary>
		void ServerSetupMenu(){
			GUILayout.BeginVertical();

			//Setting up the row for Port
			GUILayout.BeginHorizontal();
				PortGUI ();
				GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			//Start Server
			if(GUILayout.Button ("Start server")){
				serverSetup.StartServer();
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
		}

		/// <summary>
		/// Sets up and controls the GUI for the Client Setup Menu.
		/// </summary>
		void ClientSetupMenu(){
			GUILayout.BeginVertical();

			GUILayout.BeginHorizontal();
				GUILayout.Label ("Nickname: ");
				clientInfo.Nickname = GUILayout.TextField(clientInfo.Nickname);
				GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			//Setting up the row for IP
			GUILayout.BeginHorizontal();
				GUILayout.Label ("IP: ");
				ServerInfo.IP = GUILayout.TextField (ServerInfo.IP);
				GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			
			//Setting up the row for Port
			GUILayout.BeginHorizontal();
				PortGUI ();
				GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			//Setting up toolbar for team selection
			clientInfo.Team = (Teams)(GUILayout.Toolbar((int)clientInfo.Team, System.Enum.GetNames(typeof(Teams))));
			TestTeam = clientInfo.Team;
			//Start Server
			if(GUILayout.Button ("Join server") && clientInfo.Team != Teams.NA){
				clientSetup.JoinServer(clientInfo);
				Menu = "Connecting";
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
		}
		/// <summary>
		/// Sets up the GUI for the Port
		/// </summary>
		void PortGUI(){
			GUILayout.Label ("Port: ");
			//Because port needs to be an int, we have to try and parse it out.
			int PortTempHolder;
			if(int.TryParse(GUILayout.TextField (ServerInfo.Port.ToString()), out PortTempHolder)){
				ServerInfo.Port = PortTempHolder;
			}
		}
		public void OnNewPlayer(object sender, NetworkPlayer player){

		}

		void LobbyMenu(){
			List<string> BlueTeamNames =
				(from Player in MainManager.PlayerDict
				 where Player.Value.Team == Teams.Blue
				 select Player.Value.Nickname).ToList();

			List<string> OrangeTeamNames =
				(from Player in MainManager.PlayerDict
				 where Player.Value.Team == Teams.Orange
				 select Player.Value.Nickname).ToList();

			List<string> AllOtherPlayersNames =
				(from Player in MainManager.PlayerDict
				 where Player.Value.Team != Teams.Orange && Player.Value.Team != Teams.Blue
				 select Player.Value.Nickname).ToList();

			BlueTeamNamesA = BlueTeamNames.ToArray();

			OrangeTeamNamesA = OrangeTeamNames.ToArray();
			GUILayout.BeginHorizontal();

			GUILayout.BeginVertical();
				GUILayout.Label("Blue Team");
				foreach(string name in BlueTeamNames)GUILayout.Label(name);
				GUILayout.FlexibleSpace();
			GUILayout.EndVertical();

			GUILayout.BeginVertical();
				GUILayout.Label("Orange Team");
				foreach(string name in OrangeTeamNames)GUILayout.Label(name);
				GUILayout.FlexibleSpace();
			GUILayout.EndVertical();



			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.BeginVertical();
				GUILayout.Label("Other Players");
				GUILayout.BeginHorizontal();
					foreach(string name in AllOtherPlayersNames)GUILayout.Label(name);
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			GUILayout.EndVertical();

			if(Network.isServer){
				GUILayout.BeginHorizontal();
					if(GUILayout.Button ("Start The Show!")){
						MainManager.GM.GetComponent<GameManager>().loadGame();
					}
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
		}
	}
}
