using UnityEngine;
using System.Collections;

namespace SuddenlyEntertainment{

	public class PlayerScriptServer : MonoBehaviour {

		public PlayerScriptClient PSC;
		public Inventory Inv;

		bool isDead = false;
		float spawnTime = 0;
		// Use this for initialization
		void Start () {
			PSC = GetComponent<PlayerScriptClient>();
			Inv = GetComponent<Inventory>();
			Inv.OnBuy += new System.EventHandler(RecalculateStats);
		}
		
		// Update is called once per frame
		void Update () {
			if(PSC.health < 0 && !isDead){
				Kill();
			}else if(isDead){
				if(Time.time >= spawnTime){
					Spawn();
				}
			}
		}
		public void Kill(){
			isDead = true;
			spawnTime = Time.time + 10;
			networkView.RPC ("Freeze", RPCMode.All, PSC.OwnerClient);
			networkView.RPC ("SpectateOther", MainManager.GUIDDict[PSC.OwnerClient]);
		}

		public void Spawn(){
			isDead = true;
			networkView.RPC ("Unfreeze", RPCMode.All, PSC.OwnerClient);
			networkView.RPC ("SpectateSelf", MainManager.GUIDDict[PSC.OwnerClient]);
		}


		public void RecalculateStats(object sender, System.EventArgs e){
			UnitStats stats = PSC.B_Stats;
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