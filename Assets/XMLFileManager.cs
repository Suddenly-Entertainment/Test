using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SuddenlyEntertainment{
	/// <summary>
	/// XML file manager.
	/// </summary>
	public static class XMLFileManager{
		public static Dictionary<string, ItemProperties> Items = new Dictionary<string, ItemProperties>();
		public static string[] ItemXMLs;

		public static Dictionary<string, ChampionProperties> Champions = new Dictionary<string, ChampionProperties>();
		public static string[] ChampionXMLs;

		public static Dictionary<string, AbilityProperties> Abilities = new Dictionary<string, AbilityProperties>();
		public static string[] AbilityXMLs;

		public static Dictionary<string, EffectProperties> Effects = new Dictionary<string, EffectProperties>();
		public static string[] EffectXMLs;

		static XMLFileManager(){
			//Debug.Log (Directory.GetCurrentDirectory());
		}
		public static void Setup(){
			SetupItems();
			SetupChampions();
			SetupAbilities();
			SetupEffects();
		}
		public static void SetupItems(){
			ItemXMLs = Directory.GetFiles("Assets\\XML\\Items", "*.xml");
			foreach(string filePath in ItemXMLs){
				string XML = PropertyLoader.LoadXML(filePath);
				ItemProperties item = PropertyLoader.DeserializeObject<ItemProperties>(XML);
				Items.Add(item.Name, item);
			}
		}
		public static void SetupChampions(){
			ChampionXMLs = Directory.GetFiles("Assets\\XML\\Champions", "*.xml");
			foreach(string filePath in ChampionXMLs){
				string XML = PropertyLoader.LoadXML(filePath);
				ChampionProperties item = PropertyLoader.DeserializeObject<ChampionProperties>(XML);
				Champions.Add(item.Name, item);
			}
		}
		public static void SetupAbilities(){
			AbilityXMLs = Directory.GetFiles("Assets\\XML\\Abilities", "*.xml");
			foreach(string filePath in AbilityXMLs){
				string XML = PropertyLoader.LoadXML(filePath);
				AbilityProperties item = PropertyLoader.DeserializeObject<AbilityProperties>(XML);
				Abilities.Add(item.Name, item);
			}
		}
		public static void SetupEffects(){
			EffectXMLs = Directory.GetFiles("Assets\\XML\\Effects", "*.xml");
			foreach(string filePath in EffectXMLs){
				string XML = PropertyLoader.LoadXML(filePath);
				EffectProperties item = PropertyLoader.DeserializeObject<EffectProperties>(XML);
				Effects.Add(item.Name, item);
			}
		}
		public static void ExportItem(Item item, string fileName){
			string xMl = PropertyLoader.SerializeObject<ItemProperties>(item.properties);
			PropertyLoader.CreateXML(xMl, "Assets\\XML\\Items\\"+fileName);
		}

		public static void ExportChampion(ChampionProperties champ, string fileName){
			string xMl = PropertyLoader.SerializeObject<ChampionProperties>(champ);
			PropertyLoader.CreateXML(xMl, "Assets\\XML\\Items\\"+fileName);
		}
	}
}