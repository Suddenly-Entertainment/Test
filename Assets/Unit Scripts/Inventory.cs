using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace SuddenlyEntertainment{
	public class Inventory : MonoBehaviour {
		public event System.EventHandler OnBuy;

		Item[] Inv;
		public int ItemCount;
		public int InvSize;

		public bool ShopScreen = false;
		// Use this for initialization
		void Start () {
			ItemCount = 0;
			InvSize = 6;
			Inv = new Item[InvSize];
		}
		
		// Update is called once per frame
		void Update () {
			if(GetComponent<PlayerScriptClient>().OwnerClient == Network.player.guid){
				if(Input.GetAxis("ShopButton") == 1){
					ShopScreen = !ShopScreen;			
				}
			}
		}

		void OnGUI(){
			if(ShopScreen){
				GUI.Box (new Rect(0,0,300,300), "Shop");
				GUILayout.BeginArea(new Rect(0,0,300,300));
				GUILayout.BeginVertical();
				GUILayout.Label("Buy: ");
				GUILayout.BeginHorizontal();

				foreach(KeyValuePair<string, ItemProperties> item in XMLFileManager.Items){
					if(GUILayout.Button(new GUIContent(item.Value.Name + ": "+ item.Value.Cost+"g", item.Value.StatChanges.GetString()))){
						networkView.RPC ("RequestPurchase", RPCMode.Server, item.Value.Name);
					}
				}

				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.Label("Item details: \n"+ GUI.tooltip);
				GUILayout.Label("Your items: ");
				GUILayout.BeginHorizontal();

				foreach(Item item in Inv){
					if(item == null)continue;
					GUILayout.Label(new GUIContent(item.Name, item.Properties.StatChanges.GetString()));

				}

				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.FlexibleSpace();
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.EndArea();
			}
		}

		public void AddItem(string ItemName){
			if(ItemCount != InvSize && XMLFileManager.Items.ContainsKey(ItemName)){
				Inv[ItemCount] = new Item(XMLFileManager.Items[ItemName]);
			}
			OnBuy(this, new System.EventArgs());
			networkView.RPC ("SuccessfulPurchase", RPCMode.Others, ItemName, ItemCount);
			ItemCount++;
		}

		[RPC]
		public void RequestPurchase(string itemName){
			AddItem(itemName);
		}

		[RPC]
		public void SuccessfulPurchase(string ItemName, int ItemCount){
			if(ItemCount != InvSize && XMLFileManager.Items.ContainsKey(ItemName)){
				Inv[ItemCount] = new Item(XMLFileManager.Items[ItemName]);
			}
			ItemCount++;

		}

		public UnitStats GetTotalStatChange(){
			UnitStats Result = new UnitStats();

			foreach(Item item in Inv){
				if(item == null)continue;
				Result += item.Properties.StatChanges;
			}

			return Result;
		}
	}
}
