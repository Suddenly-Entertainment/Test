using UnityEngine;
using System.Collections;

namespace SuddenlyEntertainment{

	public class PlayerScriptServer : MonoBehaviour {

		public PlayerScriptClient PSC;
		public Inventory Inv;
		// Use this for initialization
		void Start () {
			PSC = GetComponent<PlayerScriptClient>();
			Inv = GetComponent<Inventory>();
			Inv.OnBuy += new System.EventHandler(RecalculateStats);
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public void RecalculateStats(object sender, System.EventArgs e){
			UnitStats stats = PSC.Stats;
			UnitStats ItemStatChanges = Inv.GetTotalStatChange();

			PSC.Stats = stats + ItemStatChanges;
			SendStatsToClient(PSC.Stats);
		}


		public void SendStatsToClient(UnitStats stats){
			string Serial = fastJSON.JSON.Instance.ToJSON(stats);
			networkView.RPC ("RecieveStats", RPCMode.Others, Serial);
		}
	}
}