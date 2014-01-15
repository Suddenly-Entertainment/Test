using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SuddenlyEntertainment{
	public static class XMLFileManager{
		public static Dictionary<string, ItemProperties> Items = new Dictionary<string, ItemProperties>();
		public static string[] ItemXMLs;
		static XMLFileManager(){
			//Debug.Log (Directory.GetCurrentDirectory());
		}

		public static void SetupItems(){
			ItemXMLs = Directory.GetFiles("Assets\\XML\\Items", "*.xml");
			foreach(string filePath in ItemXMLs){
				string XML = PropertyLoader.LoadXML(filePath);
				ItemProperties item = PropertyLoader.DeserializeObject<ItemProperties>(XML);
				Items.Add(item.Name, item);
			}
		}

		public static void ExportItem(Item item, string fileName){
			string xMl = PropertyLoader.SerializeObject<ItemProperties>(item.Properties);
			PropertyLoader.CreateXML(xMl, "Assets\\XML\\Items\\"+fileName);
		}
	}
}