//  	File Name			:	EventOnUnitDeathArgs.cs
//
// 	Created By		:	robo
// 	Created On		:	1/15/2014
//
// 	Purpose			:	<${Purpose}>
using System;

namespace SuddenlyEntertainment
{
	public class OnUnitDeathEventArgs : EventArgs
	{
		public string UnitName;
		public float goldGained;
		public float expierenceGained;
		public int Level;
	}
}

