using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuddenlyEntertainment
{
	public delegate void onLevels( object sender, int Level );
	/// <summary>
	/// Stores Stats for Units.
	/// </summary>
	[System.Serializable]
	public class UnitStats
	{
		public event onLevels onLevel;
		public event EventHandler<OnUnitDeathEventArgs> onDeath;
		public Stat _Level = new Stat("Unit Level");
		public int _maxLevel;
		public Stat _moveSpeed = new Stat("Movement Speed");
		public Stat _attackDamage = new Stat("Basic Attack Damage");
		public Stat _attackSpeed = new Stat("Basic Attack Speed");
		public Stat _attackRange = new Stat("Basic Attack Range");

		public Stat _maxHealth = new Stat("Maximum Health");
		public Stat _healthRegeneration = new Stat("Health Regeneration");
		public float _currentHealth;

		public Stat _Armor = new Stat("Armor");
		public Stat _specialResist = new Stat("Special Resist");

		public Stat _armorPenFlat = new Stat("Armor Penetration Flat");
		public Stat _armorPenPercent = new Stat("Armor Penetration Percent");

		public Stat _specialResistPenFlat = new Stat("Special Resist Penetration Flat");
		public Stat _specialResistPenPercent = new Stat("Special Resist Penetration Percent");

		public float _Expierence;
		public float _totalExpierence;
		public  float[] ExpierenceCurve;
		public Stat _expierenceOnDeath = new Stat("Expierence Granted On Death");


		public Stat _goldOnDeath = new Stat("Gold Granted On Death");


		public float _Gold;

		public Stat _goldGeneration = new Stat("Gold Generation");

		public int Level {
			get {
				return (int)_Level.GetCurrent();
			}
			set {
				if(value <= _maxLevel - 1)
				_Level.Bonus = (float)value;

				if(onLevel != null)
					onLevel(this, Level);
			}
		}


		public float MoveSpeed {
			get {
				return _moveSpeed.GetCurrent ();
			}
			set {
				_moveSpeed.Bonus = value;
			}
		}

		public float B_MoveSpeed {
			get {
				return _moveSpeed.Base;
			}
			set {
				_moveSpeed.Base = value;
			}
		}


		public float AttackDamage {
			get {
				return _attackDamage.GetCurrent(Level);
			}
			set {
				_attackDamage.Bonus = value;
			}
		}

		public float AttackSpeed {
			get {
				return _attackSpeed.GetCurrent (Level);
			}
			set {
				_attackSpeed.SetBonusToPercentOfBase(value);
			}
		}

		public float AttackRange {
			get {
				return _attackRange.GetCurrent (Level);
			}
			set {
				_attackRange.Bonus = value;
			}
		}

		public float MaxHealth {
			get{
				return _maxHealth.GetCurrent(Level);
			}
			set{
				_maxHealth.Bonus = value;
			}
		}

		public float CurrentHealth {
			get{ return _currentHealth;}
			set{
				if(value <= 0) {
					_currentHealth = 0;
					if(onDeath != null)
						onDeath(this, new OnUnitDeathEventArgs());
				}else if(value > MaxHealth) {
					_currentHealth = MaxHealth;
				} else {
					_currentHealth = value;
				}
			}
		}

		public float Expierence {
			get {
				return _Expierence;
			}
			set {
				if(value <= 0)return;
				_Expierence = CheckAndSetLevel(value);
			}
		}

		public float GoldGeneration {
			get{
				return _goldGeneration.GetCurrent(Level);
			}
			set{
				_goldGeneration.Bonus = value;
			}
		}

		public float GoldOnDeath {
			get {
				return _goldOnDeath.GetCurrent (Level);
			}
			set {
				_goldOnDeath.Bonus = value;
			}
		}

		public float ExpierenceOnDeath {
			get {
				return _expierenceOnDeath.GetCurrent (Level);
			}
			set {
				_expierenceOnDeath.Bonus = value;
			}
		}

		public float Gold{
			get{
				return _Gold;
			}
			set {
				if(value < 0) {
					return;
				}
				_Gold = value;
			}
		}

		public float HealthRegeneration {
			get {
				return _healthRegeneration.GetCurrent (Level);
			}
			set {
				_healthRegeneration.Bonus = value;
			}
		}

		public float Armor {
			get {
				return _Armor.GetCurrent (Level);
			}
			set {
				_Armor.Bonus = value;
			}
		}

		public float SpecialResist {
			get {
				return _specialResist.GetCurrent (Level);
			}
			set {
				_specialResist.Bonus = value;
			}
		}

		public float ArmorPenFlat {
			get {
				return _armorPenFlat.GetCurrent (Level);
			}
			set {
				_armorPenFlat.Bonus = value;
			}
		}

		public float ArmorPenPercent {
			get {
				return _armorPenPercent.GetCurrent (Level);
			}
			set {
				_armorPenPercent.Bonus = value;
			}
		}

		public float SpecialResistPenFlat {
			get {
				return _specialResistPenFlat.GetCurrent (Level);
			}
			set {
				_specialResistPenFlat.Bonus = value;
			}
		}

		public float SpecialResistPenPercent {
			get {
				return _specialResistPenPercent.GetCurrent (Level);
			}
			set {
				_specialResistPenPercent.Bonus = value;
			}
		}

		public int MaxLevel {
			get{ return _maxLevel; }
			set{ _maxLevel = value;}
		}


		public float CheckAndSetLevel(float Value){
			if(Level == MaxLevel)return 0;
			float Hold = Value;
			_totalExpierence += Value - _Expierence;

			while(Hold - ExpierenceCurve[Level] >= 0){
				float levelDiff = Value - ExpierenceCurve[Level];

				Hold = levelDiff;
				++Level;
				if(Level == MaxLevel){
					Hold = 0;
					break;
				}
			}
			return Hold;
		}
		
		public UnitStats()
		{
			_Expierence = 0;
			_totalExpierence = 0;
			_Level.Base = 1;
			_maxLevel = 18;
			_Gold = 0;
			ExpierenceCurve = new float[_maxLevel];
		}
		public UnitStats(bool SetDefaultUnit)
		{
			_Expierence = 0;
			_totalExpierence = 0;
			_Level.Base = 1;
			_maxLevel = 18;
			_Gold = 0;
			ExpierenceCurve = new float[_maxLevel];

			if(SetDefaultUnit) {
				_maxHealth.Base = 1000f;
				CurrentHealth = 1000f;


				ExpierenceCurve = new float[]{0, 280,380,480,580,680,780,880,980,1080,1180,1280,1380,1480,1580,1680,1780,1880};
				_expierenceOnDeath.Base = 200f;
				_goldOnDeath.Base = 200f;

				//

				_moveSpeed.Base = 20f;
		
				_attackDamage.Base = 80f;
				_attackDamage.PerLevel = 10f;

				_attackSpeed.Base = 1f;
				_attackSpeed.PerLevel = _attackSpeed.SetPerLevelToPercentOfBase (0.05f);

				_attackRange.Base = 20f;

				_maxHealth.Base = 500f;
				_maxHealth.PerLevel = 50f;
				_healthRegeneration.Base = 2.4f;
				_healthRegeneration.PerLevel = .3f;
				_currentHealth = _maxHealth.GetCurrent ();

				_Armor.Base = 30f;
				_Armor.PerLevel = 3f;

				_specialResist.Base = 15f;
				_specialResist.PerLevel = 1f;



				_goldGeneration.Base = 1.6f;

			}
		}
		public static UnitStats Add(UnitStats u1, UnitStats u2){
			//TODO: Add a way for them to add more then just the bonus together.
			UnitStats Return = new UnitStats();
			List<FieldInfo> Fields = GetListOfFields(Return);

			foreach(FieldInfo field in Fields){
				if(field.GetType() == typeof(Stat)){
					float u1val = (field.GetValue(u1) as Stat).Bonus;
					float u2val = (field.GetValue(u2) as Stat).Bonus;
					Stat returnVal = (field.GetValue (Return) as Stat);
					returnVal.Bonus = u1val + u2val;
					field.SetValue(Return, returnVal);
				}
			}

			return Return;
		}
		public static List<FieldInfo> GetListOfFields(object atype){
			if (atype == null) return new List<FieldInfo>();
			Type t = atype.GetType();
			var flb = BindingFlags.Instance & BindingFlags.NonPublic;
			FieldInfo[] props = t.GetFields();
			var dict = new List<FieldInfo>();
			foreach (FieldInfo prp in props)
			{
				//object value = prp.GetValue(atype);
				dict.Add(prp);
			}
			return dict;
		}
		public static List<PropertyInfo> GetListOfProperties(object atype){
			if (atype == null) return new List<PropertyInfo>();
			Type t = atype.GetType();
			PropertyInfo[] props = t.GetProperties();
			var dict = new List<PropertyInfo>();
			foreach (PropertyInfo prp in props)
			{
				//object value = prp.GetValue(atype);
				dict.Add(prp);
			}
			return dict;
		}
		public string GetNiceString(bool GetUnchangedValues){
			var fieldlist = GetListOfFields(this);
			string Return = "";
			if(GetUnchangedValues){
				foreach (var item in fieldlist) {
					if(item.GetValue(this) is Stat){
						Return += (item.GetValue(this) as Stat).getNiceString(Level);
					}
				}
			}else{
				foreach (var item in fieldlist) {
					if(item.GetValue(this) is Stat){
						Stat itemVal = (item.GetValue(this) as Stat);
						switch(item.Name){
							case "_Level":
								if(itemVal.Base != 1){
									Return += itemVal.getNiceString(Level);
								}
								break;
							default:
								if(itemVal != new Stat(itemVal.Name)){
									Return += itemVal.getNiceString(Level);
								}
								break;
						}
					}
				}
			}
			return Return;
		}

		public string GetString(){
			string Result = "";
			
			
			if(_moveSpeed){
				Result += MoveSpeed + " Move Speed\n";
			}
			if(_attackDamage){
				Result += AttackDamage + " Attack Damage\n";
			}
			if(_attackSpeed){
				Result += AttackSpeed + " Attack Speed\n";
			}

			if(_attackRange){
				Result += AttackRange + " Attack Range\n";
			}

			if(_Level){
				Result += Level + " Level(s)\n";
			}

			return Result;
		}
		public static UnitStats operator +(UnitStats c1, UnitStats c2) 
		{
			UnitStats Return = new UnitStats();
			Return._attackDamage = c1._attackDamage + c2._attackDamage;
			Return._Level = c1._Level + c2._Level;
			Return._moveSpeed = c1._moveSpeed + c2._moveSpeed;
			return Return;
		}

		public static UnitStats operator -(UnitStats c1, UnitStats c2)
		{
			UnitStats Return = new UnitStats();
			Return._attackDamage = c1._attackDamage - c2._attackDamage;
			Return._Level = c1._Level - c2._Level;
			Return._moveSpeed = c1._moveSpeed - c2._moveSpeed;
			return Return;
		}

		public static bool operator false (UnitStats c1)
		{
			if ((!c1._attackDamage) && (!c1._Level) && (!c1._moveSpeed) && (!c1._attackRange) && (!c1._attackSpeed)) {
				return true;
			} else {
				return false;
			}
		}

		public static bool operator true (UnitStats c1)
		{
			if (c1._attackDamage) {
				return true;
			} else if (c1._Level) {
				return true;
			} else if (c1._moveSpeed) {
				return true;
			} else if (c1._attackRange) {
				return true;
			} else if (c1._attackSpeed) {
				return true;
			} else {
				return false;
			}
		}

		public static void StreamSend(BitStream stream, UnitStats u1, UnitStats u2){
			var fieldList = GetListOfFields(u1);

			foreach (var item in fieldList) {
				if(item.GetValue(u1).GetType() == typeof(Stat)){
					Stat s1 = (item.GetValue(u1) as Stat);
					Stat s2 = (item.GetValue(u2) as Stat);
					var statFieldList = GetListOfFields(s1);
					foreach (var stat in statFieldList) {
						if(stat.GetType() == typeof(float)){
						float v1 = (float)stat.GetValue(s1);
						float v2 = (float)stat.GetValue(s2);
						if(!v1.Equals(v2)){
							stream.Serialize(ref v2);
						}
						}
					}
				}else if(item.GetValue(u1).GetType() == typeof(float)){
						float v1 = (float)item.GetValue(u1);
						float v2 = (float)item.GetValue(u2);
						if(!v1.Equals(v2)){
							stream.Serialize(ref v2);
						}
				}

			}
		}

		public static void StreamRecieve ( BitStream stream, UnitStats stats )
		{
			var fieldList = GetListOfFields (stats);
			foreach (var item in fieldList) {
				if(item.GetValue(stats).GetType () == typeof(Stat)) {
					Stat s1 = (item.GetValue(stats) as Stat);
					var statFieldList = GetListOfFields(s1);
					foreach (var stat in statFieldList) {
						if(stat.GetType() == typeof(float)){
							float v2 = (float)stat.GetValue(s1);
							stream.Serialize(ref v2);
							stat.SetValue(s1, v2);
						}
					}
				}else if(item.GetValue(stats).GetType() == typeof(float)){
					float v2 = (float)(item.GetValue(stats));
					stream.Serialize(ref v2);
					item.SetValue(stats, v2);
				}
			}
		}


	}
}