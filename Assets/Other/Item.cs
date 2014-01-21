using System;

namespace SuddenlyEntertainment
{
	public delegate void UpSet();
	public delegate UnitStats StatChangers(UnitStats Stats);

	public delegate void Collision(Collision collision);
	public delegate void Triggers(int collider);
	/// <summary>
	/// Item.
	/// </summary>
	[System.Serializable]
	public class Item
	{
		public ItemProperties properties;

		public double Cost;

		public string Name;

		public UpSet Setup;

		public StatChangers Changes;
		public UpSet Passives;
		public UpSet UniquePassives;

		public UpSet Active;
		public UpSet UniqueActive;

		public Collision OnCollision;
		public Triggers OnTrigger;

		public Item (ItemProperties Props)
		{
			properties = Props;

			Name = properties.Name;
			Cost = properties.Cost;

			SetupDelegates();
		}

		public void SetupDelegates ()
		{
			if (properties.hasStatChanges) Changes += new StatChangers(AddStatChanges);

			if(properties.hasPassive) SetupPassives(false);

			if(properties.hasUniquePassive) SetupPassives(true);




		}
		public void SetupPassives (bool Unique)
		{
			/*
			if (Unique) {
				foreach(PassiveProperties Passive in Properties.UniquePassives){
					//TODO: Code this.
				}
			} else {
				foreach(PassiveProperties Passive in Properties.Passives){
					//TODO: Code this.
				}
			}*/
		}
		public UnitStats AddStatChanges(UnitStats Stats){
			//TODO: Add some checks.
			return UnitStats.Add(Stats, properties.StatChanges);

		}


	}
}

