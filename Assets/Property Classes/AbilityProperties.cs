using System;

namespace SuddenlyEntertainment
{
	public class AbilityProperties
	{
		public string Name;

		public double Cooldown;

		public bool hasPassive;
		public bool hasActive;
		public bool hasAOE;
		
		public bool isProjectile;
		public bool isStatModifier;
		public bool isToggle;

		public bool createsWall;

		public ProjectileProperties projectileProperties;
		public UnitStats StatChanges;

		public AbilityProperties ()
		{
			Name = "TestName";

			Cooldown = 10;

			hasPassive = false;
			hasActive = true;
			hasAOE = false;

			isProjectile = false;
			isStatModifier = false;
			isToggle = true;

			createsWall = false;

			projectileProperties = new ProjectileProperties();
			StatChanges = new UnitStats();
		}
	}
	/*
	 * (1/ SqRt(2)) * (SqRt(2)/SqRt(2)) = SqRt(2)/SqRt(4) = SqRt(2)/2;
	 */
}

