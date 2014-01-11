using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace SuddenlyEntertainment{

	public class MainManager{

		public static GameObject GM;
		public static List<NetworkPlayer> uninitializedPlayers;
		public static Dictionary<NetworkPlayer, ClientSetupInfo> PlayerDict;

		public static List<NetworkPlayer> LoadedClients;

		public static bool GameInitialized;

		static MainManager(){
			PlayerDict = new Dictionary<NetworkPlayer, ClientSetupInfo>();
			uninitializedPlayers = new List<NetworkPlayer>();

			LoadedClients = new List<NetworkPlayer>();

			GameInitialized = false;
		}
	}
}