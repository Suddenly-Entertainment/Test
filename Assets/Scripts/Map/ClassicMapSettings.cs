using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClassicMapSettings : MonoBehaviour {

	public GameObject Map = null;

	public Transform oMap = null;
	public Transform Ground = null;
	public Transform MapWalls = null;
	public Transform MapBoundries = null;


	public Terrain terrainScript;

	public Transform[] childTs;

	public Transform[] Boundries;
	public Vector3[] boundsPos = new Vector3[5];
	public Vector3[] boundsScale = new Vector3[5];

	public Color[] TeamColors;

	public GameObject[] BlueTeamSpawns;
	public GameObject[] OrangeTeamSpawns;
	public GameObject[] OtherTeamSpawns;
	/*public Color BlueTeamPlayerColor;
	public Color OrangeTeamPlayerColor;
	public Color NeutralTeamPlayerColor;
	public Color OtherTeamPlayerColor;*/
	
	public float Width = 2000;
	public float Height = 600;
	public float boundaryHeight = 5;
	public float boundaryThinkness = 0.25f;
	public float Length = 2000;

	public bool useDiffBaseSizes = false; //Do you want to use different sizes for each base?
	public float baseWidth = 100;
	public float baseLength = 100;
	public float baseHeight = 100;
	public float baseWallHeight = 5;

	public bool madeChange = false;
	public bool hasCreatedMap = false;


	// Use this for initialization
	void Start () {

	}
	public void CreateMap(){
		if(GameObject.Find("Map") == null){
			Object M = Network.Instantiate(Map, Vector3.zero, Quaternion.identity, 0);
			M.name = "Map";
		}

		if(Network.isServer){
			if(Map == null){
				Debug.LogError ("No map!!!");
				Network.Disconnect();
				Application.Quit();
				//Map = GameObject.Find("Map");
			}else{
				oMap = GameObject.Find ("Map").transform;
				Ground = oMap.Find ("Ground");
				MapWalls = oMap.Find ("Map Walls");
				MapBoundries = MapWalls.Find("Map Boundries");

				terrainScript = Ground.GetComponent<Terrain>();

				childTs = new Transform[Map.transform.childCount];


				if(boundsPos.Length > 0){
					/*float bPH = boundaryHeight/2;
					float BoundsThickPos = boundaryThinkness/2;
					for(int i = 0; i < MapBoundries.childCount; i++){
						Transform B = MB.GetChild(i);
						Vector3 Position = new Vector3(Width/2, bPH, -BoundsThickPos);
						Vector3 Scale = new Vector3(Width, boundaryHeight, boundaryThinkness);

						switch(i){
						case 0:
							//This one is the default values;
							break;
						case 1:
							//Position = new Vector3(-BoundsThickPos, bPH,  Length/2);
							Position.x = Position.z;
							Position.z = Length/2;

							Scale.x = Scale.z;
							Scale.z = Length;
							//B.localScale = new Vector3(boundaryThinkness, boundaryHeight, Length);
							break;
						case 2:
							break;
						case 3:
							B.localPosition = new Vector3(Width+BoundsThickPos, bPH, Length/2);
							B.localScale = new Vector3(boundaryThinkness, boundaryHeight, Length);
							Position.x
								Position.z
							break;
						case 4:
							B.localPosition = new Vector3(Width/2, bPH, Length+BoundsThickPos);
							B.localScale = new Vector3(Width, boundaryHeight, boundaryThinkness);
							break;
						}

						B.localPosition = Position;
						B.localScale = Scale;
					}*/
					for(int i = 0; i < Mathf.Min (MapBoundries.childCount, boundsPos.Length); i++){
						Debug.Log(i);
						MapBoundries.GetChild(i).localPosition = boundsPos[i];
					}
				}
				if(boundsScale.Length > 0){
					for(int i = 0; i < Mathf.Min (MapBoundries.childCount, boundsScale.Length); i++){
						MapBoundries.GetChild(i).localScale = boundsScale[i];
					}
				}

				for(int i2 = 0; i2 < Map.transform.childCount; i2++){
					childTs[i2] = Map.transform.GetChild(i2);
				}
				BlueTeamSpawns = GameObject.FindGameObjectsWithTag ("Blue Respawn");
				OrangeTeamSpawns = GameObject.FindGameObjectsWithTag ("Orange Respawn");
				//Network.Instantiate(Map, Vector3.zero, Quaternion.identity, 0);
				terrainScript.terrainData.size = new Vector3(Width, Height, Length);
				terrainScript.Flush();
			}
		}

		hasCreatedMap = true;
		AstarPath.active.Scan ();
	}
	// Update is called once per frame
	void Update () {
		if(hasCreatedMap && Network.isServer && madeChange){
			CreateMap ();
			madeChange = false;
		}
	}

	//SpawnPlayer(GameManager, out pName, out PlayerName, out PlayerColor, NetPlayer);
	public GameObject SpawnPlayer(GameManager GM, GameObject PlayerObj, out string pName, out Color PlayerColor, string PlayerName, NetworkPlayer NetPlayer){
		pName = "";
		//PlayerName = "";
		//PlayerColor = new Color(0,0,0,0);

		TEAMS PlayerTeam = GM.PlayerTeams[NetPlayer];
		int TeamColorIndex = ((int)PlayerTeam) + 1; //This is because the array is zero indexed, while TEAMS is -1 indexed.
		PlayerColor = TeamColors[TeamColorIndex];


		Vector3 SpawnPos = FindSpawnPos(PlayerTeam);
		GameObject PlayerO = (Network.Instantiate (PlayerObj, SpawnPos, Quaternion.identity, 2) as GameObject);
		//PlayerColor = GM.PlayerTeams[NetPlayer] == TEAMS.BLUE ? BlueTeamPlayerColor : GM.PlayerTeams[NetPlayer] == TEAMS.ORANGE ?
		pName = PlayerO.name;
		return PlayerO;
	}


	public Vector3 FindSpawnPos(TEAMS playerTeam){
		Vector3 SpawnPos = new Vector3(Width/2, 2.5f, Length/2);
		if(playerTeam == TEAMS.BLUE){
			foreach(GameObject Obj in BlueTeamSpawns){
				if(!Obj.GetComponent<RespawnScript>().isTriggered){
					SpawnPos = Obj.transform.position;
				}
			}
		}else if(playerTeam == TEAMS.ORANGE){
			foreach(GameObject Obj in OrangeTeamSpawns){
				if(!Obj.GetComponent<RespawnScript>().isTriggered){
					SpawnPos = Obj.transform.position;
				}
			}
		}
		return SpawnPos;
	}

	public float GetDeathTime(int Level){
		//TODO: Implement this function.
		Debug.LogWarning ("GetDeathTime is not fully implemented yet!  It currently just returns 10.");
		return 10; 
	}
}
