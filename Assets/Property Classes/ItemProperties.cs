using System;

namespace SuddenlyEntertainment
{
	/// <summary>
	/// Item properties.
	/// </summary>
	[System.Serializable]
	public class ItemProperties : Properties
	{
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
		/// <summary>
			/// Initializes a new instance of the <see cref="SuddenlyEntertainment.ItemProperties"/> class.
		/// </summary>
		public ItemProperties ()
		{
			Name = "Item Properties";

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
}

