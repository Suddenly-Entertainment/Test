using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;

namespace SuddenlyEntertainment{
	[System.Serializable]
	public class ClientSetupInfo : INetSerializable{
		public string Nickname
		{
			get;
			set;
		}

		public Teams Team
		{
			get;
			set;
		}

		public string Champion
		{
			get;
			set;
		}

		public GameObject PlayerObj
		{
			get;
			set;
		}

		public ClientSetupInfo(){
			Nickname = "Bow Wacker";
			Team = Teams.NA;
		}

		public ClientSetupInfo(string NickName){
			Nickname = NickName;
			Team = Teams.NA;
		}

		public ClientSetupInfo(Teams clientTeam){
			Nickname = "Bow Wacker";
			Team = clientTeam;
		}

		public ClientSetupInfo(int clientTeam){
			Nickname = "Bow Wacker";
			Team = (Teams)clientTeam;
		}

		public ClientSetupInfo(string NickName, Teams clientTeam){
			Nickname = NickName;
			Team = clientTeam;
		}

		public ClientSetupInfo(string NickName, int clientTeam){
			Nickname = NickName;
			Team = (Teams)clientTeam;
		}
		public void Deserialize(string Serial){
			PropertyInfo[] props = this.GetType().GetProperties();
			Dictionary<string, PropertyInfo> propsDict = new Dictionary<string, PropertyInfo>();
			foreach(PropertyInfo prop in props){
				propsDict.Add(prop.Name, prop);
			}

			string[] nameAndData = Serial.Split('!');
			nameAndData[1] = nameAndData[1].Replace("(","").Replace(")","");

			string[] propertiesSerial = nameAndData[1].Split ('|');
			foreach(string property in propertiesSerial){
				string[] DAV = property.Split('='); //Declaration and value
				string[] TAN = DAV[0].Split();//Type and Name
				IConvertible value = DAV[1];
				//.GetFormat(Type.GetType(TAN[0]))
				propsDict[TAN[1]].GetSetMethod().Invoke(this, new object[]{value.ToType(Type.GetType(TAN[0]), CultureInfo.CurrentCulture)}); //SetValue(value.ToType(Type.GetType(TAN[0]), CultureInfo.CurrentCulture));
			}

		}

		public string Serialize(){
			PropertyInfo[] props = this.GetType().GetProperties();
			string Serial = "[ClientSetupInfo]!(";
			foreach(PropertyInfo prop in props){
				if(prop.GetType().IsValueType){
					Serial += string.Format("{1} {2}={3}|", prop.GetType().Name, prop.Name, prop.GetGetMethod().Invoke(this, null).ToString());
				}else{
					Type[] Interfaces = prop.GetType().GetInterfaces();
					bool HasINetSerializable = false;
					foreach(Type Interface in Interfaces){
						if(Interface.Name == "INetSerializable"){
							HasINetSerializable = true;
							break;
						}
					}
					if(HasINetSerializable){
						//Note:  This is perfectly safe, as we have already checked to see if this object has 
						//unsafe{
						Serial += (prop.GetGetMethod().Invoke(this, null) as INetSerializable).Serialize();
						//}
					}
				}
			}
			//We got to remove the extra | at the end
			Serial = Serial.Remove(Serial.Length-1);

			//Now we got to close up this Serial with a )
			Serial += ")";

			return Serial;

		}
		public Dictionary<string, object> NetPrep(){
			Dictionary<string, object> Result = new Dictionary<string, object>();

			Result.Add("Nickname", Nickname);
			int intTeam = (int)Team;
			Result.Add("Team", intTeam);

			return Result;
		}
	}
}