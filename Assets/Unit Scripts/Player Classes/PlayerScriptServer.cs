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
			PSC.Stats.onDeath += new System.EventHandler(OnDeath);
			PSC.onSpawn += new System.EventHandler(OnSpawn);
			StartCoroutine("Generation");
		}
	
		// Update is called once per frame
		void Update () {
			if(isDead){
				if(Time.time >= spawnTime){
					if(PSC.onSpawn != null)
						PSC.onSpawn(this);
				}
			}else{
			
			}
		}

		public void OnDeath(object sender, System.EventArgs e){
			isDead = true;
			spawnTime = Time.time + 10;
		}

		public void OnSpawn(object sender, System.EventArgs e){
			PSC.Stats.CurrentHealth = PSC.Stats.MaxHealth;
			isDead = false;
			spawnTime = 0;
		}


		public void Kill(){
			isDead = true;
			spawnTime = Time.time + 10;
			networkView.RPC ("Freeze", RPCMode.Others, PSC.OwnerClient);
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

		private IEnumerator Generation(){

			while(true){
				UnitStats PStats = PSC.Stats;
				if(!isDead){
					PStats.CurrentHealth += PStats.HealthRegeneration;
				}
				PStats.Gold += PStats.GoldGeneration;

				yield return new WaitForSeconds(1);
			}

		}

		public void SendStatsToClient(UnitStats stats){
			string Serial = fastJSON.JSON.Instance.ToJSON(stats);
			networkView.RPC ("RecieveStats", RPCMode.Others, Serial);
		}
	}
}