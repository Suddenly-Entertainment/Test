using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

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
		private Stat _Level = new Stat("Unit Level");
		private int _maxLevel;
		private Stat _moveSpeed = new Stat("Movement Speed");
		private Stat _attackDamage = new Stat("Basic Attack Damage");
		private Stat _attackSpeed = new Stat("Basic Attack Speed");
		private Stat _attackRange = new Stat("Basic Attack Range");

		private Stat _maxHealth = new Stat("Maximum Health");
		private Stat _healthRegeneration = new Stat("Health Regeneration");
		private double _currentHealth;

		private Stat _Armor = new Stat("Armor");
		private Stat _specialResist = new Stat("Special Resist");

		private Stat _armorPenFlat = new Stat("Armor Penetration Flat");
		private Stat _armorPenPercent = new Stat("Armor Penetration Percent");

		private Stat _specialResistPenFlat = new Stat("Special Resist Penetration Flat");
		private Stat _specialResistPenPercent = new Stat("Special Resist Penetration Percent");

		private double _Expierence;
		private double _totalExpierence;
		public  double[] ExpierenceCurve;
		private Stat _expierenceOnDeath = new Stat("Expierence Granted On Death");


		private Stat _goldOnDeath = new Stat("Gold Granted On Death");


		private double _Gold;

		private Stat _goldGeneration = new Stat("Gold Generation");

		public int Level {
			get {
				return (int)_Level.GetCurrent();
			}
			set {
				if(value <= _maxLevel - 1)
				_Level.Bonus = (double)value;

				if(onLevel != null)
					onLevel(this, Level);
			}
		}


		public double MoveSpeed {
			get {
				return _moveSpeed.GetCurrent ();
			}
			set {
				_moveSpeed.Bonus = value;
			}
		}

		public double B_MoveSpeed {
			get {
				return _moveSpeed.Base;
			}
			set {
				_moveSpeed.Base = value;
			}
		}


		public double AttackDamage {
			get {
				return _attackDamage.GetCurrent(Level);
			}
			set {
				_attackDamage.Bonus = value;
			}
		}

		public double AttackSpeed {
			get {
				return _attackSpeed.GetCurrent (Level);
			}
			set {
				_attackSpeed.SetBonusToPercentOfBase(value);
			}
		}

		public double AttackRange {
			get {
				return _attackRange.GetCurrent (Level);
			}
			set {
				_attackRange.Bonus = value;
			}
		}

		public double MaxHealth {
			get{
				return _maxHealth.GetCurrent(Level);
			}
			set{
				_maxHealth.Bonus = value;
			}
		}

		public double CurrentHealth {
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

		public double Expierence {
			get {
				return _Expierence;
			}
			set {
				if(value <= 0)return;
				_Expierence = CheckAndSetLevel(value);
			}
		}

		public double GoldGeneration {
			get{
				return _goldGeneration.GetCurrent(Level);
			}
			set{
				_goldGeneration.Bonus = value;
			}
		}

		public double GoldOnDeath {
			get {
				return _goldOnDeath.GetCurrent (Level);
			}
			set {
				_goldOnDeath.Bonus = value;
			}
		}

		public double ExpierenceOnDeath {
			get {
				return _expierenceOnDeath.GetCurrent (Level);
			}
			set {
				_expierenceOnDeath.Bonus = value;
			}
		}

		public double Gold{
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

		public double HealthRegeneration {
			get {
				return _healthRegeneration.GetCurrent (Level);
			}
			set {
				_healthRegeneration.Bonus = value;
			}
		}

		public double Armor {
			get {
				return _Armor.GetCurrent (Level);
			}
			set {
				_Armor.Bonus = value;
			}
		}

		public double SpecialResist {
			get {
				return _specialResist.GetCurrent (Level);
			}
			set {
				_specialResist.Bonus = value;
			}
		}

		public double ArmorPenFlat {
			get {
				return _armorPenFlat.GetCurrent (Level);
			}
			set {
				_armorPenFlat.Bonus = value;
			}
		}

		public double ArmorPenPercent {
			get {
				return _armorPenPercent.GetCurrent (Level);
			}
			set {
				_armorPenPercent.Bonus = value;
			}
		}

		public double SpecialResistPenFlat {
			get {
				return _specialResistPenFlat.GetCurrent (Level);
			}
			set {
				_specialResistPenFlat.Bonus = value;
			}
		}

		public double SpecialResistPenPercent {
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


		private double CheckAndSetLevel(double Value){
			if(Level == MaxLevel)return 0;
			double Hold = Value;
			_totalExpierence += Value - _Expierence;

			while(Hold - ExpierenceCurve[Level] >= 0){
				double levelDiff = Value - ExpierenceCurve[Level];

				Hold = levelDiff;
				++Level;
				if(Level == MaxLevel){
					Hold = 0;
					break;
				}
			}
			return Hold;
		}
		

		public UnitStats ()
		{
			_maxHealth.Base = 1000;
			CurrentHealth = 1000;

			_Expierence = 0;
			_totalExpierence = 0;
			ExpierenceCurve = new double[]{0, 280,380,480,580,680,780,880,980,1080,1180,1280,1380,1480,1580,1680,1780,1880};
			_expierenceOnDeath.Base = 200;
			_goldOnDeath.Base = 200;

			//
			_maxLevel = 18;
			_Level.Base = 1;
			_moveSpeed.Base = 20;
		
			_attackDamage.Base = 80;
			_attackDamage.PerLevel = 10;

			_attackSpeed.Base = 1;
			_attackSpeed.PerLevel = _attackSpeed.SetPerLevelToPercentOfBase(0.05);

			_attackRange.Base = 20;

			_maxHealth.Base = 500;
			_maxHealth.PerLevel = 50;
			_healthRegeneration.Base = 2.4;
			_healthRegeneration.PerLevel = .3;
			_currentHealth = _maxHealth.GetCurrent();

			_Armor.Base = 30;
			_Armor.PerLevel = 3;

			_specialResist.Base = 15;
			_specialResist.PerLevel = 1;

			_Gold = 0;

			_goldGeneration.Base = 1.6;


		}
		public static UnitStats Add(UnitStats u1, UnitStats u2){
			//TODO Add a way for them to add more then just the bonus together.
			UnitStats Return = new UnitStats();
			List<FieldInfo> Fields = GetListOfFields(Return);

			foreach(FieldInfo field in Fields){
				if(field.GetType() == typeof(Stat)){
					double u1val = (field.GetValue(u1) as Stat).Bonus;
					double u2val = (field.GetValue(u2) as Stat).Bonus;
					Stat returnVal = (field.GetValue (Return) as Stat);
					returnVal.Bonus = u1val + u2val;
					field.SetValue(Return, returnVal);
				}
			}

			return Return;
		}
		private static List<FieldInfo> GetListOfFields(object atype){
			if (atype == null) return new List<FieldInfo>();
			Type t = atype.GetType();
			FieldInfo[] props = t.GetFields();
			var dict = new List<FieldInfo>();
			foreach (FieldInfo prp in props)
			{
				//object value = prp.GetValue(atype);
				dict.Add(prp);
			}
			return dict;
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


	}
}