using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using UnityEngine;

namespace SuddenlyEntertainment
{
	public static class PropertyLoader
	{
		public static string SerializeObject<T>(object pObject) 
   		{ 
	      string XmlizedString = null; 
	      
			MemoryStream memoryStream = new MemoryStream(); 
			XmlSerializer xs = new XmlSerializer(typeof(T)); 
			XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
	      	xs.Serialize(xmlTextWriter, pObject); 
	      	memoryStream = (MemoryStream)xmlTextWriter.BaseStream; 
	      	XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray()); 
	      	return XmlizedString; 
   		}
		   public static string UTF8ByteArrayToString(byte[] characters) 
		   {      
		      UTF8Encoding encoding = new UTF8Encoding(); 
		      string constructedString = encoding.GetString(characters); 
		      return (constructedString); 
		   } 
		   public static void CreateXML(string _data, string FilePath) 
		   { 
		      StreamWriter writer; 
		      FileInfo t = new FileInfo(FilePath); 
		      if(!t.Exists) 
		      { 
		         writer = t.CreateText(); 
		      } 
		      else 
		      { 
		         t.Delete(); 
		         writer = t.CreateText(); 
		      } 
		      writer.Write(_data); 
		      writer.Close(); 
			Debug.Log("File written: "+ FilePath); 
		   }

		  public static byte[] StringToUTF8ByteArray(string pXmlString) 
		   { 
		      UTF8Encoding encoding = new UTF8Encoding(); 
		      byte[] byteArray = encoding.GetBytes(pXmlString); 
		      return byteArray; 
		   }

		  public static t DeserializeObject<t>(string pXmlizedString) 
		   { 
		      XmlSerializer xs = new XmlSerializer(typeof(t)); 
		      MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString)); 
		      XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
			return (t)(xs.Deserialize(memoryStream)); 
		   } 

		  public static string LoadXML(string FilePath) 
		   { 
			StreamReader r = File.OpenText(FilePath);
			string _info = r.ReadToEnd(); 
		    r.Close(); 
		      
		    Debug.Log("File Read: "+ FilePath); 
			  return _info; 
		   } 
	}
}

