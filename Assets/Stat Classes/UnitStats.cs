using System;

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
		public event EventHandler onDeath;
		public Stat _Level = new Stat();
		public Stat _moveSpeed = new Stat();
		public Stat _attackDamage = new Stat();
		public Stat _attackSpeed = new Stat();
		public Stat _attackRange = new Stat();

		public Stat _maxHealth = new Stat();
		public Stat _healthRegeneration = new Stat();
		public double _currentHealth;

		public double _Expierence;
		public double _totalExpierence;
		public double[] _expierenceCurve;
		public Stat _expierenceOnDeath;

		public Stat _goldOnDeath;

		public double _Gold;
		public Stat _goldGeneration;

		public int Level {
			get {
				return (int)_Level.GetCurrent();
			}
			set {
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
						onDeath(this, EventArgs.Empty);
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
				_healthRegeneration = value;
			}
		}

		private double CheckAndSetLevel(double Value){
			double Hold = Value;
			_totalExpierence += Value - _Expierence;
			while(Hold - _expierenceCurve[Level] >= 0){
				double levelDiff = Value - _expierenceCurve[Level];

				Hold = levelDiff;
				++Level;
			}
			return Hold;
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