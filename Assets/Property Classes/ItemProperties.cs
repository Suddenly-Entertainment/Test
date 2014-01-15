using System;

namespace SuddenlyEntertainment
{
	[System.Serializable]
	public class ItemProperties
	{
		public string Name;

		public double Cost;

		public string[] BuildsOutOf;

		public UnitStats StatChanges;

		public bool hasStatChanges;

		public bool hasActive;
		public bool hasUniqueActive;

		public bool hasPassive;
		public bool hasUniquePassive;

		public PassiveProperties[] Passives;
		
		public PassiveProperties[] UniquePassives;


		public AbilityProperties ActiveProperties;

		public ItemProperties ()
		{
			Name = "TestName";

			Cost = 360;

			BuildsOutOf = new string[]{""};

			hasActive = false;
			hasUniqueActive = false;

			hasPassive = false;
			hasUniquePassive = false;

			StatChanges = new UnitStats();

			Passives = new PassiveProperties[1];
			UniquePassives = new PassiveProperties[1];


			ActiveProperties = new AbilityProperties();
		}
	}

	public class ProjectileProperties
	{
		public bool Targeted;
		public float Range;
		public float Damage;
		public UnitType[] collideTags;

		public ProjectileProperties()
		{
			Targeted = true;
			Range = 600;
			Damage = 100;
			collideTags = new UnitType[]{UnitType.Nonstructure};
		}
	}
}

