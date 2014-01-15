using System;

namespace SuddenlyEntertainment
{
	/// <summary>
	/// Stores Stats for Units.
	/// </summary>
	[System.Serializable]
	public class UnitStats
	{

		public Stat _Level = new Stat();
		public Stat _moveSpeed = new Stat();
		public Stat _attackDamage = new Stat();
		public Stat _attackSpeed = new Stat();
		public Stat _attackRange = new Stat();

		public int Level {
			get {
				return (int)_Level.GetCurrent();
			}
			set {
				_Level.Bonus = (double)value;
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

		public UnitStats ()
		{
			
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