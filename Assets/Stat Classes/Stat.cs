//using System;

namespace SuddenlyEntertainment
{
	/// <summary>
	/// A class to store stats!
	/// </summary>
	[System.Serializable]
	public class Stat
	{
		/// <summary>
		/// The name of the stat.
		/// </summary>
		public string Name;
		
		/// <summary>
		/// This is the base amount for this stat, when the level = 0;
		/// </summary>
		public double Base;

		/// <summary>
		/// This is for the linear bonus to this stat given for every level.
		/// </summary>
		public double PerLevel;

		/// <summary>
		/// This is the bonus to the stat gotten through other means other than level.
		/// </summary>
		public double Bonus;

		/// <summary>
		/// Gets the current (total) value of the stat.
		/// NOTE:  This is without level calculation (We don't know the level) add the level to the param
		/// </summary>
		/// <returns>
		/// The Current Value of the Stat of type <see cref="{T}">.
		/// </returns>
		public virtual double GetCurrent ()
		{
			return Base + Bonus;
		}

		/// <summary>
		/// Gets the current (total) value of the stat.
		/// </summary>
		/// <returns>
		/// The current.
		/// </returns>
		/// <param name='Level'>
		/// The Unit's Level.  This is necessary because individual stats do not hold level.  So to calculate it, the stat needs the level added.
		/// </param>
		public virtual double GetCurrent (int Level)
		{	
			return Base + Bonus + CurrentLevelBonus(Level);
		}
		/// <summary>
		/// Gets the Stat without bonus added to it.
		/// </summary>
		/// <returns>
		/// The value of the stat without the bonus added to it.
		/// </returns>
		/// <param name='Level'>
		/// Level.
		/// </param>
		public virtual double GetWithoutBonus (int Level)
		{
			return Base + (PerLevel * Level);
		}

		/// <summary>
		/// A shortcut function to quickly get the current bonus.  All it does is multiply PerLevel by Level
		/// </summary>
		/// <returns>
		/// The current level bonus.
		/// </returns>
		/// <param name='Level'>
		/// Level.
		/// </param>
		public virtual double CurrentLevelBonus (int Level)
		{
			return PerLevel * Level;
		}

		public static Stat operator +(Stat c1, Stat c2) 
		{
			Stat Result = new Stat(c1.Name);
			Result.Base = c1.Base + c2.Base;
			Result.PerLevel = c1.PerLevel + c2.PerLevel;
			Result.Bonus = c1.Bonus + c2.Bonus;
			return Result;
		}

		public static Stat operator -(Stat c1, Stat c2) 
		{
			Stat Result = new Stat(c1.Name);
			Result.Base = c1.Base - c2.Base;
			Result.PerLevel = c1.PerLevel - c2.PerLevel;
			Result.Bonus = c1.Bonus - c2.Bonus;
			return Result;
		}

		public static bool operator true (Stat c1)
		{
			if (c1.Name == "") {
				return false;
			} else {
				return true;
			}
		}

		public static bool operator false (Stat c1)
		{
			if (c1.Name != "") {
				return false;
			} else {
				return true;
			}
		}

		public static bool operator ! (Stat c1)
		{
			if (c1) {
				return false;
			} else {
				return true;
			}
		}

		public double SetBonusToPercentOfBase (double Percent)
		{
			double bonus = Base * Percent;
			Bonus = bonus;
			return Bonus;
		}
		public double SetPerLevelToPercentOfBase (double Percent)
		{
			double perlevel = Base * Percent;
			PerLevel = perlevel;
			return PerLevel;
		}
		public double AddPercentOfBaseToBonus (double Percent)
		{
			double bonus = Base * Percent;
			Bonus += bonus;
			return Bonus;
		}
		public double AddPercentOfBaseToPerLevel (double Percent)
		{
			double perlevel = Base * Percent;
			PerLevel += perlevel;
			return PerLevel;
		}
		public Stat ()
		{
			Name = "";
			Base = 0;
			PerLevel = 0;
			Bonus = 0;
		}


		public Stat(string name){
			Name = name;
			Base = 0;
			PerLevel = 0;
			Bonus = 0;
		}
	}
}

