using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace SuddenlyEntertainment{

	public class MainManager{

		public static GameObject GM;
		public static List<NetworkPlayer> uninitializedPlayers;
		public static Dictionary<string, ClientSetupInfo> PlayerDict;
		public static Dictionary<string, NetworkPlayer> GUIDDict;

		public static List<NetworkPlayer> LoadedClients;

		public static bool GameInitialized;

		static MainManager(){
			PlayerDict = new Dictionary<string, ClientSetupInfo>();
			GUIDDict = new Dictionary<string, NetworkPlayer>();
			uninitializedPlayers = new List<NetworkPlayer>();

			LoadedClients = new List<NetworkPlayer>();

			GameInitialized = false;
		}
	}
}