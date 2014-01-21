//  	File Name			:	ProjectileProperties.cs
//
// 	Created By		:	RoboThePichu
// 	Created On		:	1/21/2014
//
// 	Purpose			:	<${Purpose}>

using System;

namespace SuddenlyEntertainment
{	
	/// <summary>
	/// Projectile properties.
	/// </summary>
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
