using UnityEngine;
using System;
using System.Reflection;
using System.Collections;

[RequireComponent (typeof (NetworkView))]

public class LevelLoader : MonoBehaviour {
	public GameManager GM;	
	public string[] supportedNetworkLevels;
	public string disconnectedLevel;
	private int lastLevelPrefix ;

	
	public int PlayerCount;
	public GameObject[] playerObjs;
	public string PlayerName;
	
	public string networkID;

	public void initVarsToDefaultValue(){
		supportedNetworkLevels = new string[2];
		supportedNetworkLevels[0] = "TownHub";
		supportedNetworkLevels[1] = "ClassicMap";

		disconnectedLevel = "SceneMainMenu";
		lastLevelPrefix = 0;
		PlayerCount = 0;
		playerObjs = new GameObject[30];
		networkID = "";
	}

	public void Awake ()
	{
		initVarsToDefaultValue();
	    // Network level loading is done in a separate channel.
	    DontDestroyOnLoad(this);
	    networkView.group = 1;
	    //Application.LoadLevel(disconnectedLevel);
	    
		Type t = typeof(Terrain);
		foreach(MethodInfo mi in t.GetMethods()) {
	    
			        System.String s = System.String.Format("{0} {1} (", mi.ReturnType, mi.Name);
			        ParameterInfo[] pars = mi.GetParameters();
	    
			        for (int j = 0; j < pars.Length; j++) {
	            s = String.Concat(s, String.Format("{0}{1}", pars[j].ParameterType,
	                (j == pars.Length-1) ? "" : ", "));
	        }
	        s = String.Concat(s, ")");
	        Debug.Log(s);
	    }
	    //Network.SetSendingEnabled(0, false);
	    //Network.isMessageQueueRunning = false;
	}
	
	public void OnGUI ()
	{
		
		if (Network.peerType == NetworkPeerType.Server)
		{
			GUILayout.BeginArea(new Rect(0, Screen.height - 30, Screen.width, 30)); //Starts the area for our buttons
			GUILayout.BeginHorizontal(); //Starts the fact that we are adding stuff in a horizontal row.
			
			if(GUILayout.Button("Disconnect")){
				Network.Disconnect();
				MasterServer.UnregisterHost(); //Doesn't do anything unless we are registered, so I put it in here, just in case.
			}
			
			foreach(string level in supportedNetworkLevels) //Adds a button for every row.
			{
				if (GUILayout.Button(level)) //Buttons in Unity GUI are created and used in if statements
				{
					//Hopefully we will remove all the RPCs, and go to the next level
					Network.RemoveRPCsInGroup(0); 
					Network.RemoveRPCsInGroup(1); 
					networkView.RPC( "LoadLevel", RPCMode.AllBuffered, level, lastLevelPrefix + 1);
				}
			}
			
	
			GUILayout.FlexibleSpace(); //Makes it flexible
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
			
		}else if(Network.peerType == NetworkPeerType.Client){
		
			GUILayout.BeginArea(new Rect(0, Screen.height - 30, Screen.width, 30));
			GUILayout.BeginHorizontal();
			
			if(GUILayout.Button("Disconnect")){
				Network.Disconnect();
			}
			networkID = GUILayout.TextField(networkID);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
	
		}
	}
	
	[RPC]
	public void LoadLevel (string level, int levelPrefix)
	{
		Debug.Log (level);
		lastLevelPrefix = levelPrefix;
			// There is no reason to send any more data over the network on the default channel,
			// because we are about to load the level, thus all those objects will get deleted anyway
			Network.SetSendingEnabled(0, false);	
	
			// We need to stop receiving because first the level must be loaded first.
			// Once the level is loaded, rpc's and other state update attached to objects in the level are allowed to fire
			Network.isMessageQueueRunning = false;
			playerObjs = new GameObject[30];
	
			// All network views loaded from a level will get a prefix into their NetworkViewID.
			// This will prevent old updates from clients leaking into a newly created scene.
			Network.SetLevelPrefix(levelPrefix);
			Application.LoadLevel(level);

		StartCoroutine(FinishLoading(level));

	}

	public IEnumerator FinishLoading(string level){
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();

		Debug.Log ("Entered");
		// Allow receiving data again
		Network.isMessageQueueRunning = true;
		// Now the level has been loaded and we can start sending out data to clients
		Network.SetSendingEnabled(0, true);
		GM.onNetworkLoadedLevel(level, PlayerName);
		
		foreach(GameObject go in FindObjectsOfType(typeof(GameObject)))
			go.SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);	
	}
	
	public void OnDisconnectedFromServer ()
	{
		Application.LoadLevel(disconnectedLevel);
	}


	public void OnPlayerConnected(NetworkPlayer player ){
		Debug.Log("Player " +  player);
		PlayerCount++;
	}
	public void OnPlayerDisconnected(NetworkPlayer player){
			Debug.Log("Clean up after player " +  player);
			PlayerCount--;
			Network.RemoveRPCs(player);
			Network.DestroyPlayerObjects(player);
	}
	[RPC]
	public void AddPlayerObj(string Pname, NetworkPlayer player){
		GameObject PlayerObject = GameObject.Find(Pname);
		if(PlayerObject == null){
			PlayerObject = GameObject.Find("Player "+player.guid);
			if(PlayerObject == null){
				Debug.LogError("We srs cannot find this player we tried (" + Pname + ") And (Player " + player.guid + ").");
				Network.Disconnect();
			}
		}
		PlayerObject.name = "Player "+player.guid;
		playerObjs[PlayerCount-1] = PlayerObject;
		Player PC = (PlayerObject.GetComponent(typeof(Player)) as Player);
		//Debug.LogError(PC);
		PC.PlayerNum = PlayerCount;
		PC.NetPlayer = player;
		PC.Team = PlayerCount % 2 == 0 ? 2 : 1;
		
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
