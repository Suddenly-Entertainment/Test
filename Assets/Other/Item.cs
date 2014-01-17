using System;

namespace SuddenlyEntertainment
{
	public delegate void UpSet();
	public delegate void StatChangers(ref UnitStats Stats);

	public delegate void Collision(Collision collision);
	public delegate void Triggers(int collider);

	[System.Serializable]
	public class Item
	{
		public ItemProperties Properties;

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
			Properties = Props;

			Name = Properties.Name;
			Cost = Properties.Cost;

			SetupDelegates();
		}

		public void SetupDelegates ()
		{
			if (Properties.hasStatChanges) Changes += new StatChangers(AddStatChanges);

			if(Properties.hasPassive) SetupPassives(false);

			if(Properties.hasUniquePassive) SetupPassives(true);




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
		public void AddStatChanges(ref UnitStats Stats){
			//TODO: Something here.
			Stats += Properties.StatChanges;
		}


	}
}

