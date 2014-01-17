using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
namespace SuddenlyEntertainment{

	public static class MainManager{

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

		public static string Compress(string s)
		{
		    var bytes = Encoding.Unicode.GetBytes(s);
		    using (var msi = new MemoryStream(bytes))
		    using (var mso = new MemoryStream())
		    {
		        using (var gs = new GZipStream(mso, CompressionMode.Compress))
		        {
		            msi.CopyTo(gs);
		        }
		        return Convert.ToBase64String(mso.ToArray());
		    }
		}
		 
		public static string Decompress(string s)
		{
		    var bytes = Convert.FromBase64String(s);
		    using (var msi = new MemoryStream(bytes))
		    using (var mso = new MemoryStream())
		    {
		        using (var gs = new GZipStream(msi, CompressionMode.Decompress))
		        {
		            gs.CopyTo(mso);
		        }
		        return Encoding.Unicode.GetString(mso.ToArray());
		    }
		}
	}
}