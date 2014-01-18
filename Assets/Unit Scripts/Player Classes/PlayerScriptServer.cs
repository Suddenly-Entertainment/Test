using UnityEngine;
using System.Collections;

namespace SuddenlyEntertainment{

	public class PlayerScriptServer : MonoBehaviour {

		public PlayerScriptClient PSC;
		public Inventory Inv;

		public event System.EventHandler<OnUnitDeathEventArgs> onUnitDieNearby;


		public float spawnTime = 0;
		// Use this for initialization
		void Start () {
			PSC = GetComponent<PlayerScriptClient>();
			Inv = GetComponent<Inventory>();
			Inv.OnBuy += new System.EventHandler(RecalculateStats);
			PSC.Stats.onDeath += new System.EventHandler<OnUnitDeathEventArgs>(OnDeath);
			PSC.onSpawn += new System.EventHandler(OnSpawn);
			StartCoroutine("Generation");
		}
	
		// Update is called once per frame
		void Update () {
			if(PSC.isDead){
				if(Time.time >= spawnTime){
					PSC.CallOnSpawn();
				}
			}else{
			
			}
		}

		public void OnDeath(object sender, OnUnitDeathEventArgs e){
			PSC.isDead = true;
			spawnTime = Time.time + 10;
			var Args = new OnUnitDeathEventArgs();

			Args.Level = PSC.Stats.Level;
			Args.expierenceGained = CalcExpGivenOnDeath(PSC.Stats.Level);//PSC.Stats.ExpierenceOnDeath;
			Args.goldGained = PSC.Stats.GoldOnDeath;
			Args.UnitName = MainManager.PlayerDict[PSC.OwnerClient].Nickname;

			var Colliders = Physics.OverlapSphere(transform.position, 20);
			foreach(var col in Colliders){
				if(col.tag == "Player"){
					col.GetComponent<PlayerScriptServer>().UnitDiedNearby(this, Args);
				}
			}
		}
		public float CalcExpGivenOnDeath(int Level){
			return PSC.Stats.ExpierenceCurve[Level] * 0.75f;
		}

		public void OnSpawn(object sender, System.EventArgs e){
			PSC.Stats.CurrentHealth = PSC.Stats.MaxHealth;
			PSC.isDead = false;
			spawnTime = 0;
			transform.position = new Vector3(0,1,0);
		}

		public void UnitDiedNearby(object Obj, OnUnitDeathEventArgs e){
			PSC.B_Stats.Expierence += e.expierenceGained;
			PSC.B_Stats.Gold += e.goldGained;
			RecalculateStats(this, System.EventArgs.Empty);
			if(onUnitDieNearby != null)
				onUnitDieNearby(Obj, e);
		}

		public void Attacked ( Damage damage )
		{
			float TotalDamage = 0;
			float armorAfterPen, specialResistAfterPen;
			float PhysicalDamage = 0;
			float SpecialDamage = 0;
			if(damage.PhysicalDamage != 0) {
				armorAfterPen = (float)PSC.Stats.Armor;

				armorAfterPen -= armorAfterPen * damage.ArmorPenPercent;
				armorAfterPen -= damage.ArmorPenFlat;
				if(armorAfterPen < 0)
					armorAfterPen = 0;
				PhysicalDamage = PhysicalDamageAfterArmor (armorAfterPen, damage.PhysicalDamage);
			}
			if(damage.SpecialDamage != 0){
				specialResistAfterPen = (float)PSC.Stats.SpecialResist;

				specialResistAfterPen -= specialResistAfterPen * damage.SpecialResistPenPercent;
				specialResistAfterPen -= damage.SpecialResistPenFlat;
				if(specialResistAfterPen < 0)
					specialResistAfterPen = 0;

				SpecialDamage = PhysicalDamageAfterArmor(specialResistAfterPen, damage.SpecialDamage);
			}

			TotalDamage = PhysicalDamage + SpecialDamage + damage.TrueDamage;
			if(PSC.Stats.CurrentHealth - TotalDamage <= 0){
				PSC.Killer = damage.Attacker;
			}
			PSC.Stats.CurrentHealth -= TotalDamage;
		}

		public float PhysicalDamageAfterArmor(float Armor, float damage){
			//Damage multiplier = 100 / (100 + Armor) if Armor ≥ 0
			//Damage multiplier = 2 − [100 / (100 − Armor)] if Armor ≤ 0
			float damageMultiplier;
			if(Armor >= 0){
				damageMultiplier = 100 / (100 + Armor);
			}else{
				damageMultiplier = 2 - (100 / 100 - Armor);
			}

			return damage*damageMultiplier;
		}
		/*public void Kill(){
			isDead = true;
			spawnTime = Time.time + 10;
			networkView.RPC ("Freeze", RPCMode.Others, PSC.OwnerClient);
			networkView.RPC ("SpectateOther", MainManager.GUIDDict[PSC.OwnerClient]);
		}

		public void Spawn(){
			isDead = true;
			networkView.RPC ("Unfreeze", RPCMode.All, PSC.OwnerClient);
			networkView.RPC ("SpectateSelf", MainManager.GUIDDict[PSC.OwnerClient]);
		}*/


		public void RecalculateStats(object sender, System.EventArgs e){
			UnitStats ItemStatChanges = Inv.GetTotalStatChange();

			PSC.Stats = UnitStats.Add(PSC.Stats, ItemStatChanges);
			//SendStatsToClient(PSC.Stats);
		}

		private IEnumerator Generation(){

			while(true){
				UnitStats PStats = PSC.Stats;
				if(!PSC.isDead){
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