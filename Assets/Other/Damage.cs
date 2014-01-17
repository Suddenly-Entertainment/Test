//  	File Name			:	Damage.cs
//
// 	Created By		:	robo
// 	Created On		:	1/16/2014
//
// 	Purpose			:	<${Purpose}>
using System;
using UnityEngine;

namespace SuddenlyEntertainment
{
	public class Damage
	{
		public GameObject Attacker;
		public string AttackerName;
		public string AttackerTag;
		public Teams AttackerTeam;

		public bool isPlayer;
		public string AttackerGUID;

		public float TrueDamage;
		public float PhysicalDamage;
		public float SpecialDamage;

		public float ArmorPenFlat;
		public float ArmorPenPercent;

		public float SpecialResistPenFlat;
		public float SpecialResistPenPercent;

		public Damage()
		{
			AttackerTeam = Teams.NA;
			isPlayer = false;

			TrueDamage = 0;
			PhysicalDamage = 0;
			SpecialDamage = 0;

			ArmorPenFlat = 0;
			ArmorPenPercent = 0;

			SpecialResistPenFlat = 0;
			SpecialResistPenPercent = 0;
		}
	}
}

