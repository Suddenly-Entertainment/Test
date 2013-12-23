#pragma strict
var ip: String = "76.84.167.144";
var port: String = "27015";
var netLoadobj: GameObject;

function Start () {

}

function Update () {

}

function OnGUI(){
	GUI.Box(Rect(10,10,150,150), "Main Menu");
	
	GUI.Label(Rect(15,100,30,20), "IP:");
	ip = GUI.TextField(Rect(46,100,84,20), ip);
	
	GUI.Label(Rect(15,130,30,20), "Port:");
	port = GUI.TextField(Rect(46,130,84,20), port);
	
	if(GUI.Button(Rect(20,40,80,20), "Host Server")){
		Network.InitializeServer(32, parseInt(port), false);
		//Instantiate(netLoadobj);
	}
	
	if(GUI.Button(Rect(20,70,80,20), "Join Server")){
		Network.Connect(ip, parseInt(port));
	}

}