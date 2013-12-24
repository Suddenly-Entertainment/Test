var supportedNetworkLevels : String[] = [ "TownHub" ];
var disconnectedLevel : String = "SceneMainMenu";
private var lastLevelPrefix = 0;
var PlayerObj: GameObject;

var PlayerCount : int = 0;
var playerObjs: GameObject[] = new GameObject[30];

function Awake ()
{
    // Network level loading is done in a separate channel.
    DontDestroyOnLoad(this);
    networkView.group = 1;
    Application.LoadLevel(disconnectedLevel);
}

function OnGUI ()
{
	
	if (Network.peerType == NetworkPeerType.Server)
	{
		GUILayout.BeginArea(Rect(0, Screen.height - 30, Screen.width, 30)); //Starts the area for our buttons
		GUILayout.BeginHorizontal(); //Starts the fact that we are adding stuff in a horizontal row.
		
		if(GUILayout.Button("Disconnect")){
			Network.Disconnect();
			MasterServer.UnregisterHost(); //Doesn't do anything unless we are registered, so I put it in here, just in case.
		}
		
		for (var level in supportedNetworkLevels) //Adds a button for every row.
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
	
		GUILayout.BeginArea(Rect(0, Screen.height - 30, Screen.width, 30));
		GUILayout.BeginHorizontal();
		
		if(GUILayout.Button("Disconnect")){
			Network.Disconnect();
		}
		
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();

	}
}

@RPC
function LoadLevel (level : String, levelPrefix : int)
{
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
		yield;
		yield;

		// Allow receiving data again
		Network.isMessageQueueRunning = true;
		// Now the level has been loaded and we can start sending out data to clients
		Network.SetSendingEnabled(0, true);
		onNetworkLoadedLevel(level);

		for (var go in FindObjectsOfType(GameObject))
			go.SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);	
}

function OnDisconnectedFromServer ()
{
	Application.LoadLevel(disconnectedLevel);
}
function onNetworkLoadedLevel(level : String){
	if(level == "TownHub"){
		PlayerCount++;
		var P = Network.Instantiate(PlayerObj, Vector3(10,10,10), Quaternion.identity, 2);
		
		var R = Random.value;
		var G = Random.value;
		var B = Random.value;
		//P.tag = "Player";
		P.networkView.RPC("SetupPlayerColor", RPCMode.AllBuffered, R, G, B);
		networkView.RPC("AddPlayerObj", RPCMode.AllBuffered, P.name);
	}
}
function OnPlayerConnected(player: NetworkPlayer){
	Debug.Log("Player " +  player);
	PlayerCount++;
}
function OnPlayerDisconnected(player: NetworkPlayer){
		Debug.Log("Clean up after player " +  player);
		PlayerCount--;
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
}
@RPC
function AddPlayerObj(playerName: String){
	var PlayerObject = GameObject.Find(playerName);
	playerObjs[PlayerCount-1] = PlayerObject;
	var PC = PlayerObject.GetComponent(PlayerChecker);
	PC.Player = PlayerCount;
	PC.Team = PlayerCount % 2 == 0 ? 2 : 1;
	
}
@script RequireComponent(NetworkView)