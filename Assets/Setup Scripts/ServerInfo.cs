using UnityEngine;
using System.Collections;

namespace SuddenlyEntertainment{
	/// <summary>
	/// This class is for storing information about the server both client and server side.
	/// </summary>
	public class ServerInfo {
		public static string IP
		{
			get;
			set;
		}

		public static int Port
		{
			get;
			set;
		}

		static ServerInfo(){
			IP = "76.84.167.144";
			Port = 27015;
		}
	}
}