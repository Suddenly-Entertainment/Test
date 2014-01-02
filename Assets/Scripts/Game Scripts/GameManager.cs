using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TEAMS {NA = -1, NEUTRAL, BLUE, ORANGE, SPECTATOR, OTHER};

public class GameManager : MonoBehaviour {
	public GameObject MinionObj;
	//Dictionary clientsFinished;
	public GameObject PlayerObj;
	public Dictionary<NetworkPlayer, GameObject> NetPlayerToGameObjectDict = new Dictionary<NetworkPlayer, GameObject>();
	public Dictionary<GameObject, NetworkPlayer> GameObjectToNetPlayerDict = new Dictionary<GameObject, NetworkPlayer>();
	public NetworkPlayer[] BlueTeam = new NetworkPlayer[10];
	public NetworkPlayer[] OrangeTeam = new NetworkPlayer[10];
	public NetworkPlayer[] Spectators = new NetworkPlayer[10];
	public Dictionary<NetworkPlayer, TEAMS> PlayerTeams = new Dictionary<NetworkPlayer, TEAMS>(); 

	// Use this for initialization
	void Start () {
		BlueTeam = new NetworkPlayer[10];
		OrangeTeam = new NetworkPlayer[10];
		Spectators = new NetworkPlayer[10];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onNetworkLoadedLevel(string level, string PlayerName){
		Debug.Log("Level loaded: "+level);
		if(level != "Lobby" || level != "MainMenu"){
			GameObject P = PlayerObj; //I am certain that this will be overwritten, and even if not, I don't think there is a problem.
			GetComponent<LevelLoader>().PlayerCount++;
			string pName = "";
			NetworkPlayer NetPlayer = Network.player;
			Color PlayerColor = new Color(0,0,0);
			if(level == "TownHub"){
				P = (Network.Instantiate(PlayerObj, new Vector3(10,10,10), Quaternion.identity, 2) as GameObject);
				pName = P.name;
				PlayerColor.r = UnityEngine.Random.value;
				PlayerColor.g = UnityEngine.Random.value;
				PlayerColor.b = UnityEngine.Random.value;
			}else if(level == "ClassicMap"){
				GetComponent<ClassicMapSettings>().CreateMap();
				P = GetComponent<ClassicMapSettings>().SpawnPlayer(this, PlayerObj, out pName, out PlayerColor, PlayerName, NetPlayer);
			}
			NetPlayerToGameObjectDict.Add (NetPlayer, P);
			GameObjectToNetPlayerDict.Add (P, NetPlayer);
			PlayerSetup(P, pName, NetPlayer, PlayerName, PlayerColor);
		}
	}
	public void PlayerSetup(GameObject P, string pName, NetworkPlayer NetPlayer, string PlayerName, Color PlayerColor){
		P.networkView.RPC("SetupPlayerColor", RPCMode.AllBuffered, PlayerColor.r, PlayerColor.g, PlayerColor.b);
		P.networkView.RPC("SetupPlayer", RPCMode.AllBuffered, PlayerName);
		networkView.RPC("AddPlayerObj", RPCMode.AllBuffered, pName, NetPlayer);
	}
	public bool isOrange = false;
	public void OnPlayerConnected(NetworkPlayer player ){
		Debug.Log("Player " +  player);
//		int BTIndex = System.Array.FindIndex(BlueTeam, predFindFirstNull);
//		int OTIndex = System.Array.FindIndex(OrangeTeam, predFindFirstNull);
//		int SIndex = System.Array.FindIndex(Spectators, predFindFirstNull);
//
//		if(BTIndex <= OTIndex && BTIndex != -1){
//			BlueTeam[BTIndex] = player; 
//			PlayerTeams.Add (player, TEAMS.BLUE);
//		}else if(BlueTeam.Length > OrangeTeam.Length && OTIndex != -1){
//			OrangeTeam[OTIndex] = player;
//			PlayerTeams.Add (player, TEAMS.ORANGE);
//		}else if(SIndex != -1){
//			Spectators[SIndex] = player;
//			PlayerTeams.Add (player, TEAMS.SPECTATOR);
//		}else{
//			Network.CloseConnection(player, true);
//		}
		if(isOrange)PlayerTeams.Add (player, TEAMS.ORANGE);
		else PlayerTeams.Add (player, TEAMS.BLUE);
	}
	public void OnServerInitialized(){
		OnPlayerConnected(Network.player);
		GetComponent<LevelLoader>().OnPlayerConnected(Network.player);
	}
	
	public bool predFindFirstNull(NetworkPlayer Obj){
		if(Obj == new NetworkPlayer())
			return true;
		else
			return false;
	}
	[RPC]
	public void SpawnMinion(string MinionName, int Team){
		GameObject Minion = GameObject.Find(MinionName);
		//Minion.networkView.viewID = viewID;
		(Minion.GetComponent(typeof(UnitBase)) as UnitBase).Team = Team;
	}
	
	[RPC]
	public void ClientRemovial(string objName){
		Destroy(GameObject.Find(objName));
		
		networkView.RPC("ClientFinished", RPCMode.Server, objName);
	}
	//clientsFinished = 0;
	/*[RPC]
	public void ClientFinished(string objName){
		if(!clientsFinished){
			clientsFinished.Add(objName, 1);
		}else{
			clientsFinished[objName] = parseInt(clientsFinished[objName].ToString()) + 1;
		}
		if(parseInt(clientsFinished[objName].ToString()) >= Network.connections.Length){
			//Network.RemoveRPCs(viewID);
			Destroy(this.gameObject);
			clientsFinished[objName] = 0;
		}
	}*/

}
