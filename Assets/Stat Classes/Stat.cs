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
		public float Base;

		/// <summary>
		/// This is for the linear bonus to this stat given for every level.
		/// </summary>
		public float PerLevel;

		/// <summary>
		/// This is the bonus to the stat gotten through other means other than level.
		/// </summary>
		public float Bonus;

		/// <summary>
		/// The penalty to the stats.
		/// </summary>
		public float Penalty;

		public virtual float GetCurrent()
		{
			return Base + Bonus - Penalty;
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
		public virtual float GetCurrent (int Level)
		{	
			return Base + Bonus - Penalty + CurrentLevelBonus(Level);
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
		public virtual float GetWithoutBonus (int Level)
		{
			return Base + CurrentLevelBonus(Level);
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
		public virtual float CurrentLevelBonus (int Level)
		{
			return PerLevel * Level;
		}

		public static Stat operator +(Stat c1, Stat c2) 
		{
			Stat Result = new Stat(c1.Name);
			Result.Base = c1.Base + c2.Base;
			Result.PerLevel = c1.PerLevel + c2.PerLevel;
			Result.Bonus = c1.Bonus + c2.Bonus;
			Result.Penalty = c1.Penalty + c2.Penalty;
			return Result;
		}

		public static Stat operator -(Stat c1, Stat c2) 
		{
			Stat Result = new Stat(c1.Name);
			Result.Base = c1.Base - c2.Base;
			Result.PerLevel = c1.PerLevel - c2.PerLevel;
			Result.Bonus = c1.Bonus - c2.Bonus;
			Result.Penalty = c1.Penalty - c2.Penalty;
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

		public float SetBonusToPercentOfBase (float Percent)
		{
			float bonus = Base * Percent;
			Bonus = bonus;
			return Bonus;
		}

		public float SetPenaltyToPercentOfBase (float Percent)
		{
			float penalty = Base * Percent;
			Penalty = penalty;
			return penalty;
		}

		public float SetPerLevelToPercentOfBase (float Percent)
		{
			float perlevel = Base * Percent;
			PerLevel = perlevel;
			return PerLevel;
		}
		/// <summary>
		/// Adds the percent of base to bonus.
		/// </summary>
		/// <returns>
		/// The new bonus.
		/// </returns>
		/// <param name='Percent'>
		/// Percent
		/// </param>
		public float AddPercentOfBaseToBonus (float Percent)
		{
			float bonus = Base * Percent;
			Bonus += bonus;
			return Bonus;
		}

		public float AddPercentOfBaseToPenalty (float Percent)
		{
			float penalty = Base * Percent;
			penalty += penalty;
			return penalty;
		}
		public float AddPercentOfBaseToPerLevel (float Percent)
		{
			float perlevel = Base * Percent;
			PerLevel += perlevel;
			return PerLevel;
		}
		public Stat ()
		{
			Name = "";
			Base = 0;
			PerLevel = 0;
			Bonus = 0;
			Penalty = 0;
		}
		public Stat(string name){
			Name = name;
			Base = 0;
			PerLevel = 0;
			Bonus = 0;
			Penalty = 0;
		}
		public Stat(string name, float baseV){
			Name = name;
			Base = baseV;
			PerLevel = 0;
			Bonus = 0;
			Penalty = 0;
		}
		public Stat(string name, float baseV, float perLevel){
			Name = name;
			Base = baseV;
			PerLevel = perLevel;
			Bonus = 0;
			Penalty = 0;
		}
		public Stat(string name, float baseV, float perLevel, float bonus){
			Name = name;
			Base = baseV;
			PerLevel = perLevel;
			Bonus = bonus;
			Penalty = 0;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="SuddenlyEntertainment.Stat"/> class.
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		/// <param name='baseV'>
		/// The Base Value.
		/// </param>
		/// <param name='perLevel'>
		/// The value added Per level.
		/// </param>
		/// <param name='bonus'>
		/// The Bonus Value.
		/// </param>
		/// <param name='penalty'>
		/// The Penalty value. 
		/// Note:  This value is SUBTRACTED from the rest to get current, so a positive value DECREASES the total.
		/// </param>
		public Stat(string name, float baseV, float perLevel, float bonus, float penalty){
			Name = name;
			Base = baseV;
			PerLevel = perLevel;
			Bonus = bonus;
			Penalty = penalty;
		}
		public string getNiceString(){
			string Return = "";
			Return = Name + ": " + GetCurrent() + "\n";
			return Return;
		}
		public string getNiceString(int Level){
			string Return = "";
			Return = Name + ": " + GetCurrent(Level) + "\n";
			return Return;
		}
	}
}

